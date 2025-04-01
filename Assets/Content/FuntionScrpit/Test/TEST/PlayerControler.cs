using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IGOAPOwner
{
    public GOAPAgent agent;
    void Start()
    {
        agent.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        agent.OnUpdate();
    }
}
