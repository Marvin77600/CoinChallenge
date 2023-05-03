using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text health;
    [SerializeField] private Image healthBar;
    [SerializeField] private Player player;

    public void SetHealthText()
    {
        healthBar.fillAmount = float.Parse((player.Health / 100f).ToString());
        health.SetText($"{player.Health} / 100");
        if (player.Health > 50)
        {
            healthBar.CrossFadeColor(Color.green, 2, true, true);
        }
        else if (player.Health <= 50)
        {
            healthBar.CrossFadeColor(Color.yellow, 2, true, true);
        }
        else
        {
            healthBar.CrossFadeColor(Color.red, 2, true, true);
        }
    }
}