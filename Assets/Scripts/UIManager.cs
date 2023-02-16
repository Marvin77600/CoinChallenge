using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;
    [SerializeField]
    private TMP_Text tooltip;
    [SerializeField]
    private Player character;
    [SerializeField]
    private TMP_Text enemyKillCount;

    public static UIManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        score.SetText($"{character.Score}");
        enemyKillCount.SetText($"Nombre d'ennemis tués : {character.EnemyKillCount}");

        if (character.ClosestChest != null && character.ClosestChest.CanLoot)
        {
            tooltip.SetText("Appuie sur [E] pour looter le coffre");
            tooltip.color = Color.white;
        }
    }

    public void SetTooltip(string str, Color color)
    {
        tooltip.SetText(str);
        tooltip.color = color;
    }

    public void SetTooltip(string str)
    {
        tooltip.SetText(str);
        tooltip.color = Color.white;
    }
}