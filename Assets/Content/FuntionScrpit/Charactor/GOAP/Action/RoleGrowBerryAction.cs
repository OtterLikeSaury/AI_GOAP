using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ƶ�����ֲλ��
//�ݳ�����
//���ڳ��콬����
public class RoleGrowBerryAction : RoleActionBase
{
    private Vector2Int berryCoord;
    private GOAPRunState runState;

    protected override GOAPRunState OnStart()
    {
        berryCoord = MapManager.Instance.GetNextBuildCoord();

        Vector3 berryWorldPos=MapManager.Instance.GetCellPosition(berryCoord);
        RoleMoveState moveState = role.stateMachine.ChangeState<RoleMoveState>();

        moveState.SetDestination(berryWorldPos);
        moveState.SetCallBack(OnMoveEnd,1.5f);
        runState = GOAPRunState.Runing;
        return runState;
    }

    public override GOAPRunState OnUpdate()
    {

        return runState;
    }

    private void OnMoveEnd()
    {
        //�ݳ�����
        RolePerformState performState =role.stateMachine.ChangeState<RolePerformState>();
        performState.PlayAnimation("Work",1f,OnPerformEnd);
    }

    private void OnPerformEnd()
    {
        //���ڳ��콬����
        role.stateMachine.ChangeState<RoleIdleState>();
        BerryController berry=MapManager.Instance.SpawnBerry(berryCoord);
        if (berry.IsRipe)
        {
            agent.states.GetState<UnityObjectState>("���콬��").SetValue(berry);
        }
        runState = GOAPRunState.Successed;

    }
}
