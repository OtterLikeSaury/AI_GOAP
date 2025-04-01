using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolState : GOAPStateBase<BoolState, bool,BoolStateCompare>
{

    public override void ApplyEffect(BoolStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case BoolCompareSymbol.ÊÇ:
                value = true;
                break;

            case BoolCompareSymbol.·ñ:
                value = false;
                break;
        }

    }

    public override bool CompareForEffect(BoolStateCompare comparer)
    {
        return CompareForPrecondition(comparer);
    }

    public override bool CompareForPrecondition(BoolStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case BoolCompareSymbol.ÊÇ:
                return value;

            case BoolCompareSymbol.·ñ:
                return !value;
        }

        return false;
    }

    public override bool EqualsValue(BoolState other)
    {
        return this.value == other.value;
    }
}

public class BoolStateCompare : GOAPStateComparer<BoolState, BoolStateCompare>
{
    public BoolCompareSymbol symbol;

    public override bool EqualsComparer(BoolStateCompare other)
    {
        return symbol == other.symbol;
    }
}

public enum BoolCompareSymbol
{
    ÊÇ,
    ·ñ
}

