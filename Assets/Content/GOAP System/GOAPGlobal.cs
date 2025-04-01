using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局属性声明黑板
/// </summary>
public class GOAPGlobal : SerializedMonoBehaviour
{
    //单例模式
    public static GOAPGlobal instance { get; private set; }
    private void Awake()
    {

        instance = this;
    }
    //

    public bool TryGetGlobalState(string targetState, out GOAPStateBase state)
    {
        state = default;
        if (globalStates == null || globalStates.stateDic == null) return false;
        return globalStates.TryGetState(targetState, out state);
    }

    [SerializeField] private GOAPStates globalStates;
    public GOAPStates GlobalStates => globalStates;

}
