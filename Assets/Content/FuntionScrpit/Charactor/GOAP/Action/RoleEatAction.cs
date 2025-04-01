using UnityEngine;

public class RoleEatAction : RoleActionBase
{
    private GOAPRunState runState;

    protected override GOAPRunState OnStart()
    {
        //先去篝火
        RoleMoveState moveState=role.stateMachine.ChangeState<RoleMoveState>();
        moveState.SetDestination(MapManager.Instance.CampfirePoint.position);
        moveState.SetCallBack(OnMoveEnd,2f);
        runState = GOAPRunState.Runing;
        return runState;
    }


    private void OnMoveEnd() //到达篝火处开始吃
    {
        if (GOAPGlobal.instance.GlobalStates.GetState<IntState>("储备食物的数量").value<=0)
        {
            runState = GOAPRunState.Failed;
            return;
        }
        //到达篝火处开始吃
        RolePerformState performState =role.stateMachine.ChangeState<RolePerformState>();
        performState.PlayAnimation("Eat",1f,OnEatEnd);
    }

    private void OnEatEnd()
    {
        MapManager.Instance.OnRoleEat();
        role.stateMachine.ChangeState<RoleIdleState>();
        ApplyEffect();
        runState = GOAPRunState.Successed;
    }

    public override GOAPRunState OnUpdate()
    {
        return runState;
    }
}
