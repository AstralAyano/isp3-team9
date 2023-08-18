using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapRenderer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
        AstarPath.active.UpdateGraphs(new Bounds(new Vector3(0, 0, 0), new Vector3(500, 500, 0)), 0);
    }

    protected abstract void RunProceduralGeneration();
}
