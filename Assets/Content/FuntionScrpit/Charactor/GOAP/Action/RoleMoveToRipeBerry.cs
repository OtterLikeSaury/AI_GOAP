using UnityEngine;

/// <summary>
/// 角色移动到成熟的浆果处Action
/// </summary>
public class RoleMoveToRipeBerry : RoleActionBase
{
    private GOAPRunState runState;
    private BerryController berry;
    private UnityObjectState ripeBerryState;
    private float oriCostValue;

    public override void Init(GOAPAgent agent, IGOAPOwner owner)
    {
        base.Init(agent, owner);
        //oriCostValue = costValue;
        ripeBerryState = agent.states.GetState<UnityObjectState>("成熟浆果");
    }

    protected override GOAPRunState OnStart()
    {
        //costValue = oriCostValue;
        berry = (BerryController)ripeBerryState.value;   //访问内部属性，是否有自己刚刚种植的浆果丛

        if (berry == null) berry = MapManager.Instance.RoleTryGetRipeBerry();


        if (berry == null)
        {
            //costValue = 2;
            return runState = GOAPRunState.Failed;
        }

        // 前往浆果
        RoleMoveState moveState = role.stateMachine.ChangeState<RoleMoveState>();
        moveState.SetDestination(berry.transform.position);
        moveState.SetCallBack(OnMoveEnd, 1.5f);
        return runState = GOAPRunState.Runing;
    }

    private void OnMoveEnd()
    {
        if (berry.IsRipe)
        {
            agent.states.GetState<UnityObjectState>("成熟浆果").SetValue(berry);
            //berry = null;
        }

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
            Debug.Log("返还浆果丛"+berry.transform.position);

            berry = null;
            agent.states.GetState<UnityObjectState>("成熟浆果").SetValue(berry);
        }
    }  

    public override void OnDestroy()
    {
        OnStop();
    }
}
