public abstract class RoleActionBase :GOAPActionBase
{
    protected RoleController role;

    public override void Init(GOAPAgent agent, IGOAPOwner owner)
    {
        base.Init(agent, owner);
        role = (RoleController)owner;
    }
}
