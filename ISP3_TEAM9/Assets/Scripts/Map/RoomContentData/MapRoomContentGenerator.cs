using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MapRoomContentGenerator : MonoBehaviour
{
    [SerializeField]
    private BaseRoomGenerationData playerRoom, EndPointRoom, defaultRoom;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField]
    private GraphTest graphTest;


    public Transform itemParent;

    [SerializeField]
    private CinemachineVirtualCamera cinemachineCamera;

    public UnityEvent RegenerateDungeon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var item in spawnedObjects)
            {
                Destroy(item);
            }
            RegenerateDungeon?.Invoke();
        }
    }
    public void GenerateRoomContent(MapDungeonData dungeonData)
    {
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        SelectPlayerSpawnPoint(dungeonData);
        SelectEndPoint(dungeonData);
        SelectEnemySpawnPoints(dungeonData);

        foreach (GameObject item in spawnedObjects)
        {
            if(item != null)
                item.transform.SetParent(itemParent, false);
        }
        AstarPath.active.UpdateGraphs(new Bounds(new Vector3(0, 0, 0), new Vector3(500, 500, 0)), 0);
    }

    private void SelectPlayerSpawnPoint(MapDungeonData dungeonData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
        Vector2Int playerSpawnPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        graphTest.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        Vector2Int roomIndex = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            dungeonData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(roomIndex)
            );

        FocusCameraOnThePlayer(placedPrefabs[placedPrefabs.Count - 1].transform);

        spawnedObjects.AddRange(placedPrefabs);

        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }

    private void FocusCameraOnThePlayer(Transform playerTransform)
    {
        cinemachineCamera.LookAt = playerTransform;
        cinemachineCamera.Follow = playerTransform;
    }

    private void SelectEndPoint(MapDungeonData dungeonData)
    {
        while (true)
        {
            int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
            Vector2Int endPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);
            Vector2Int roomIndex = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);
            foreach (Vector2Int position in dungeonData.roomsDictionary[roomIndex])
            {
                if (graphTest.dijkstraResult[position] == graphTest.highestValue)
                {
                    List<GameObject> placedPrefabs = EndPointRoom.ProcessRoom(
                        endPoint,
                        dungeonData.roomsDictionary.Values.ElementAt(randomRoomIndex),
                        dungeonData.GetRoomFloorWithoutCorridors(roomIndex)
                        );

                    spawnedObjects.AddRange(placedPrefabs);
                    dungeonData.roomsDictionary.Remove(endPoint);
                    return;
                }
            }
        }
    }

    private void SelectEnemySpawnPoints(MapDungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int,HashSet<Vector2Int>> roomData in dungeonData.roomsDictionary)
        { 
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    roomData.Value, 
                    dungeonData.GetRoomFloorWithoutCorridors(roomData.Key)
                    )
            );

        }
    }

}
