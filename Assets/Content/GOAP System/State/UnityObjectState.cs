using UnityEngine;

public class UnityObjectState : GOAPStateBase<UnityObjectState, Object,UnityObjectStateCompare>
{
    public override void ApplyEffect(UnityObjectStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case UnityObjectSymbol.��:
                this.value = comparer.value;
                break;
            case UnityObjectSymbol.Ϊ��:
                this.value = null;
                break;

        }

    }

    public override bool CompareForPrecondition(UnityObjectStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case UnityObjectSymbol.��:
                return this.value == comparer.value;
            case UnityObjectSymbol.��:
                return this.value != comparer.value;
            case UnityObjectSymbol.Ϊ��:
                return this.value == null;
            case UnityObjectSymbol.��Ϊ��:
                return this.value != null;
        }
        return this.value = comparer.value;
    }
    public override bool CompareForEffect(UnityObjectStateCompare comparer)
    {
        switch (comparer.symbol)
        {
            case UnityObjectSymbol.��:
                return this.value == comparer.value;
            case UnityObjectSymbol.��:
                return this.value != comparer.value;
            case UnityObjectSymbol.Ϊ��:
                return this.value == null;
            case UnityObjectSymbol.��Ϊ��:
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
            case UnityObjectSymbol.��:
            case UnityObjectSymbol.��:
                return this.value == other.value;
            case UnityObjectSymbol.��Ϊ��:
                break;

        }

        return true;
    }
}

public enum UnityObjectSymbol
{
    ��,
    ��,
    ��Ϊ��,
    Ϊ��
}

