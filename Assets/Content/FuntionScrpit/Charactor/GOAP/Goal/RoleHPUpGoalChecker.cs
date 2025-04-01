public class RoleHPUpGoalChecker : IGOAPGoalChecker
{
    public void Update(GOAPGoals.Goal _item, GOAPAgent agent, IGOAPOwner owner)
    {
        RoleController role = (RoleController)owner;
        float current = agent.states.GetState<FloatState>("����ֵ").value;
        float critical = RoleController.maxHP - RoleController.foodEffect;

        if (current > critical) _item.runtimePriority = 0;   //��̬�ı䵱ǰĿ���Ȩ��
        else _item.runtimePriority = (1 - (current / critical));

    }

}
