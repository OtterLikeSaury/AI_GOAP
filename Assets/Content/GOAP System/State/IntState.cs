public class IntState : GOAPStateBase<IntState, int,IntStateCompare>
{
    public override void ApplyEffect(IntStateCompare comparer)
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

    public override bool CompareForEffect(IntStateCompare comparer)
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

    public override bool CompareForPrecondition(IntStateCompare comparer)
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

