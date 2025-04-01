using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判断目标是否完成的检查器
/// </summary>
public interface IGOAPGoalChecker
{
    public void Update(GOAPGoals.Goal _item, GOAPAgent agent, IGOAPOwner owner);
}
