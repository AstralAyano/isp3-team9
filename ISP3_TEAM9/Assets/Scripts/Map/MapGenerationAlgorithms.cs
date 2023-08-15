using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPos);
        var prevPos = startPos;

        for (int i = 0; i < walkLength; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomCardinalDirection();
            path.Add(newPos);
            prevPos = newPos;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLength)
    {
        List<Vector2Int> corridor  = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currPos = startPos;
        corridor.Add(currPos);

        for (int i = 0; i < corridorLength; i++)
        {
            currPos += direction;
            corridor.Add(currPos);
        }
        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsLists = new List<Vector2Int>
    {
        new Vector2Int(0, 1), //UP
        new Vector2Int(1, 0), //RIGHT
        new Vector2Int(0, -1), //DOWN
        new Vector2Int(-1, 0) //LEFT
    };

    public static List<Vector2Int> diagonalDirectionsLists = new List<Vector2Int>
    {
        new Vector2Int(1, 1), //UP-RIGHT
        new Vector2Int(1, -1), //RIGHT-DOWN
        new Vector2Int(-1, -1), //DOWN-LEFT
        new Vector2Int(-1, 1) //LEFT-UP
    };

    public static List<Vector2Int> eightDirectionsLists = new List<Vector2Int>
    {
        new Vector2Int(0, 1), //UP
        new Vector2Int(1, 1), //UP-RIGHT
        new Vector2Int(1, 0), //RIGHT
        new Vector2Int(1, -1), //RIGHT-DOWN
        new Vector2Int(0, -1), //DOWN
        new Vector2Int(-1, -1), //DOWN-LEFT
        new Vector2Int(-1, 0), //LEFT
        new Vector2Int(-1, 1) //LEFT-UP
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsLists[Random.Range(0,cardinalDirectionsLists.Count)];
    }
}