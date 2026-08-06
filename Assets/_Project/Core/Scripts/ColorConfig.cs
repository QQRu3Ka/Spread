using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ColorConfig", menuName = "Scriptable Objects/ColorConfig")]
public class ColorConfig : ScriptableObject
{
    [FormerlySerializedAs("ColorDict")] public SerializedDictionary<GameColor, ColorData> ColorDictionary;

    [Serializable]
    public class ColorData
    {
        public Color Color;
        public Material BlockMaterial;
        public Material CellMaterial;
    }
}
