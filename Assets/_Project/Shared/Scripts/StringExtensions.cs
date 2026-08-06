using Unity.VisualScripting;
using UnityEngine;

public static class StringExtensions
{
    public static string ColorWith(this string str, Color color)
    {
        return $"<color=#{color.ToHexString()}>{str}</color>";
    }
}
