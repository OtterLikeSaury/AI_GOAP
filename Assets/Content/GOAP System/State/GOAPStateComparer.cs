
/// <summary>
/// ��Ϊ��Ϊ�ıȽ��жϣ�����ĳ����Ϊ�Ƿ������ĳֵ
/// </summary>
public abstract class GOAPStateComparer
{
    public abstract bool EqualsComparer(GOAPStateComparer other);
}

public abstract class GOAPStateComparer<S,C>:GOAPStateComparer where S:GOAPStateBase where C:GOAPStateComparer
{
    /// <summary>
    /// �������ֻ�ǱȽ������Ƚ����Ƿ���ȣ�����ֵ��Symbol
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public abstract bool EqualsComparer(C other);
    public override bool EqualsComparer(GOAPStateComparer other)
    {
        return EqualsComparer((C)other);
    }
}
