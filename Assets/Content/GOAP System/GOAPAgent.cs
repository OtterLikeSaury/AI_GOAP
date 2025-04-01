using Sirenix.OdinInspector;
using System.Collections.Generic;

public class GOAPAgent : SerializedMonoBehaviour
{
    [LabelText("目标")]public GOAPGoals goals=new GOAPGoals();
    [LabelText("局部状态")] public GOAPStates states=new GOAPStates();
    [LabelText("全部行为")] public GOAPActions actions=new GOAPActions();
    [LabelText("计划")] public GOAPPlan plan=new GOAPPlan();

    public IGOAPOwner owner { get; private set; }

    public void Init(IGOAPOwner owner)
    {
        this.owner = owner;
        actions.Init(this, owner);
        goals.Init(this, owner);
    }

    public void OnUpdate()
    {
        if (owner == null) return;

        if (!plan.runing)          //当计划无法执行则再规划计划
        {
            //计划再执行就不需要构件任务了
            SortedList<string, GOAPGoals.Goal> sortedGoals = goals.UpdateGoals();
            foreach (KeyValuePair<string, GOAPGoals.Goal> item in sortedGoals)
            {
                //优先级不是负数，同时可以基于这个目标生成计划
                if (item.Value.piority > 0 && GeneratePlan(item.Key,out GOAPPlanNode targetNode))
                {
                    //Debug.Log("计划构件成功：" + item.Key);
                    RunPlan(item.Key,targetNode);
                    break;
                }
            }

        }
        else
        {
            //如果当前目标可以被切断，尝试找更高优先级的目标执行
            GOAPGoals.Goal currentGoal = goals.dic[plan.goalName];
            if (currentGoal.canBeBreak)
            {
                SortedList<string, GOAPGoals.Goal> sortedGoals = goals.UpdateGoals();
                foreach (KeyValuePair<string, GOAPGoals.Goal> item in sortedGoals)
                {
                    if (item.Key!=plan.goalName
                        &&item.Value.canBreak
                        &&item.Value.piority>currentGoal.piority
                        && GeneratePlan(item.Key, out GOAPPlanNode targetNode))
                    {
                        //Debug.Log("目标替换为优先级更高的：" + item.Key+"并构件计划");
                        StopPlan();
                        RunPlan(item.Key, targetNode);
                    }
                }
            }

            plan.OnUpdate();
        }

        if(plan.runing) plan.OnUpdate();
    }

    private void OnDestroy()
    {
        plan.OnDestroy();
    }

    #region 状态
    public void ApplyEffect(GOAPTypeAndComparer effect)
    {
        states.ApplyEffect(effect);
    }

    /// <summary>
    /// 针对某个代理的某个状态类型，是否满足前置条件
    /// </summary>
    /// <param name="stateType">目标状态类型</param>
    /// <param name="stateComparer">目标比较器</param>
    /// <returns></returns>
    public bool CheckStateForPrecondition(GOAPStateType stateType, GOAPStateComparer stateComparer)
    {
        if (GOAPGlobal.instance.TryGetGlobalState(stateType,out GOAPStateBase state))
        {
            return state.CompareForPrecondition(stateComparer);
        }
        else
        {
            return states.CheckStateForPrecondition(stateType, stateComparer);
        }
    }

    public bool CheckStateForEffect(GOAPStateType stateType, GOAPStateComparer stateComparer)
    {
        if (GOAPGlobal.instance.TryGetGlobalState(stateType, out GOAPStateBase state))
        {
            return state.CompareForEffect(stateComparer);
        }
        else
        {
            return states.CheckStateForEffect(stateType, stateComparer);
        }
    }
    #endregion

    #region 生成计划

    /// <summary>
    /// 任务实时优先级排序规则
    /// </summary>
    private class PlanNodePriorityComparer : IComparer<GOAPPlanNode>
    {
        public int Compare(GOAPPlanNode x, GOAPPlanNode y)
        {
            return y.action.priority.CompareTo(x.action.priority);
        }
    }

    private SortedSet<GOAPPlanNode> GetNodeSortedSet()
    {
        SortedSet<GOAPPlanNode> nodes = GOAPObjectPool.Get<SortedSet<GOAPPlanNode>>();
        if (nodes == null) nodes = new SortedSet<GOAPPlanNode>(new PlanNodePriorityComparer());
        return nodes;
    }

