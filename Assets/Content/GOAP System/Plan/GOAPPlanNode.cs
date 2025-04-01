using System.Collections.Generic;
using UnityEngine;
public class GOAPPlanNode
{
    public GOAPActionBase action;  //自身的action
    public GOAPPlanNode parent;   //父节点
    public List<GOAPPlanNode> preconditions=new List<GOAPPlanNode>();  //前置节点，其实就是子节点
    public int indexAtParent;  //自身是父节点的第几个

    public void Destroy()  //节点回收函数
    {
        if (action == null) return;
        action = null;
        parent?.Destroy();
        parent = null;
        foreach (GOAPPlanNode item in preconditions)
        {
            item.Destroy();
        }
        preconditions.Clear();
        GOAPObjectPool.Recycle(this);
    }

    public GOAPRunState Start()
    {
        return action.StartRun();
    }

    public GOAPRunState Update()
    {
        return action.OnUpdate();
    }

    /// <summary>
    /// 节点在成功前被中止
    /// </summary>
    public void Stop()
    {
        action?.OnStop();
    }

    /// <summary>
    /// 节点执行过程中成功
    /// </summary>
    public void Succed()
    {
        action?.OnSucced();
    }
}
