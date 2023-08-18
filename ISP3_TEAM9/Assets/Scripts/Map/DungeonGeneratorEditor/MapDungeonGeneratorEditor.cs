using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseDungeonGenerator), true)]
public class MapDungeonGeneratorEditor : Editor
{
    BaseDungeonGenerator generator;

    private void Awake()
    {
        generator = (BaseDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
