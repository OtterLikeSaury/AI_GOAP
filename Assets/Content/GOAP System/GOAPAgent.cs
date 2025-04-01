using Sirenix.OdinInspector;
using System.Collections.Generic;

public class GOAPAgent : SerializedMonoBehaviour
{
    [LabelText("Ŀ��")]public GOAPGoals goals=new GOAPGoals();
    [LabelText("�ֲ�״̬")] public GOAPStates states=new GOAPStates();
    [LabelText("ȫ����Ϊ")] public GOAPActions actions=new GOAPActions();
    [LabelText("�ƻ�")] public GOAPPlan plan=new GOAPPlan();

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

        if (!plan.runing)          //���ƻ��޷�ִ�����ٹ滮�ƻ�
        {
            //�ƻ���ִ�оͲ���Ҫ����������
            SortedList<string, GOAPGoals.Goal> sortedGoals = goals.UpdateGoals();
            foreach (KeyValuePair<string, GOAPGoals.Goal> item in sortedGoals)
            {
                //���ȼ����Ǹ�����ͬʱ���Ի������Ŀ�����ɼƻ�
                if (item.Value.piority > 0 && GeneratePlan(item.Key,out GOAPPlanNode targetNode))
                {
                    //Debug.Log("�ƻ������ɹ���" + item.Key);
                    RunPlan(item.Key,targetNode);
                    break;
                }
            }

        }
        else
        {
            //�����ǰĿ����Ա��жϣ������Ҹ������ȼ���Ŀ��ִ��
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
                        //Debug.Log("Ŀ���滻Ϊ���ȼ����ߵģ�" + item.Key+"�������ƻ�");
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

    #region ״̬
    public void ApplyEffect(GOAPTypeAndComparer effect)
    {
        states.ApplyEffect(effect);
    }

    /// <summary>
    /// ���ĳ�������ĳ��״̬���ͣ��Ƿ�����ǰ������
    /// </summary>
    /// <param name="stateType">Ŀ��״̬����</param>
    /// <param name="stateComparer">Ŀ��Ƚ���</param>
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

    #region ���ɼƻ�

    /// <summary>
    /// ����ʵʱ���ȼ��������
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
    /// �ڵ���պ���
    /// </summary>
    /// <param name="nodes"></param>
    private void RecycleNodeSortedSet(SortedSet<GOAPPlanNode> nodes)
    {
        foreach (GOAPPlanNode item in nodes)
        {
            item.Destroy();
            //GOAPObjectPool.Recycle(item);   //�����Ǿ��Ե�Ҫ�㣬��Ϊ�ó��ӻ��յĽڵ��������Action����Ϊnull���ֱ�����ټ���
        }

        nodes.Clear();
        GOAPObjectPool.Recycle(nodes);
    }

    /// <summary>
    /// �ҵ�����ĳ��Ч����������Ϊ���γɼƻ��ڵ�
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
                    if (effect.stateType==targetType && effect.stateComparer.EqualsComparer(comparer))  //��Ϊ��Ӱ�����Ƿ����㴫��ıȽ�������
                    {
                        action.UpdatePriority();  //��Ϊ���㵱ǰĿ�����󣬸��������ȼ���������\
                        GOAPPlanNode node = GOAPObjectPool.GetOrNew<GOAPPlanNode>();  //�Ӷ�������ɽڵ�
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
    /// ����ĳ��Դͷ�����ƻ�·����
    /// ʧ�ܵĿ����ԣ�ĳ���������޷��ﵽĳ��ǰ������
    /// </summary>
    /// <param name="startNode"></param>
    /// <returns></returns>
    private bool TryBuildPlanPath(GOAPPlanNode startNode)
    {
        //��������ǰ������������ȫ��������ܹ����ɹ�
        foreach (GOAPTypeAndComparer precondition in startNode.action.preconditions)
        {
            //��ǰ״̬���������
            bool check = CheckStateForPrecondition(precondition.stateType,precondition.stateComparer);

            if (!check)   //��ǰ״̬�����㣬��ҪѰ�ҿ������������Action��Ϊ�ӽڵ�
            {
                SortedSet<GOAPPlanNode> preNodes = GetPlanNodeByEffectStateType(precondition.stateType,precondition.stateComparer);
                GOAPPlanNode targetNode = null;

                foreach (GOAPPlanNode preItemNode in preNodes)
                {
                    if (preItemNode!=startNode&&TryBuildPlanPath(preItemNode))  //preItemNode!=startNode:�����Լ����Լ���ǰ��
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
                if (!check)  //��ζ���޷���������
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
        //����ȫ����Ч�������ȫ��������û������
        if (CheckStateForEffect(goal.targetState, goal.targetValue))
        {
            return false;
        }
        GOAPStateType targetStateType = goal.targetState;

        SortedSet<GOAPPlanNode> nodes = GetPlanNodeByEffectStateType(targetStateType,goal.targetValue);  //�õ�����Ч����ȫ��Action���������ȼ�������õ���һ�����ȼ���ߵ�action���������Action

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

    #region ִ������
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
