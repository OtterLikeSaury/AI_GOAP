using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;

public class GOAPActions
{
    public List<GOAPActionBase> actions = new List<GOAPActionBase>();

    /// <summary>
    /// 各个行为结果对其状态的修改映射字典
    /// </summary>
    public Dictionary<GOAPStateType,List<GOAPActionBase>> actionEffectDic { get; private set; }  //Value:可以满足GOAPStateType的行为列表


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
    /// 将行为管理器中全部的行为效果添加到行为管理器中
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
    /// 外置按钮，检查全部行为的配置，前置条件的比较器和效果的比较器是否符合状态规范
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
