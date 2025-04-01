public class IntState : GOAPStateBase<IntState, int,IntStateCompare>
{
    public override void ApplyEffect(IntStateCompare comparer)
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

    public override bool CompareForEffect(IntStateCompare comparer)
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

    public override bool CompareForPrecondition(IntStateCompare comparer)
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

    public override bool EqualsValue(IntState other)
    {
        return this.value == other.value;
    }

}

public class IntStateCompare : GOAPStateComparer<IntState, IntStateCompare>
{
    public NumberCompareSymbol symbol;
    public int value;

    public override bool EqualsComparer(IntStateCompare other)
    {
        return symbol == other.symbol;
    }
}

