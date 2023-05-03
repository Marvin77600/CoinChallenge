using UnityEngine;

public interface IEntity
{
    bool CanAttackTarget(IEntity _entity, out int _damageValue);

    void AttackTarget(int _damageValue);

    void Damage(int _damage);

    void Death();

    int Health { get; set; }

    bool IsDead { get; }

    Vector3 Position { get; }

    IEntity Target { get; set; }
}