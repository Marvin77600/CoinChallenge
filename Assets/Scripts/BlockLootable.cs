using UnityEngine;

public class BlockLootable : MonoBehaviour
{
    [SerializeField] private int moneyValue;
    private bool looted = false;
    private IInteractable interactable;

    public IInteractable Interactable { get { return interactable; } set { interactable = value; } }

    public bool Looted { get { return looted; } set { looted = value; } }
    
    public int MoneyValue => moneyValue;

    public bool CanLoot => !looted;

    public virtual void Loot()
    {
        Looted = true;
        if (interactable != null) interactable.DisplayTooltip(string.Empty);
    }

    void OnTriggerEnter(Collider other)
    {
        interactable = other.GetComponent<IInteractable>();
        if (interactable == null) return;
        interactable.ClosestChest = this;

        if (!looted)
            interactable.DisplayTooltip("Appuie sur [E] pour looter le coffre");
    }

    void OnTriggerExit(Collider other)
    {
        if (interactable == null) return;
        interactable.DisplayTooltip(string.Empty);
        interactable = null;
    }
}