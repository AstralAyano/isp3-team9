using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class MapWallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPos, MapTilemapRenderer tilemapVisualizer)
    {
        var basicWallPos = FindWallsInDirections(floorPos, Direction2D.cardinalDirectionsLists);
        var cornerWallPos = FindWallsInDirections(floorPos, Direction2D.diagonalDirectionsLists);

        CreateBasicWall(tilemapVisualizer, basicWallPos, floorPos);
        CreateCornerWalls(tilemapVisualizer, cornerWallPos, floorPos);
    }

    private static void CreateCornerWalls(MapTilemapRenderer tilemapVisualizer, HashSet<Vector2Int> cornerWallPos, HashSet<Vector2Int> floorPos)
    {
        foreach (var position in cornerWallPos)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsLists)
            {
                var neighbourPos = position + direction;
                if (floorPos.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
    }

    private static void CreateBasicWall(MapTilemapRenderer tilemapVisualizer, HashSet<Vector2Int> basicWallPos, HashSet<Vector2Int> floorPos)
    {
        foreach (var position in basicWallPos)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionsLists)
            {
                var neighbourPos = position + direction;
                if (floorPos.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPos, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPos = new HashSet<Vector2Int>();
        foreach (var position in floorPos)
        {
            foreach (var direction in directionList)
            {
                var neighbourPos = position + direction;
                if (floorPos.Contains(neighbourPos) == false)
                {
                    wallPos.Add(neighbourPos);
                }
            }
        }
        return wallPos;
    }
}
