using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ж�Ŀ���Ƿ���ɵļ����
/// </summary>
public interface IGOAPGoalChecker
{
    public void Update(GOAPGoals.Goal _item, GOAPAgent agent, IGOAPOwner owner);
}
