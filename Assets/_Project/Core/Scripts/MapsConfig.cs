using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MapsConfig", menuName = "Scriptable Objects/MapsConfig")]
public class MapsConfig : ScriptableObject
{
    public SerializedDictionary<Map, MapData> Catalog;

    [Serializable]
    public class MapData
    {
        public GameObject Map;
        public string Name;
    }
}
