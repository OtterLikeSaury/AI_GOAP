using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapObjectBase : MonoBehaviour
{
    [ReadOnly] public Vector2Int coord;
    public virtual void Init(Vector2Int coord)
    {
        this.coord = coord;
    }
}
