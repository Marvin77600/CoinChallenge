using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockScore : MonoBehaviour
{
    Player Player;
    [SerializeField] TMP_Text text;

    void Start()
    {
        Player = FindObjectOfType<Player>();
        if (Player) text?.SetText(Player.ScoreCollection.ToString());
    }
}