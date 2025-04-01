using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePerformState : RoleStateBase
{
    private string animationName;
    private Action onEnd;
    private float endTime;
    private float endTimer;

    /// <summary>
    /// 演出组件状态播放某个动画
    /// </summary>
    /// <param name="animationName"></param>
    /// <param name="endTime"></param>
    /// <param name="onEndAction"></param>
    public void PlayAnimation(string animationName,float endTime,Action onEndAction)
    {
        this.animationName = animationName;
        this.onEnd = onEndAction;
        this.endTime = endTime;
        role.PlayAniamtion(animationName);
        endTimer = 0;
    }

    public override void OnUpdate()
    {
        endTimer += Time.deltaTime;
        if (endTimer>endTime)
        {
            onEnd?.Invoke();
            onEnd = null;
        }
    }

    public override void OnExit()
    {
        onEnd = null;
    }
}
