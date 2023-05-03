using UnityEngine;

public class BlockCharacterCollision : MonoBehaviour
{
    [SerializeField] Collider CharacterCollider;
    [SerializeField] CapsuleCollider CharacterCollisionBlocker;

    void Start()
    {
        Physics.IgnoreCollision(CharacterCollider, CharacterCollisionBlocker, true);
    }
}