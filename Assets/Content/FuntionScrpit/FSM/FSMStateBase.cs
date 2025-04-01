using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态机基类
/// </summary>
public abstract class FSMStateBase
{
    public virtual void Init(IStateMachineOwner owner) { }

    /// <summary>
    /// 反初始化
    /// </summary>
    public virtual void UInit() { }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

}