    /// <summary>
    /// 节点回收函数
    /// </summary>
    /// <param name="nodes"></param>
    private void RecycleNodeSortedSet(SortedSet<GOAPPlanNode> nodes)
    {
        foreach (GOAPPlanNode item in nodes)
        {
            item.Destroy();
            //GOAPObjectPool.Recycle(item);   //这里是绝对的要点，因为用池子回收的节点后续出来Action还是为null因此直接销毁即可
        }

        nodes.Clear();
        GOAPObjectPool.Recycle(nodes);
    }

    /// <summary>
    /// 找到符合某个效果的所有行为并形成计划节点
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    private SortedSet<GOAPPlanNode> GetPlanNodeByEffectStateType(GOAPStateType targetType,GOAPStateComparer comparer)
    {
        SortedSet<GOAPPlanNode> stateTypeNodes = GetNodeSortedSet();
        if (actions.actionEffectDic.TryGetValue(targetType,out List<GOAPActionBase> actionList))
        {
            foreach (GOAPActionBase action in actionList)
            {
                foreach (GOAPTypeAndComparer effect in action.effects)
                {
                    if (effect.stateType==targetType && effect.stateComparer.EqualsComparer(comparer))  //行为的影响结果是否满足传入的比较器需求
                    {
                        action.UpdatePriority();  //行为满足当前目标需求，更新其优先级方便排序\
                        GOAPPlanNode node = GOAPObjectPool.GetOrNew<GOAPPlanNode>();  //从对象池生成节点
                        node.action = action;
                        stateTypeNodes.Add(node);
                        break;
                    }
                }
            }
        }
        return stateTypeNodes;
    }

    /// <summary>
    /// 基于某个源头构建计划路径，
    /// 失败的可能性：某个环节中无法达到某个前置条件
    /// </summary>
    /// <param name="startNode"></param>
    /// <returns></returns>
    private bool TryBuildPlanPath(GOAPPlanNode startNode)
    {
        //遍历所有前置条件，必须全部满足才能构件成功
        foreach (GOAPTypeAndComparer precondition in startNode.action.preconditions)
        {
            //当前状态的满足情况
            bool check = CheckStateForPrecondition(precondition.stateType,precondition.stateComparer);

            if (!check)   //当前状态不满足，需要寻找可以满足的其他Action作为子节点
            {
                SortedSet<GOAPPlanNode> preNodes = GetPlanNodeByEffectStateType(precondition.stateType,precondition.stateComparer);
                GOAPPlanNode targetNode = null;

                foreach (GOAPPlanNode preItemNode in preNodes)
                {
                    if (preItemNode!=startNode&&TryBuildPlanPath(preItemNode))  //preItemNode!=startNode:避免自己是自己的前提
                    {
                        targetNode = preItemNode;
                        preItemNode.parent = startNode;
                        preItemNode.indexAtParent = startNode.preconditions.Count;
                        startNode.preconditions.Add(preItemNode);
                        check = true;
                        break;
                    }
                }

                if (targetNode != null) preNodes.Remove(targetNode);
                RecycleNodeSortedSet(preNodes);
                if (!check)  //意味着无法满足条件
                {
                    return false;
                }

            }
        }

        return true;
    }

    private bool GeneratePlan(string goalName,out GOAPPlanNode targetNode)
    {
        bool success = false;
        GOAPGoals.Goal goal = goals.dic[goalName];
        targetNode = null;
        //遍历全部的效果，如果全部满足则没有意义
        if (CheckStateForEffect(goal.targetState, goal.targetValue))
        {
            return false;
        }
        GOAPStateType targetStateType = goal.targetState;

        SortedSet<GOAPPlanNode> nodes = GetPlanNodeByEffectStateType(targetStateType,goal.targetValue);  //拿到符合效果的全部Action并根据优先级排序后拿到第一个优先级最高的action，清楚其它Action

        foreach (GOAPPlanNode node in nodes)
        {
            if (TryBuildPlanPath(node))
            {
                targetNode = node;
                node.parent = null;
                node.indexAtParent = 0;

                success = true;
                break;
            }
        }
        if (targetNode != null) nodes.Remove(targetNode);
        RecycleNodeSortedSet(nodes);
        return success;
    }
    #endregion

    #region 执行任务
    private void RunPlan(string goalName,GOAPPlanNode targetNode)
    {
        plan.StartRunPlan(goalName,targetNode);
    }

    public void StopPlan()
    {
        plan.StopRun();
    }
    #endregion
}
