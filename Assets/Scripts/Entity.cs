using UnityEngine;

public class Entity : MonoBehaviour, IEntity
{
    [SerializeField] private int health;
    [SerializeField] private int damageMelee;

    public int Health { get { return health; } set { health = value; } }

    public bool IsAlive => !IsDead;

    public bool IsDead => health <= 0;

    public int DamageMelee => damageMelee;

    public Vector3 Position => transform.position;

    public IEntity Target { get; set; }

    public virtual void AttackTarget(IEntity _entity)
    {
        _entity.Damage(damageMelee);
        if (_entity.Health <= 0) _entity.Death();
    }

    public virtual bool CanAttackTarget(IEntity _entity)
    {
        var entity = (Entity)_entity;
        if (Vector3.Distance(entity.Position, Position) <= 4)
        {
            return true;
        }
        return false;
    }

    public virtual void Damage(int _value)
    {
        health -= _value;
    }

    public virtual void Death()
    { 
    }
}