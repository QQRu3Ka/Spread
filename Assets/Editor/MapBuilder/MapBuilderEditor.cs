using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapBuilder))]
public class MapBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var builder = (MapBuilder)target;
        if(GUILayout.Button("Build Map"))
        {
            builder.BuildMap();
        }
    }
}
