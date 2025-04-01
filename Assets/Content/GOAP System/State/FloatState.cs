public class FloatState : GOAPStateBase<FloatState, float,FloatStateCompare>
{
    public override void ApplyEffect(FloatStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case NumberCompareSymbol.����:
                value = comparer.value;
                break;

            default:
                value += comparer.value;

                break;
        }
    }

    public override bool CompareForEffect(FloatStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case NumberCompareSymbol.����:
                return value > comparer.value;

            case NumberCompareSymbol.С��:
                return value < comparer.value;

            case NumberCompareSymbol.���ڵ���:
                return value >= comparer.value;

            case NumberCompareSymbol.С�ڵ���:
                return value <= comparer.value;

            case NumberCompareSymbol.����:
                return value == comparer.value;

            case NumberCompareSymbol.��������:
                return false;

            case NumberCompareSymbol.��������:
                return false;

        }
        return false;

    }

    public override bool CompareForPrecondition(FloatStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case NumberCompareSymbol.����:
                return value > comparer.value;

            case NumberCompareSymbol.С��:
                return value < comparer.value;

            case NumberCompareSymbol.���ڵ���:
                return value >= comparer.value;

            case NumberCompareSymbol.С�ڵ���:
                return value <= comparer.value;

            case NumberCompareSymbol.����:
                return value == comparer.value;

            case NumberCompareSymbol.��������:
                return value > 0;

            case NumberCompareSymbol.��������:
                return value < 0;

        }

        return false;
    }

    public override bool EqualsValue(FloatState other)
    {
        return this.value == other.value;
    }

}

public class FloatStateCompare : GOAPStateComparer<FloatState, FloatStateCompare>
{
    public NumberCompareSymbol symbol;
    public float value;

    public override bool EqualsComparer(FloatStateCompare other)
    {
        return symbol == other.symbol;
    }
}

/// <summary>
/// ����Ƚ�����ö��
/// </summary>
public enum NumberCompareSymbol
{
    ����,
    С��,
    ���ڵ���,
    С�ڵ���,
    ����,

    ��������,
    ��������,

}