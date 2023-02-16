using UnityEngine;

public interface IEntity
{
    bool CanAttackTarget(IEntity entity);

    void AttackTarget(IEntity entity);

    void Damage(int damage);

    void Death();

    int Health { get; set; }

    bool IsAlive => !IsDead;

    bool IsDead { get; }

    Vector3 Position { get; }

    IEntity Target { get; set; }
}