using Sirenix.OdinInspector;
using System;
using System.Numerics;
using UnityEngine;
public class GOAPPlan
{
    public GOAPPlanNode startNode;  //�������Ŀ��Ч���Ľڵ�
    public GOAPPlanNode runingNode; //�����еĽڵ�
    public string goalName;  //Ŀ��
    [ShowInInspector,ReadOnly]public bool runing { get; private set; }
    public GOAPPlanNode stageNode => runingNode.parent;  //��ǰ���нڵ����һ���ڵ�
    public int runingNodeChildIndex => runingNode.indexAtParent;  //��ǰ���еĽڵ��Ǹ��ڵ��µĵڼ���


    public void StartRunPlan(string goalName,GOAPPlanNode targetNode)
    {
        this.goalName = goalName;
        this.startNode = targetNode;

        //TODO:�ҵ����ṹ���²�Ľڵ�
        StartNode(GetDeepestNode(startNode));   //��ʼ��ʼ������ĳ�ڵ�
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
    /// Ѱ�Ҽƻ�������������Ľڵ�
    /// </summary>
    /// <param name="startNode"></param>
    /// <returns></returns>
    private GOAPPlanNode GetDeepestNode(GOAPPlanNode startNode)
    {
        if (startNode.preconditions.Count == 0) return startNode;  //���ýڵ���������ƻ�������ײ�Ľڵ㣬��ֱ�ӷ������Լ�
        GOAPPlanNode tempNode = startNode.preconditions[0];
        return GetDeepestNode(tempNode);

    }

    public void OnUpdate()
    {
        if (runingNode!=null)
        {
            GOAPRunState nodeState = runingNode.Update();

            if (nodeState == GOAPRunState.Successed) //ִ����һ���ڵ�
            {
                //runingNode.Stop();

                //�����ɵ���startNode�������ƻ����
                if (runingNode == startNode)
                {
                    //Debug.Log("����ȫ�����");
                    SuccedRun();
                    return;
                }

                //������һ�������и��ڵ�
                if (runingNodeChildIndex < stageNode.preconditions.Count - 1)
                {
                    StartNode(stageNode.preconditions[runingNodeChildIndex + 1]);
                }
                //��������һ���ڵ㣬�����и��ڵ�
                else
                {
                    StartNode(stageNode);
                }
            }
            else if (nodeState == GOAPRunState.Failed)
            {
                StopRun();          //ִ����ͣ

            }

        }

    }

    /// <summary>
    /// ��ĳ�ڵ�֮ǰ��ȫ���ӽڵ����
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
