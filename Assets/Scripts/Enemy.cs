using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    private Animator animator;
    private NavMeshAgent agent;
    private bool walk;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead) return;
        if (Target != null)
        {
            if (walk)
            {
                animator.SetBool("IsWalk", walk);
            }
            if (CanAttackTarget(Target))
            {
                agent.isStopped = true;
                AttackTarget(Target);
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
            agent.SetDestination(Position);
        }
    }

    public override void AttackTarget(IEntity _entity)
    {
        base.AttackTarget(_entity);
        animator.SetTrigger("Attack");
    }

    public override void Damage(int _value)
    {
        base.Damage(_value);
        animator.SetTrigger("TakeDamage");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() == null) return;
        Target = other.GetComponent<Player>();
        Target.Target = this;
        walk = true;
    }

    void OnTriggerExit(Collider other)
    {
        Target.Target = null;
        Target = null;
        walk = false;
        animator.SetBool("IsWalk", walk);
        animator.SetTrigger("Idle");
    }

    public override void Death()
    {
        animator.SetBool("IsDead", true);
        boxCollider.enabled = false;
    }
}