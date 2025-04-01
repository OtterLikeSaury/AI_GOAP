using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// 数值管理类，用来管理代理需要监听的全部数值类型
/// 会用在本地数据的存储，或是用作全球数据的存储
/// </summary>
public class GOAPStates
{
    public Dictionary<string, GOAPStateBase> stateDic = new Dictionary<string, GOAPStateBase>();

    public bool TryAddState(GOAPStateType _type,GOAPStateBase _state)
    {
        return stateDic.TryAdd(_type,_state);
    }

    public bool TryRemove(GOAPStateType _type)
    {
        return stateDic.Remove(_type);
    }

    public T GetState<T>(GOAPStateType _type) where T : GOAPStateBase
    {
        return (T)stateDic[_type];
    }

    public bool TryGetState(GOAPStateType type,out GOAPStateBase state)
    {

        state = default;
        if (stateDic == null||type.name==null) return false;
        return stateDic.TryGetValue(type,out state);
    }
    public bool TryGetState<T>(GOAPStateType _type,out T _state) where T : GOAPStateBase
    {
        _state = default;
        if (stateDic == null || _type.name == null) return false;

        if (stateDic.TryGetValue(_type,out GOAPStateBase _tempState))
        {
             _state = (T)_tempState;
            return true;
        }
        else return false;

    }

    /// <summary>
    /// 某前置条件是否已经达到某比较器中的描述条件
    /// </summary>
    /// <param name="_type">状态类型</param>
    /// <param name="_comparer">状态比较器</param>
    /// <returns></returns>
    public bool CheckStateForPrecondition(GOAPStateType _type,GOAPStateComparer _comparer)
    {
        if (TryGetState(_type,out GOAPStateBase _state))
        {
            return _state.CompareForPrecondition(_comparer);
        }

        return false;
    }

    /// <summary>
    /// 某效果是否已经实现
    /// </summary>
    /// <param name="_type">状态类型</param>
    /// <param name="_comparer">状态比较器</param>
    /// <returns></returns>
    public bool CheckStateForEffect(GOAPStateType _type, GOAPStateComparer _comparer)
    {
        if (TryGetState(_type, out GOAPStateBase _state))
        {
            return _state.CompareForEffect(_comparer);
        }

        return false;
    }

    public void ApplyEffect(GOAPTypeAndComparer effect)
    {
        if (stateDic.TryGetValue(effect.stateType,out GOAPStateBase value))
        {
            value.ApplyEffect(effect.stateComparer);
        }
    }


#if UNITY_EDITOR


#endif

}
