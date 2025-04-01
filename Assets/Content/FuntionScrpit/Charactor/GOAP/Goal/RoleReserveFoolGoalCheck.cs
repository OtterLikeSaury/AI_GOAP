using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 食物和人口的比例关系检测
/// </summary>
public class RoleReserveFoolGoalCheck : IGOAPGoalChecker
{
    public float maxPriority = 3;
    public void Update(GOAPGoals.Goal _item, GOAPAgent agent, IGOAPOwner owner)
    {
        int foodCount = MapManager.Instance.ReserveFoodCount;
        if (foodCount==0)
        {
            _item.runtimePriority = maxPriority;
            return;
        }
        float piority = MapManager.Instance.RoleCount / (float)foodCount;
        _item.runtimePriority = Mathf.Clamp(piority,0,maxPriority);
    }
}
