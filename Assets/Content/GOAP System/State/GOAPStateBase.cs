using System;

/// <summary>
/// һ��GOAP״̬���Ե���߳������
/// </summary>
public abstract class GOAPStateBase
{
    /// <summary>
    /// �Ƚ�����GOAP State����ֵ���Ƿ����
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public abstract bool EqualsValue(GOAPStateBase other);


    /// <summary>
    /// ���Լ���ֵ��Ϊ����Ŀ��ֵ
    /// </summary>
    /// <param name="other"></param>
    public abstract void SetValue(GOAPStateBase other);

    /// <summary>
    /// ����һ���µĻ�������ֵ�Ŀ���
    /// </summary>
    /// <returns></returns>
    public abstract GOAPStateBase Copy();

    /// <summary>
    /// ��ñȽ���������
    /// </summary>
    /// <returns></returns>
    public abstract Type GetComparerType();

    /// <summary>
    /// ��ȡʹ�õıȽ���
    /// </summary>
    /// <returns></returns>
    public abstract GOAPStateComparer GetComparer();

    /// <summary>
    /// �Ƚ�ǰ������
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public abstract bool CompareForPrecondition(GOAPStateComparer comparer);

    public abstract bool CompareForEffect(GOAPStateComparer comparer);

    public abstract void ApplyEffect(GOAPStateComparer comparer);
}

/// <summary>
/// GOAP״̬���Եĳ������
/// </summary>
public abstract class GOAPStateBase<T,V,C>:GOAPStateBase where T : GOAPStateBase<T, V,C>,new() where C:GOAPStateComparer,new()
{
    public V value;

    public abstract bool EqualsValue(T other);
    public override bool EqualsValue(GOAPStateBase other)
    {
        return EqualsValue((T)other);
    }

    public virtual void SetValue(T other)
    {
        this.value = other.value;
    }
    public override void SetValue(GOAPStateBase other)
    {
        SetValue((T)other);
    }

    public virtual void SetValue(V value)
    {
        this.value = value;
    }

    //�����µĸ��ư汾������������
    public override GOAPStateBase Copy()
    {
        return new T() { value = value };
    }


    //��ȡ�Ƚ���
    public virtual C GetStateComparer()
    {
        return new C();
    }
    public override GOAPStateComparer GetComparer()
    {
        return GetStateComparer();
    }

    //�Ƚ�ǰ�������Ƚ����Ľ��
    public abstract bool CompareForPrecondition(C comparer);
    public override bool CompareForPrecondition(GOAPStateComparer comparer)
    {
        return CompareForPrecondition((C)comparer);
    }

    //�Ƚ�Ч���Ľ��
    public abstract bool CompareForEffect(C comparer);
    public override bool CompareForEffect(GOAPStateComparer comparer)
    {
        return CompareForEffect((C)comparer);
    }


    //ֱ��Ӧ�ñȽ��������Ľ������������Ϸ�в�������ֵ�仯
    public abstract void ApplyEffect(C comparer);
    public override void ApplyEffect(GOAPStateComparer comparer)
    {
        ApplyEffect((C)comparer);
    }

    /// <summary>
    /// ��ȡ�Ƚ�������
    /// </summary>
    /// <returns></returns>
    public override Type GetComparerType()
    {
        return typeof(C);
    }
}