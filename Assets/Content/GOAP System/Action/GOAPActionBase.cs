using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPActionBase
{
    [LabelText("��Ϊ����")] public string actionName;
    [LabelText("��Ϊ����")] public string actionDescribe;

    [LabelText("ǰ��")] public List<GOAPTypeAndComparer> preconditions = new List<GOAPTypeAndComparer>();
    [LabelText("Ч��")] public List<GOAPTypeAndComparer> effects = new List<GOAPTypeAndComparer>();

    [SerializeField]
    [LabelText("����ֵ")] protected float costValue;
    [SerializeField]
    [LabelText("Ч��ֵ")] protected float effectValue;
    [LabelText("���ȼ�"), ReadOnly] public virtual float priority => effectValue - costValue;
    protected GOAPAgent agent;

    /// <summary>
    /// GOAP Action��ʼ������
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="owner"></param>
    public virtual void Init(GOAPAgent agent,IGOAPOwner owner)
    {
        this.agent = agent;

    }

    public virtual void UpdatePriority() { }

    /// <summary>
    /// ��Ϊǰ�������Ƿ����
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
    /// ���Ч���Ƿ��Ѿ�ʵ��
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
    /// GOAP Action��Ϊ���뺯��
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="owner"></param>
    protected virtual GOAPRunState OnStart() { return default; }

    /// <summary>
    /// ��ΪUpdate����
    /// </summary>
    /// <returns></returns>
    public virtual GOAPRunState OnUpdate() { return default; }

    /// <summary>
    /// ����Ϊ���ʱ
    /// </summary>
    public virtual void OnSucced() { }

    /// <summary>
    /// ��Ϊ��Ч�����ǰ�ж�����
    /// </summary>
    public virtual void OnStop() { }  //������ͣ����

    /// <summary>
    /// ��Ϊ���պ���
    /// </summary>
    public virtual void OnDestroy() { }

    /// <summary>
    /// ��Ϊ��ɺ�Ӧ����Ч��
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
                agent.ApplyEffect(effect);    //����ΪЧ��Ӧ�����ڲ�״̬
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

