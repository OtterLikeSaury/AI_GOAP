using UnityEngine;

public class UnityObjectState : GOAPStateBase<UnityObjectState, Object,UnityObjectStateCompare>
{
    public override void ApplyEffect(UnityObjectStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case UnityObjectSymbol.是:
                this.value = comparer.value;
                break;
            case UnityObjectSymbol.为空:
                this.value = null;
                break;

        }

    }

    public override bool CompareForPrecondition(UnityObjectStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case UnityObjectSymbol.是:
                return this.value == comparer.value;
            case UnityObjectSymbol.否:
                return this.value != comparer.value;
            case UnityObjectSymbol.为空:
                return this.value == null;
            case UnityObjectSymbol.不为空:
                return this.value != null;
        }
        return this.value = comparer.value;
    }
    public override bool CompareForEffect(UnityObjectStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case UnityObjectSymbol.是:
                return this.value == comparer.value;
            case UnityObjectSymbol.否:
                return this.value != comparer.value;
            case UnityObjectSymbol.为空:
                return this.value == null;
            case UnityObjectSymbol.不为空:
                return this.value != null;
        }
        return this.value = comparer.value;
    }


    public override bool EqualsValue(UnityObjectState other)
    {
        return this.value == other.value;
    }
}

public class UnityObjectStateCompare : GOAPStateComparer<UnityObjectState, UnityObjectStateCompare>
{
    public UnityObjectSymbol symbol;
    public Object value;

    public override bool EqualsComparer(UnityObjectStateCompare other)
    {
        if (other.symbol != symbol) return false;

        switch (other.symbol)
        {
            case UnityObjectSymbol.是:
            case UnityObjectSymbol.否:
                return this.value == other.value;
            case UnityObjectSymbol.不为空:
                break;

        }

        return true;
    }
}

public enum UnityObjectSymbol
{
    是,
    否,
    不为空,
    为空
}

