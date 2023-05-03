using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utils
{
    public static void LogColor(ColorEnum _color, object _object)
    {
        Debug.Log($"<color={_color.ToString().ToLower()}>{_object}</color>");
    }

    public enum ColorEnum
    {
        Blue,
        Red,
        Yellow,
        Green,
        Magenta,
        Black
    } 
}