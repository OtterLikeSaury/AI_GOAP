using UnityEngine;

public class TestHPGoalChecker : IGOAPGoalChecker
{

    public void Update(GOAPGoals.Goal _item, GOAPAgent agent, IGOAPOwner owner)
    {
        _item.runtimePriority += Time.deltaTime;
    }
}
