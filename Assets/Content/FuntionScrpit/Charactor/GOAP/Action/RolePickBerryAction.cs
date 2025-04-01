using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePickBerryAction : RoleActionBase
{
    private GOAPRunState runState;
    private BerryController berry;
    private UnityObjectState ripeBerryState;
    public override void Init(GOAPAgent agent, IGOAPOwner owner)
    {
        base.Init(agent, owner);
        ripeBerryState = agent.states.GetState<UnityObjectState>("���콬��");
    }
    protected override GOAPRunState OnStart()
    {
        // 1.�ƶ������еĽ�������
        runState = GOAPRunState.Runing;

        berry = (BerryController)ripeBerryState.value;

        RoleMoveState moveState = role.stateMachine.ChangeState<RoleMoveState>();
        moveState.SetDestination(berry.transform.position);
        moveState.SetCallBack(OnMoveToBerry, 1.5f);
        return runState = GOAPRunState.Runing;
    }

    private void OnMoveToBerry()
    {
        // 2.��ժ
        ripeBerryState.value = null; // �뿪��������콬��
        RolePerformState performState = role.stateMachine.ChangeState<RolePerformState>();
        performState.PlayAnimation("Work", 0.5f, OnWorked);
    }

    private void OnWorked()
    {
        // 3.��ʼ�ؼ�
        RoleMoveState moveState = role.stateMachine.ChangeState<RoleMoveState>();
        moveState.SetDestination(MapManager.Instance.CampfirePoint.position);
        moveState.SetCallBack(OnMoveToHome, 1.5f);
        berry.OnPick();

        berry = null;
        agent.states.GetState<UnityObjectState>("���콬��").SetValue(berry);
    }

    private void OnMoveToHome()
    {
        // 4.����
        role.stateMachine.ChangeState<RoleIdleState>();
        MapManager.Instance.AddFood(1);
        runState = GOAPRunState.Successed;
    }

    public override GOAPRunState OnUpdate()
    {
        return runState;
    }

    public override void OnStop()
    {

        if (berry != null)
        {

            berry.CheckRipeState();
            Debug.Log("����������" + berry.transform.position);

            berry = null;
            agent.states.GetState<UnityObjectState>("���콬��").SetValue(berry);
        }
    }

    public override void OnDestroy()
    {
        OnStop();
    }


}
