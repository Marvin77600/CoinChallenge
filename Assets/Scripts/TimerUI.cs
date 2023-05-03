using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerUI : SingletonMonoBehaviour<TimerUI>
{
    [SerializeField] float timer;
    [SerializeField] UnityEvent onTick;
    [SerializeField] UnityEvent onEnd;
    [SerializeField] TMP_Text text;

    bool active = false;
    float timeElapsed = 0;

    public float Timer => timer;
    public float TimeElapsed => timeElapsed;

    void Start()
    {
        active = true;
    }

    void Update()
    {
        if (timer <= 1)
        {
            onEnd?.Invoke();
            active = false;
        }
        if (active)
        {
            timer -= Time.deltaTime;
            timeElapsed += Time.deltaTime;
        }
        onTick?.Invoke();
    }

    public void Pause(bool _flag)
    {
        if (_flag) active = false;
        else active = true;
    }

    public string IntToTime()
    {
        if (timer == 0f)
        {
            return "";
        }

        int num = (int)Math.Floor(timer / 3600f);
        int num2 = (int)Math.Floor((timer - (num * 3600)) / 60f);
        int num3 = (int)Math.Floor(timer % 60f);
        if (num2 < 10 && num3 < 10)
            return $"0{num2}:0{num3}";
        if (num2 < 10)
            return $"0{num2}:{num3}";
        if (num3 < 10)
            return $"{num2}:0{num3}";
        return $"{num2}:{num3}";
    }

    public void UpdateText()
    {
        text?.SetText(IntToTime());
    }
}