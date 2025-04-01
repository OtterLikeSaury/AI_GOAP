public class RoleHPUpGoalChecker : IGOAPGoalChecker
{
    public void Update(GOAPGoals.Goal _item, GOAPAgent agent, IGOAPOwner owner)
    {
        RoleController role = (RoleController)owner;
        float current = agent.states.GetState<FloatState>("饥饿值").value;
        float critical = RoleController.maxHP - RoleController.foodEffect;

        if (current > critical) _item.runtimePriority = 0;   //动态改变当前目标的权重
        else _item.runtimePriority = (1 - (current / critical));

    }

}
