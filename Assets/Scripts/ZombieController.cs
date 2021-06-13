using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    GameManager gm;
    private Animator animator;

    public UnityEngine.AI.NavMeshAgent agent;
    public GaiaController controller;
    public Transform gaia;
    public LayerMask whatIsGround, whatIsGaia;

    // Patroling
    // public Vector3 walkPoint;
    // bool walkPointSet;
    // public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool gaiaInSightRange, gaiaInAttackRange;

    // Punch Zombie
    bool punching = false;

    private void Awake()
    {
        gaia = GameObject.Find("Gaia").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        gm = GameManager.GetInstance();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gm.gameState != GameManager.GameState.GAME) 
        {
            agent.isStopped = true;
            animator.SetFloat("walk", 0);
            return;
        } 
        
        agent.isStopped = false;

        // Check for sight and attack range
        gaiaInSightRange  = Physics.CheckSphere(transform.position, sightRange, whatIsGaia);
        gaiaInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsGaia);

        if (!punching) {
            if (!gaiaInAttackRange) ChaseGaia();
            if (gaiaInAttackRange) AttackGaia();
        }

        animator.SetFloat("walk", agent.desiredVelocity.magnitude);
    }

    private void ChaseGaia()
    {   
        agent.SetDestination(gaia.position);
    }

    private void AttackGaia()
    {
        if (!Physics.CheckSphere(transform.position, 0.9f, whatIsGaia)) {
            agent.SetDestination(transform.position);
        }

        transform.LookAt(gaia);

        if (!alreadyAttacked)
        {
            // Attack code here
            animator.SetTrigger("punch");
            punching = true;
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void AE_Punch() {
        if (Physics.CheckSphere(transform.position, 0.9f, whatIsGaia)) {
            gm.gaia_lifes--;
            if (gm.gaia_lifes <= 0 && gm.gameState == GameManager.GameState.GAME)
            {       
                gm.ChangeState(GameManager.GameState.ENDGAME);
            } 
        }   
    }

    void AE_PunchFinal() {
        punching = false;
    }
}
