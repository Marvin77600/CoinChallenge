using UnityEngine;

public class Entity : MonoBehaviour, IEntity
{
    [SerializeField] private int health;
    [SerializeField] private int damageMelee;

    public int Health { get { return health; } set { health = value; } }

    public bool IsDead => health <= 0;

    public int DamageMelee => damageMelee;

    public Vector3 Position => transform.position;

    public IEntity Target { get; set; }

    public virtual void AttackTarget(int _damageValue)
    {
        var target = Target as Entity;
        var positionTarget = new Vector3(target.Position.x, 0, target.Position.z);
        var position = new Vector3(Position.x, 0, Position.z);
        if (Vector3.Dot(position, positionTarget) >= .5f) Target.Damage(_damageValue);
        if (Target.Health <= 0) Target.Death();
    }

    public virtual bool CanAttackTarget(IEntity _entity, out int _damageValue)
    {
        var entity = (Entity)_entity;
        if (Vector3.Distance(entity.Position, Position) <= 4 && Vector3.Dot(Position, _entity.Position) >= .5f && !entity.IsDead)
        {
            if (entity.Health < damageMelee) _damageValue = entity.Health;
            else _damageValue = damageMelee;
            return true;
        }
        _damageValue = 0;
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