using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public StateBase currentState;
    Dictionary<StateType,StateBase> allStates = new Dictionary<StateType,StateBase>();
    public StateMachine(MonsterController controller) 
    {
        Init(controller);
    }
    void Init(MonsterController controller) 
    {
        StateBase idle = new IdleState();
        allStates.Add(idle.type, idle);
        StateBase patrol = new PatrolState();
        allStates.Add(patrol.type,patrol);
        StateBase attack = new AttackState();
        allStates.Add(attack.type, attack);
        ChangeState(StateType.Idle, controller);
    }
    public void ChangeState(StateType stateType,MonsterController controller) 
    {
        currentState?.OnExit(controller);
        currentState = allStates[stateType];
        currentState?.OnEnter(controller);
    }
    public void Onupdate(MonsterController controller) 
    {
        currentState?.OnUpdate(controller);
    }
}
