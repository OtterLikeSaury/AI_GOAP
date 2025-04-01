using Sirenix.OdinInspector;
using System;
using System.Numerics;
using UnityEngine;
public class GOAPPlan
{
    public GOAPPlanNode startNode;  //最终完成目标效果的节点
    public GOAPPlanNode runingNode; //运行中的节点
    public string goalName;  //目标
    [ShowInInspector,ReadOnly]public bool runing { get; private set; }
    public GOAPPlanNode stageNode => runingNode.parent;  //当前运行节点的上一级节点
    public int runingNodeChildIndex => runingNode.indexAtParent;  //当前运行的节点是父节点下的第几个


    public void StartRunPlan(string goalName,GOAPPlanNode targetNode)
    {
        this.goalName = goalName;
        this.startNode = targetNode;

        //TODO:找到树结构最下层的节点
        StartNode(GetDeepestNode(startNode));   //开始初始化运行某节点
    }

    public void StopRun()
    {
        runing = false;
        runingNode?.Stop();
        RecycleNodes(startNode);
        runingNode = null;
        startNode = null; 
        goalName = null;
    }

    private void SuccedRun()
    {
        runing = false;
        runingNode?.Succed();
        runingNode = null;

        RecycleNodes(startNode);
        startNode = null;

        goalName = null;
    }

    /// <summary>
    /// 寻找计划树中左下最深的节点
    /// </summary>
    /// <param name="startNode"></param>
    /// <returns></returns>
    private GOAPPlanNode GetDeepestNode(GOAPPlanNode startNode)
    {
        if (startNode.preconditions.Count == 0) return startNode;  //若该节点就是这条计划线上最底层的节点，则直接返回他自己
        GOAPPlanNode tempNode = startNode.preconditions[0];
        return GetDeepestNode(tempNode);

    }

    public void OnUpdate()
    {
        if (runingNode!=null)
        {
            GOAPRunState nodeState = runingNode.Update();

            if (nodeState == GOAPRunState.Successed) //执行下一个节点
            {
                //runingNode.Stop();

                //如果完成的是startNode则整个计划完成
                if (runingNode == startNode)
                {
                    //Debug.Log("任务全部完成");
                    SuccedRun();
                    return;
                }

                //存在下一个则运行父节点
                if (runingNodeChildIndex < stageNode.preconditions.Count - 1)
                {
                    StartNode(stageNode.preconditions[runingNodeChildIndex + 1]);
                }
                //不存在下一个节点，则运行父节点
                else
                {
                    StartNode(stageNode);
                }
            }
            else if (nodeState == GOAPRunState.Failed)
            {
                StopRun();          //执行暂停

            }

        }

    }

    /// <summary>
    /// 将某节点之前的全部子节点回收
    /// </summary>
    /// <param name="node"></param>
    private void RecycleNodes(GOAPPlanNode node)
    {
        if (node!=null)
        {
            foreach (GOAPPlanNode item in node.preconditions)
            {
                RecycleNodes(item);
            }

            node.action = null;
            node.parent = null;
            node.indexAtParent = 0;
            node.preconditions.Clear();
        }
    }

    private void StartNode(GOAPPlanNode node)
    {
        runingNode = node;
        GOAPRunState nodeState = runingNode.Start();

        switch (nodeState)
        {
            case GOAPRunState.Runing:
                runing = true;
                break;
            case GOAPRunState.Successed:
                SuccedRun();
                break;
            case GOAPRunState.Failed:
                StopRun();
                break;
        }
    }

    public void OnDestroy()
    {
        if (runingNode!=null)
        {
            runingNode.action?.OnDestroy();
        }
        if (startNode!=null)
        {
            RecycleNodes(startNode);
        }
    }

}
