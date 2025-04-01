public class FloatState : GOAPStateBase<FloatState, float,FloatStateCompare>
{
    public override void ApplyEffect(FloatStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case NumberCompareSymbol.等于:
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
            case NumberCompareSymbol.大于:
                return value > comparer.value;

            case NumberCompareSymbol.小于:
                return value < comparer.value;

            case NumberCompareSymbol.大于等于:
                return value >= comparer.value;

            case NumberCompareSymbol.小于等于:
                return value <= comparer.value;

            case NumberCompareSymbol.等于:
                return value == comparer.value;

            case NumberCompareSymbol.提升倾向:
                return false;

            case NumberCompareSymbol.降低倾向:
                return false;

        }
        return false;

    }

    public override bool CompareForPrecondition(FloatStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case NumberCompareSymbol.大于:
                return value > comparer.value;

            case NumberCompareSymbol.小于:
                return value < comparer.value;

            case NumberCompareSymbol.大于等于:
                return value >= comparer.value;

            case NumberCompareSymbol.小于等于:
                return value <= comparer.value;

            case NumberCompareSymbol.等于:
                return value == comparer.value;

            case NumberCompareSymbol.提升倾向:
                return value > 0;

            case NumberCompareSymbol.降低倾向:
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
/// 数理比较倾向枚举
/// </summary>
public enum NumberCompareSymbol
{
    大于,
    小于,
    大于等于,
    小于等于,
    等于,

    提升倾向,
    降低倾向,

}