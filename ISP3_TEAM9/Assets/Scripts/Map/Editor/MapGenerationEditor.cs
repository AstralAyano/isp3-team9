using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerationController), true)]
public class MapGenerationEditor : Editor
{
    MapGenerationController generator;

    private void Awake()
    {
        generator = (MapGenerationController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
