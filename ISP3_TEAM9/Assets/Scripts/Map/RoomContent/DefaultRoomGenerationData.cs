using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultRoomGenerationData : BaseRoomGenerationData
{
    public GameObject lightSource;

    [SerializeField]
    private MapRoomContentPrefabPlacer prefabPlacer;

    public List<EnemyPlacementData> enemyPlacementData;
    public List<ItemPlacementData> itemData;

    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
    {
        ContentPlacementHelper itemPlacementHelper =
            new ContentPlacementHelper(roomFloor, roomFloorNoCorridors);

        List<GameObject> placedObjects =
            prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        Vector2Int lightSpawnPoint = roomCenter;

        GameObject lightObject
            = prefabPlacer.CreateObject(lightSource, lightSpawnPoint + new Vector2(0.5f, 0.5f));

        placedObjects.AddRange(prefabPlacer.PlaceEnemies(enemyPlacementData, itemPlacementHelper));

        placedObjects.Add(lightObject);

        return placedObjects;
    }
}
