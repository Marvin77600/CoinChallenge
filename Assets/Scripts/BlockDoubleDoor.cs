using UnityEngine;

public class BlockDoubleDoor : MonoBehaviour
{
    private bool opened = false;
    private IInteractable interactable;
    [SerializeField] private int minimumEnemyKillCount;

    public int MinimumEnemyKillCount => minimumEnemyKillCount;

    public bool Opened => opened;

    public void Open()
    {
        opened = true;
        var animators = GetComponentsInChildren(typeof(Animator));
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("Open");
        }
        if (interactable != null) interactable.DisplayTooltip(string.Empty);
    }
    
    void OnTriggerEnter(Collider other)
    {
        var player = (Player)other.GetComponent<IInteractable>();
        interactable = other.GetComponent<IInteractable>();
        if (interactable == null) return;
        interactable.ClosestDoubleDoor = this;

        if (!opened && player.EnemyKillCount >= minimumEnemyKillCount)
            interactable.DisplayTooltip("Appuie sur [E] pour ouvrir la porte");
        else if (!opened && player.EnemyKillCount < minimumEnemyKillCount)
            interactable.DisplayTooltip("Tu dois tuer tous les ennemis de la zone pour pouvoir ouvrir la porte !", Color.yellow);
    }

    void OnTriggerExit(Collider other)
    {
        interactable = other.GetComponent<IInteractable>();
        if (interactable == null) return;
        interactable.ClosestDoubleDoor = null;
        interactable.DisplayTooltip(string.Empty);
    }
}