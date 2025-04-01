using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoleController : MonoBehaviour,IStateMachineOwner,IGOAPOwner
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public float hpDownSpeed = 1;
    public GOAPAgent goapAgent;
    public StateMachine stateMachine;
    private FloatState hpState;

    public const float maxHP=100;
    public const float foodEffect = 25;

    private void Start()
    {
        stateMachine = new StateMachine();
        stateMachine.Init(this);
        goapAgent.Init(this);
        stateMachine.ChangeState<RoleIdleState>();
        hpState = goapAgent.states.GetState<FloatState>("¼¢¶öÖµ");
    }

    private void Update()
    {
        hpState.SetValue(hpState.value-Time.deltaTime*hpDownSpeed);
        if (hpState.value<=0)
        {
            Die();
        }
        else
        {
            goapAgent.OnUpdate();
            stateMachine.OnUpdate();
        }
    }

    private void Die()
    {
        stateMachine.Stop();
        Destroy(gameObject);
        MapManager.Instance.OnRoleDie();
    }

    public void PlayAniamtion(string animationName)
    {
        animator.CrossFadeInFixedTime(animationName,0.25f);
    }

    public void StartMove()
    {
        navMeshAgent.enabled = true;
    }

    public void StopMove()
    {
        navMeshAgent.enabled = false;
    }

    public void SetDestination(Vector3 pos)
    {
        navMeshAgent.SetDestination(pos);
    }
}

