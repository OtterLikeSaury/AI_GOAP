using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;

public class GOAPActions
{
    public List<GOAPActionBase> actions = new List<GOAPActionBase>();

    /// <summary>
    /// ������Ϊ�������״̬���޸�ӳ���ֵ�
    /// </summary>
    public Dictionary<GOAPStateType,List<GOAPActionBase>> actionEffectDic { get; private set; }  //Value:��������GOAPStateType����Ϊ�б�


    public void Init(GOAPAgent agent,IGOAPOwner owner)
    {
        actionEffectDic = new Dictionary<GOAPStateType, List<GOAPActionBase>>();

        foreach (GOAPActionBase action in actions)
        {
            action.Init(agent,owner);
            foreach (GOAPTypeAndComparer effect in action.effects)
            {
                AddActionEffect(effect.stateType,action);
            }
        }
    }

    /// <summary>
    /// ����Ϊ��������ȫ������ΪЧ����ӵ���Ϊ��������
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="action"></param>
    private void AddActionEffect(GOAPStateType stateType,GOAPActionBase action)
    {
        if (!actionEffectDic.TryGetValue(stateType,out List<GOAPActionBase> actions))
        {
            actions = new List<GOAPActionBase>();
            actionEffectDic.Add(stateType,actions);
        }
        actions.Add(action);
    }

#if UNITY_EDITOR
    /// <summary>
    /// ���ð�ť�����ȫ����Ϊ�����ã�ǰ�������ıȽ�����Ч���ıȽ����Ƿ����״̬�淶
    /// </summary>
    [Button]
    public void CheckAllActionState()
    {
        foreach (GOAPActionBase action in actions)
        {
            foreach (GOAPTypeAndComparer pre in action.preconditions)
            {
                pre.CheckState();
            }
            foreach (GOAPTypeAndComparer effect in action.effects)
            {
                
                effect.CheckState();
            }
        }
    }
#endif
}
