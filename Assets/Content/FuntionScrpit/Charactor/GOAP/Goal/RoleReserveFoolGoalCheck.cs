using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʳ����˿ڵı�����ϵ���
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
