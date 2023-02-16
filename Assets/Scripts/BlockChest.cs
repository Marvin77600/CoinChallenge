using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChest : BlockLootable
{
    private Animator animator;

    public void Awake()
    {
        if (TryGetComponent(out Animator _animator))
        {
            animator = _animator;
        }
    }

    public override void Loot()
    {
        base.Loot();
        if (animator != null) animator.SetTrigger("Open");
    }
}