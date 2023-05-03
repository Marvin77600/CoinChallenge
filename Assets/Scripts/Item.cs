using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private bool destroyItemOnTriggerEnter;
    private bool canElevate;
    private float baseElevation;

    public string ItemName => itemName;

    private void Start()
    {
        baseElevation = transform.localPosition.y;
    }

    private void Update()
    {
        Rotate();
        Elevate();
    }

    private void Rotate()
    {
        var rotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(rotation.x, rotation.y + 1, rotation.z);
    }

    private void Elevate()
    {
        if (canElevate)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + .01f, transform.localPosition.z);
            if (transform.localPosition.y >= baseElevation + 1)
                canElevate = false;
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - .01f, transform.localPosition.z);
            if (transform.localPosition.y <= baseElevation)
                canElevate = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Inventory.AddItem(this);
            if (destroyItemOnTriggerEnter) Destroy(gameObject);
        }
    }
}