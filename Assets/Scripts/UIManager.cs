using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text tooltip;
    [SerializeField] TMP_Text enemyKillCount;
    [SerializeField] RectTransform retryOrQuitTransform;
    [SerializeField] HealthUI healthUI;
    [SerializeField] UnityEvent onOpenEscapeMenu;
    [SerializeField] UnityEvent onCloseEscapeMenu;

    public HealthUI HealthUI => healthUI;

    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var escape = Input.GetKeyDown(KeyCode.Escape);
        if (escape)
        {
            var bRetryOrQuitTransform = retryOrQuitTransform.gameObject.activeSelf;
            Cursor.visible = !bRetryOrQuitTransform;
            Cursor.lockState = !bRetryOrQuitTransform ? CursorLockMode.None : CursorLockMode.Locked;
            DisplayRestartOrQuitMenu(!bRetryOrQuitTransform);
        }
    }

    public void SetScore(int _score)
    {
        print(score == null);
        score.SetText(_score.ToString());
    }

    public void SetKillCount(int _value) => enemyKillCount.SetText($"Nombre d'ennemis tués : {_value}");

    public void SetTooltip(string _str, Color _color = default)
    {
        if (_color != default)
        {
            tooltip.color = _color;
        }
        tooltip.color = Color.white;
        tooltip.SetText(_str);
    }

    public void SetPlayerHealthText()
    {
        healthUI.SetHealthText();
    }

    public void DisplayRestartOrQuitMenu(bool _flag)
    {
        retryOrQuitTransform.gameObject.SetActive(_flag);
        if (_flag) onOpenEscapeMenu?.Invoke();
        else onCloseEscapeMenu?.Invoke();
    }
}