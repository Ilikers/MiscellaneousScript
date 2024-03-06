using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackState : StateBase
{
    private enum AttackBehaviour
    {Detect,Follow,Attack,Retreat }
    private AttackBehaviour m_Behaviour;
    private float timer = 0;
    private int weight = 1;
    private int attackWeight = 10;
    public AttackState()
    {
        type = StateType.Attack;
    }

    public override void OnEnter(MonsterController controller)
    {
        m_Behaviour = AttackBehaviour.Detect;
        weight = 1;
        attackWeight = 10;
        timer = 0;
    }

    public override void OnExit(MonsterController controller)
    {
        m_Behaviour = AttackBehaviour.Detect;
    }

    public override void OnUpdate(MonsterController controller)
    {
        if (!controller.isGuard)
        {
            controller.machine.ChangeState(StateType.Idle, controller);
            return;
        }
        switch (m_Behaviour)
        {
            case AttackBehaviour.Detect:
                OnDetect(controller);
                break;
            case AttackBehaviour.Follow:
                OnFollow(controller);
                break;
            case AttackBehaviour.Retreat:
                OnRetreat(controller);
                break;
            case AttackBehaviour.Attack:
                OnAttack(controller);
                break;
            default:
                break;
        }

    }
    void OnDetect(MonsterController controller) 
    {
        Rotate(controller);
        if (Vector3.SqrMagnitude(controller.transform.position-controller.target.transform.position)<56.25f)
        {
            m_Behaviour = AttackBehaviour.Follow;
            return;
        }
    }
    void OnFollow(MonsterController controller)
    {
        Rotate(controller);
        if (Vector3.SqrMagnitude(controller.transform.position - controller.target.transform.position) > 25f)
        {
            controller.transform.Translate(Vector3.forward * controller.data.moveSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > 3f)
            {
                weight = Random.Range(0, 2) * 2 - 1;
                attackWeight = Random.Range(0, 10);
                timer = 0;
            }
            controller.transform.RotateAround(controller.target.transform.position,Vector3.up,controller.data.moveSpeed*Time.deltaTime*weight);
            if (attackWeight<controller.data.attackWeight)
            {
                m_Behaviour = AttackBehaviour.Attack;
                attackWeight = 10; 
                return;
            }
        }
    }
    void OnAttack(MonsterController controller) 
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            m_Behaviour = AttackBehaviour.Retreat;
        }
    }
    void OnRetreat(MonsterController controller) 
    {
        Rotate(controller);
        if (Vector3.SqrMagnitude(controller.transform.position - controller.target.transform.position) < 25f)
        {
            controller.transform.Translate(Vector3.back * controller.data.moveSpeed * Time.deltaTime, Space.Self);
        }
        else
        {
            m_Behaviour = AttackBehaviour.Follow;
        }
    }
    void Rotate(MonsterController controller) 
    {
        Vector3 dir = controller.target.transform.position - controller.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        controller.transform.localRotation = Quaternion.Slerp(controller.transform.rotation, rot, controller.data.rotateSpeed * Time.deltaTime);
    }
}