using UnityEngine;
using UnityEngine.Events;

public class BlockDoubleDoor : MonoBehaviour
{
    bool opened = false;
    IInteractable interactable;
    [SerializeField] string itemNameNeeded;
    [SerializeField] UnityEvent onOpen;

    public bool Opened => opened;
    public string ItemNameNeeded => itemNameNeeded;

    public void Open(IInteractable _interactable)
    {
        opened = true;
        var animators = GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("Open");
        }
        if (_interactable != null) _interactable.DisplayTooltip(string.Empty);
        onOpen?.Invoke();
    }
    
    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player == null) return;
        player.ClosestDoubleDoor = this;

        if (opened) player.DisplayTooltip(string.Empty);
        else if (!opened && player.Inventory.HaveItem(itemNameNeeded) || itemNameNeeded == string.Empty)
            player.DisplayTooltip("Appuie sur [E] pour ouvrir la porte");
        else if (!opened && !player.Inventory.HaveItem(itemNameNeeded))
            player.DisplayTooltip("Tu dois tuer tous les ennemis de la zone pour pouvoir ouvrir la porte !", Color.yellow);
    }

    void OnTriggerExit(Collider other)
    {
        interactable = other.GetComponent<Player>();
        if (interactable == null) return;
        interactable.ClosestDoubleDoor = null;
        interactable.DisplayTooltip(string.Empty);
    }
}