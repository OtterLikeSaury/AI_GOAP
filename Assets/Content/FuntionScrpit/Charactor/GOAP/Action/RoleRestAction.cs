public class RoleRestAction : RoleActionBase
{
    private BoolState restState;
    private GOAPRunState runState;

    public override void Init(GOAPAgent agent, IGOAPOwner owner)
    {
        base.Init(agent, owner);
        restState = agent.states.GetState<BoolState>("是否处于休息状态");
    }
    protected override GOAPRunState OnStart()
    {
        restState.SetValue(true) ;
        role.stateMachine.ChangeState<RolePerformState>().PlayAnimation("Rest",2f,OnEnd);
        runState = GOAPRunState.Runing;
        return runState;
    }

    public override GOAPRunState OnUpdate()
    {
        return runState;
    }

    private void OnEnd()
    {
        restState.SetValue(false);
        runState = GOAPRunState.Successed;
    }

    public override void OnStop()
    {
        restState.SetValue(false);
        runState = GOAPRunState.Successed;

    }
}
