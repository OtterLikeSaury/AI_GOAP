using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPActionBase
{
    [LabelText("行为名称")] public string actionName;
    [LabelText("行为描述")] public string actionDescribe;

    [LabelText("前提")] public List<GOAPTypeAndComparer> preconditions = new List<GOAPTypeAndComparer>();
    [LabelText("效果")] public List<GOAPTypeAndComparer> effects = new List<GOAPTypeAndComparer>();

    [SerializeField]
    [LabelText("代价值")] protected float costValue;
    [SerializeField]
    [LabelText("效果值")] protected float effectValue;
    [LabelText("优先级"), ReadOnly] public virtual float priority => effectValue - costValue;
    protected GOAPAgent agent;

    /// <summary>
    /// GOAP Action初始化函数
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="owner"></param>
    public virtual void Init(GOAPAgent agent,IGOAPOwner owner)
    {
        this.agent = agent;

    }

    public virtual void UpdatePriority() { }

    /// <summary>
    /// 行为前置条件是否成立
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckPrecondition()
    {
        foreach (GOAPTypeAndComparer item in preconditions)
        {
            if (!agent.CheckStateForPrecondition(item.stateType,item.stateComparer))
            {
                return false;
            }
        }
        return true;

    }

    /// <summary>
    /// 检查效果是否已经实现
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckEffect()
    {
        foreach (GOAPTypeAndComparer item in effects)
        {
            if (!agent.CheckStateForEffect(item.stateType, item.stateComparer))
            {
                return false;
            }
        }
        return true;

    }

    public GOAPRunState StartRun()
    {
        if (CheckEffect())
        {
            return GOAPRunState.Successed;
        }
        else if (CheckPrecondition())
        {                     
            return OnStart();
        }
        else
        {
            return GOAPRunState.Failed;
        }
    }

    /// <summary>
    /// GOAP Action行为进入函数
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="owner"></param>
    protected virtual GOAPRunState OnStart() { return default; }

    /// <summary>
    /// 行为Update函数
    /// </summary>
    /// <returns></returns>
    public virtual GOAPRunState OnUpdate() { return default; }

    /// <summary>
    /// 当行为完成时
    /// </summary>
    public virtual void OnSucced() { }

    /// <summary>
    /// 行为在效果达成前中断运行
    /// </summary>
    public virtual void OnStop() { }  //当被暂停运行

    /// <summary>
    /// 行为回收函数
    /// </summary>
    public virtual void OnDestroy() { }

    /// <summary>
    /// 行为完成后应用其效果
    /// </summary>
    protected void ApplyEffect()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            GOAPTypeAndComparer effect = effects[i];

            if (GOAPGlobal.instance.TryGetGlobalState(effect.stateType,out GOAPStateBase state))
            {
                state.ApplyEffect(effect.stateComparer);
            }
            else
            {
                agent.ApplyEffect(effect);    //将行为效果应用在内部状态
            }
        }
    }

}


public class GOAPTypeAndComparer
{
    [OnValueChanged("CheckState")]public GOAPStateType stateType;
    public GOAPStateComparer stateComparer;

#if UNITY_EDITOR
    public void CheckState()
    {
        if (GOAPEditorUtility.global != null && GOAPEditorUtility.global.TryGetGlobalState(stateType, out GOAPStateBase globalstate))
        {
            if (stateComparer==null) stateComparer = globalstate.GetComparer();
            else if(stateComparer!=null&& stateComparer.GetType() != globalstate.GetComparerType()) stateComparer = globalstate.GetComparer();
        }
        else if (GOAPEditorUtility.agent != null && GOAPEditorUtility.agent.states.TryGetState(stateType, out GOAPStateBase localstate))
        {
            if (stateComparer == null) stateComparer = localstate.GetComparer();
            else if (stateComparer != null && stateComparer.GetType() != localstate.GetComparerType()) stateComparer = localstate.GetComparer();

        }
    }
#endif
}

