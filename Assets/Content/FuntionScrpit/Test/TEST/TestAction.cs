using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestAction : GOAPActionBase
{
    public float time;
    public float timer;

    protected override GOAPRunState OnStart()
    {
        timer = 0;
        return GOAPRunState.Runing;
    }

    public override GOAPRunState OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer>time)
        {
            ApplyEffect();
            return GOAPRunState.Successed;
        }
        else
        {
            return GOAPRunState.Runing;
        }
    }
}
