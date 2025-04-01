using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class RoleRestGoalCheck : IGOAPGoalChecker
{
    public float workImpact;

    public const float minPriority = 0.2f;
    public const float maxPriority = 0.4f;

    public void Update(GOAPGoals.Goal _item, GOAPAgent agent, IGOAPOwner owner)
    {
        int ripeBerry = GOAPGlobal.instance.GlobalStates.GetState<IntState>("成熟浆果的数量").value;

        if (ripeBerry != 0)
        {
            _item.runtimePriority = maxPriority-ripeBerry * workImpact;
            _item.runtimePriority = Mathf.Max(minPriority, _item.runtimePriority);
            return;
        }

    }
}
