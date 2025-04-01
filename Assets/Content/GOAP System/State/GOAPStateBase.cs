using System;

/// <summary>
/// 一切GOAP状态属性的最高抽象基类
/// </summary>
public abstract class GOAPStateBase
{
    /// <summary>
    /// 比较两个GOAP State在数值上是否相等
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public abstract bool EqualsValue(GOAPStateBase other);


    /// <summary>
    /// 将自己的值设为传入目标值
    /// </summary>
    /// <param name="other"></param>
    public abstract void SetValue(GOAPStateBase other);

    /// <summary>
    /// 创建一个新的基于自身值的拷贝
    /// </summary>
    /// <returns></returns>
    public abstract GOAPStateBase Copy();

    /// <summary>
    /// 获得比较器的类型
    /// </summary>
    /// <returns></returns>
    public abstract Type GetComparerType();

    /// <summary>
    /// 获取使用的比较器
    /// </summary>
    /// <returns></returns>
    public abstract GOAPStateComparer GetComparer();

    /// <summary>
    /// 比较前置条件
    /// </summary>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public abstract bool CompareForPrecondition(GOAPStateComparer comparer);

    public abstract bool CompareForEffect(GOAPStateComparer comparer);

    public abstract void ApplyEffect(GOAPStateComparer comparer);
}

/// <summary>
/// GOAP状态属性的抽象基类
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

    //产生新的复制版本，而不是引用
    public override GOAPStateBase Copy()
    {
        return new T() { value = value };
    }


    //获取比较器
    public virtual C GetStateComparer()
    {
        return new C();
    }
    public override GOAPStateComparer GetComparer()
    {
        return GetStateComparer();
    }

    //比较前置条件比较器的结果
    public abstract bool CompareForPrecondition(C comparer);
    public override bool CompareForPrecondition(GOAPStateComparer comparer)
    {
        return CompareForPrecondition((C)comparer);
    }

    //比较效果的结果
    public abstract bool CompareForEffect(C comparer);
    public override bool CompareForEffect(GOAPStateComparer comparer)
    {
        return CompareForEffect((C)comparer);
    }


    //直接应用比较器产生的结果，而忽略游戏中产生的数值变化
    public abstract void ApplyEffect(C comparer);
    public override void ApplyEffect(GOAPStateComparer comparer)
    {
        ApplyEffect((C)comparer);
    }

    /// <summary>
    /// 获取比较器类型
    /// </summary>
    /// <returns></returns>
    public override Type GetComparerType()
    {
        return typeof(C);
    }
}