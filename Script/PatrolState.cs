using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : StateBase
{
    Vector3 dir;
    Quaternion rotate;
    bool isNav = false;
    int nextIndex;
    int LastIndex;
    public PatrolState()
    {
        type = StateType.Patrol;
    }

    public override void OnEnter(MonsterController controller)
    {
        isNav = false;
        while (nextIndex == LastIndex)
        {
            nextIndex = Random.Range(0, 5);
        }
        controller.meshAgent.enabled = true;
        controller.meshAgent.isStopped = false;
        dir = controller.patrolPoints[nextIndex].position - controller.transform.position;
        dir.y = 0;
        rotate = Quaternion.LookRotation(dir);
        controller.animator.SetLayerWeight(1, 1);
    }

    public override void OnExit(MonsterController controller)
    {
        controller.meshAgent.isStopped = true;
        controller.meshAgent.velocity = Vector3.zero;
        controller.rb.velocity = Vector3.zero;
        controller.rb.angularVelocity = Vector3.zero;
        controller.meshAgent.enabled = false;
        LastIndex = nextIndex;
        controller.animator.SetLayerWeight(1, 0);
    }

    public override void OnUpdate(MonsterController controller)
    {
        Patrol(controller);
        if (controller.isGuard)
        {
            controller.machine.ChangeState(StateType.Attack, controller);
        }
    }
    void Patrol(MonsterController controller)
    {
        if (!isNav)
        {
            if (Vector3.Angle(controller.transform.forward, dir) > 1f)
            {
                controller.transform.localRotation = Quaternion.Slerp(controller.transform.rotation, rotate, controller.data.rotateSpeed * Time.deltaTime);
                controller.animator.SetTrigger("Turn");
            }
            else
            {
                controller.meshAgent.SetDestination(controller.patrolPoints[nextIndex].position);
                controller.animator.SetTrigger("walk");
                isNav = true;
            }
        }
        if (Vector3.SqrMagnitude(controller.transform.position - controller.patrolPoints[nextIndex].position) < 1f)
        {
            controller.animator.SetTrigger("idle");
            controller.machine.ChangeState(StateType.Idle, controller);
        }
    }
}
