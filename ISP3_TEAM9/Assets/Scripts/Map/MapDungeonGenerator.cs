using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapDungeonGenerator : MapRoomGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> corridorPositions;


    private void Awake()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPos = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorPos, potentialRoomPos);

        HashSet<Vector2Int> roomPos = CreateRooms(potentialRoomPos);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPos);

        CreateRoomsAtDeadEnds(deadEnds, roomPos);

        floorPos.UnionWith(roomPos);

        for (int i = 0; i < corridors.Count; i++)
        {
            corridors[i] = IncreaseSizeByOne(corridors[i]);
            floorPos.UnionWith(corridors[i]);
        }

        tilemapVisualizer.PaintFloorTiles(floorPos);
        MapWallGenerator.CreateWalls(floorPos, tilemapVisualizer);
    }

    private List<Vector2Int> IncreaseSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int prevDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (prevDirection != Vector2Int.zero && directionFromCell != prevDirection)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                prevDirection = directionFromCell;
            }
            else
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int direction)
    {
        if (direction == Vector2.up)
        {
            return Vector2Int.right;
        }
        if (direction == Vector2.right)
        {
            return Vector2Int.down;
        }
        if (direction == Vector2.down)
        {
            return Vector2Int.left;
        }
        if (direction == Vector2.left)
        {
            return Vector2Int.up;
        }
        return Vector2Int.zero;
    }

    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPos)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPos)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsLists)
            {
                if (floorPos.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }
            if (neighboursCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPos)
    {
        HashSet<Vector2Int> roomPos = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPos.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);

            SaveRoomData(roomPosition, roomFloor);
            roomPos.UnionWith(roomFloor);
        }
        return roomPos;
    }

    private void SaveRoomData(Vector2Int roomPosition, HashSet<Vector2Int> roomFloor)
    {
        roomsDictionary[roomPosition] = roomFloor;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potentialRoomPos)
    {
        var currPos = startPos;
        potentialRoomPos.Add(currPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = MapGenerationAlgorithms.RandomWalkCorridor(currPos, corridorLength);
            corridors.Add(corridor);
            currPos = corridor[corridor.Count - 1];
            potentialRoomPos.Add(currPos);
            floorPos.UnionWith(corridor);
        }
        corridorPositions = new HashSet<Vector2Int>(floorPos);
        return corridors;
    }
}
