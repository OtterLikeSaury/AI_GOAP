using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IStateMachineOwner owner;
    private Dictionary<Type, FSMStateBase> stateDic = new Dictionary<Type, FSMStateBase>();
    private FSMStateBase currentState;
    private bool haveState { get => currentState != null; }
    public Type currentStateType { get => currentState.GetType(); }
    
    public void Init(IStateMachineOwner owner)
    {
        this.owner = owner;
    }

    public T ChangeState<T>(bool reCurrentState=false)where T : FSMStateBase, new()
    {
        //状态一致则不需要刷新状态，不需要进行切换
        if (haveState && currentStateType == typeof(T) && !reCurrentState) return (T)currentState;

        if (currentState!=null)
        {
            currentState.OnExit();
        }

        //进入新状态
        currentState = GetState<T>();
        currentState.OnEnter();
        return (T)currentState;
    }

    //获取状态
    public FSMStateBase GetState<T>() where T : FSMStateBase, new()
    {
        Type type = typeof(T);
        if (!stateDic.TryGetValue(type,out FSMStateBase state))  //若不存在某种状态则返回一个新的状态实例化
        {
            state = new T();
            state.Init(owner);
            stateDic.Add(type,state);
        }

        return state;
    }

    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }

    public void Stop()
    {
        currentState?.OnExit();
        currentState = null;

        foreach (FSMStateBase state in stateDic.Values)
        {
            state.UInit();
        }
        stateDic.Clear();
    }
}
