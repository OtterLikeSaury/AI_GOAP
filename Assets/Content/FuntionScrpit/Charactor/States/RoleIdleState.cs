public class RoleIdleState : RoleStateBase
{


    public override void OnEnter()
    {
        role.PlayAniamtion("Idle");
    }

}
