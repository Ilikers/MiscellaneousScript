using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType 
{Common,Elite,Boss}
public enum StateType 
{Idle,Patrol,Attack }
public abstract class StateBase
{
    public StateType type;
    public StateBase() { }
    public abstract void OnEnter(MonsterController controller);
    public abstract void OnUpdate(MonsterController controller);
    public abstract void OnExit(MonsterController controller);
}
public class IdleState : StateBase
{
    float timer;
    public IdleState()
    {
        type = StateType.Idle;
    }

    public override void OnEnter(MonsterController controller)
    {
        timer = 0;
        controller.animator.SetLayerWeight(0, 1);
    }

    public override void OnExit(MonsterController controller)
    {
        controller.animator.SetLayerWeight(0, 0);
    }

    public override void OnUpdate(MonsterController controller)
    {
        timer += Time.deltaTime;
        if (timer>5f)
        {
            controller.machine.ChangeState(StateType.Patrol,controller);
        }
        if (controller.isGuard)
        {
            controller.machine.ChangeState(StateType.Attack,controller);
        }
    }
}
