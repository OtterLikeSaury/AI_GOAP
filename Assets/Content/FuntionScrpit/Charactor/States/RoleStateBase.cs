using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoleStateBase : FSMStateBase
{
    protected RoleController role;

    public override void Init(IStateMachineOwner owner)
    {
        role = (RoleController)owner;
    }

}
