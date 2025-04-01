
/// <summary>
/// 作为行为的比较判断，比如某项行为是否会提升某值
/// </summary>
public abstract class GOAPStateComparer
{
    public abstract bool EqualsComparer(GOAPStateComparer other);
}

public abstract class GOAPStateComparer<S,C>:GOAPStateComparer where S:GOAPStateBase where C:GOAPStateComparer
{
    /// <summary>
    /// 这个函数只是比较两个比较器是否相等，比如值和Symbol
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public abstract bool EqualsComparer(C other);
    public override bool EqualsComparer(GOAPStateComparer other)
    {
        return EqualsComparer((C)other);
    }
}
