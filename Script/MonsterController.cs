using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Animator     animator;
    public Rigidbody    rb;

    public GameObject   target;
    public MonsterData data;
    public Transform[] patrolPoints;
    public StateMachine machine;
    public bool isGuard = false;
    private void Awake()
    {
        machine = new StateMachine(this);
        meshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        //≤‚ ‘¥˙¬Î
        data.id = 0;
        data.name = gameObject.name;
        data.hp = 100;
        data.attack = 10;
        data.rotateSpeed = 3;
        data.moveSpeed = 3;
        data.attackWeight = 3;
        //≤‚ ‘¥˙¬ÎΩ· ¯
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        machine?.Onupdate(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGuard = true;
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGuard = false;
            target = null;
        }
    }
}
