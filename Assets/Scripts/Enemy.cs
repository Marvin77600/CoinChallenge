using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class Enemy : Entity
{
    Animator animator;
    NavMeshAgent agent;
    BoxCollider boxCollider;
    Rigidbody rb;
    DateTime attackDateTime = new DateTime();
    [SerializeField] Zone zone;
    [SerializeField] int patrolWalkRadius;

    IEnumerator patrolCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        if (zone.Entities == null) zone.Entities = new List<Entity>();
        zone.Entities.Add(this);
    }

    void Update()
    {
        if (IsDead) return;
        if (Target != null)
        {
            animator.SetBool("IsWalk", true);
            if (CanAttackTarget(Target, out int damageValue))
            {
                transform.LookAt(Target.Position);
                if (DateTime.Compare(DateTime.UtcNow.AddSeconds(-4), attackDateTime) > 0)
                {
                    agent.isStopped = true;
                    AttackTarget(damageValue);
                    attackDateTime= DateTime.UtcNow;
                }
            }
            else
            {
                transform.LookAt(Target.Position);
                agent.isStopped = false;
                agent.SetDestination(Target.Position);
            }            
        }
        else
        {
            agent.isStopped = false;
            if (patrolCoroutine == null)
            {
                patrolCoroutine = Patrol();
                StartCoroutine(patrolCoroutine);
            }
        }
    }

    IEnumerator Patrol()
    {   
        Vector3 randomDirection = Random.insideUnitSphere * patrolWalkRadius;
        randomDirection += Position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolWalkRadius, 1))
        {
            agent.SetDestination(hit.position);
            yield return new WaitForSeconds(Random.Range(10, 20));
            agent.isStopped = true;
            patrolCoroutine = null;
        }
    }

    public override void AttackTarget(int _damageValue)
    {
        animator.SetTrigger("Attack");
    }

    public virtual void DamageTarget()
    {
        if (Target != null && CanAttackTarget(Target, out int damageValue))
        {
            Target.Damage(damageValue);
        }
    }

    public override void Damage(int _value)
    {
        base.Damage(_value);
        animator.SetTrigger("TakeDamage");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coin coin))
        {
            Destroy(coin.gameObject);
        }
        else if (other.TryGetComponent(out Player player))
        {
            Target = player;
            Target.Target = this;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (Target != null) Target.Target = null;
        Target = null;
        animator.SetBool("IsWalk", false);
        animator.SetTrigger("Idle");
    }

    public override void Death()
    {
        animator.SetBool("IsDead", true);
        rb.velocity = new Vector3(0, 0, 0);
        boxCollider.enabled = false;
        zone.NumbOfEnemyToKill--;
        if (zone.NumbOfEnemyToKill <= 0)
        {
            zone.SwitchCamera();
        }
    }
}