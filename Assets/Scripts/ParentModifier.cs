using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParentModifier : MonoBehaviour
{
    [SerializeField] Transform parentEnter;
    [SerializeField] Transform parentExit;
    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onExit;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.transform.parent = parentEnter;
            var playerPos = player.Position;
            playerPos.y = 1;
            onEnter?.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.transform.parent = parentExit;
            onExit?.Invoke();
        }
    }
}