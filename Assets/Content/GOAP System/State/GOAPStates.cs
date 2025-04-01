using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// ��ֵ�����࣬�������������Ҫ������ȫ����ֵ����
/// �����ڱ������ݵĴ洢����������ȫ�����ݵĴ洢
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
    /// ĳǰ�������Ƿ��Ѿ��ﵽĳ�Ƚ����е���������
    /// </summary>
    /// <param name="_type">״̬����</param>
    /// <param name="_comparer">״̬�Ƚ���</param>
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
    /// ĳЧ���Ƿ��Ѿ�ʵ��
    /// </summary>
    /// <param name="_type">״̬����</param>
    /// <param name="_comparer">״̬�Ƚ���</param>
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
