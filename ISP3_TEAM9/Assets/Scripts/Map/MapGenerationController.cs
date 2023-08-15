using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerationController : MonoBehaviour
{
    [SerializeField]
    protected MapTilemapRenderer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPos = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
