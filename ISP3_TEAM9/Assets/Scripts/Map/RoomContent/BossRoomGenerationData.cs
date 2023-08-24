using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomGenerationData : BaseRoomGenerationData
{
    public GameObject lightSource;
    public GameObject player;

    public List<ItemPlacementData> itemData;

    public List<EnemyPlacementData> enemyPlacementData;

    [SerializeField]
    private MapRoomContentPrefabPlacer prefabPlacer;

    public override List<GameObject> ProcessRoom(
        Vector2Int roomCenter,
        HashSet<Vector2Int> roomFloor,
        HashSet<Vector2Int> roomFloorNoCorridors)
    {

        ContentPlacementHelper itemPlacementHelper =
            new ContentPlacementHelper(roomFloor, roomFloorNoCorridors);

        List<GameObject> placedObjects =
            prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        Vector2Int lightSpawnPoint = roomCenter;

        GameObject lightObject
            = prefabPlacer.CreateObject(lightSource, lightSpawnPoint + new Vector2(0.5f, 0.5f));

        placedObjects.Add(lightObject);

        Vector2Int playerSpawnPoint = roomCenter;

        GameObject playerObject
            = prefabPlacer.CreateObject(player, playerSpawnPoint + new Vector2(0.5f, 0.5f));

        placedObjects.AddRange(prefabPlacer.PlaceEnemies(enemyPlacementData, itemPlacementHelper));
        placedObjects.Add(playerObject);


        return placedObjects;
    }
}