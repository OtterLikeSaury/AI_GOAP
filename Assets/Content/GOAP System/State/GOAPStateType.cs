
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;

[HideLabel]
public struct GOAPStateType
{
    [HideLabel, ValueDropdown("GetAllState")]
    public string name;

    //定义与string类型的互相转化规则
    public static implicit operator GOAPStateType(string state)
    {
        return new GOAPStateType { name = state };
    }
    public static implicit operator string(GOAPStateType state)
    {
        return state.name;
    }
    //

    #region Editor

#if UNITY_EDITOR
    private List<string> GetAllState()
    {
        List<string> res = new List<string>();
        GOAPGlobal global = GOAPEditorUtility.global;

        if (global!=null&&global.GlobalStates!=null&&global.GlobalStates.stateDic!=null)
        {
            foreach (KeyValuePair<string, GOAPStateBase> item in global.GlobalStates.stateDic)
            {
                
                res.Add(item.Key);
            }
        }
        if (GOAPEditorUtility.agent!=null && GOAPEditorUtility.agent.states != null && global.GlobalStates.stateDic != null)
        {
            foreach (KeyValuePair<string, GOAPStateBase> item in GOAPEditorUtility.agent.states.stateDic)
            {
                res.Add(item.Key);
            }
        }

        return res;
    }
#endif

    #endregion

}
