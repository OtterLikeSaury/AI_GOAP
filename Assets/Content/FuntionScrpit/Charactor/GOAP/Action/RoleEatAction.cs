using UnityEngine;

public class RoleEatAction : RoleActionBase
{
    private GOAPRunState runState;

    protected override GOAPRunState OnStart()
    {
        //��ȥ����
        RoleMoveState moveState=role.stateMachine.ChangeState<RoleMoveState>();
        moveState.SetDestination(MapManager.Instance.CampfirePoint.position);
        moveState.SetCallBack(OnMoveEnd,2f);
        runState = GOAPRunState.Runing;
        return runState;
    }


    private void OnMoveEnd() //�������𴦿�ʼ��
    {
        if (GOAPGlobal.instance.GlobalStates.GetState<IntState>("����ʳ�������").value<=0)
        {
            runState = GOAPRunState.Failed;
            return;
        }
        //�������𴦿�ʼ��
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
