using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapRoomGenerator : MapGenerationController
{
    [SerializeField]
    protected ScriptableRoomGenerationData randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPos = RunRandomWalk(randomWalkParameters, startPos);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPos);
        MapWallGenerator.CreateWalls(floorPos, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(ScriptableRoomGenerationData parameters , Vector2Int position)
    {
        var currPos = position;
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = MapGenerationAlgorithms.SimpleRandomWalk(currPos, parameters.walkLength);
            floorPos.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                currPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
            }
        }
        return floorPos;
    }
}
