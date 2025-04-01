using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ״̬������
/// </summary>
public abstract class FSMStateBase
{
    public virtual void Init(IStateMachineOwner owner) { }

    /// <summary>
    /// ����ʼ��
    /// </summary>
    public virtual void UInit() { }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }

}
