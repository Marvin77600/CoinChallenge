using UnityEngine;

public class BlockLootable : MonoBehaviour
{
    [SerializeField] int moneyValue;

    public IInteractable Interactable { get; set; }

    public bool Looted { get; set; }
    
    public int MoneyValue => moneyValue;

    public bool CanLoot => !Looted;

    public virtual void Loot()
    {
        Looted = true;
        if (Interactable != null) Interactable.DisplayTooltip(string.Empty);
    }

    void OnTriggerEnter(Collider other)
    {
        Interactable = other.GetComponent<IInteractable>();
        if (Interactable == null) return;
        Interactable.ClosestChest = this;

        if (!Looted)
            Interactable.DisplayTooltip("Appuie sur [E] pour looter le coffre");
    }

    void OnTriggerExit(Collider other)
    {
        if (Interactable == null) return;
        Interactable.DisplayTooltip(string.Empty);
        Interactable = null;
    }
}