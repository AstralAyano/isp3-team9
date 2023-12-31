using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRoomGenerationData : BaseRoomGenerationData
{
    public GameObject lightSource;
    public GameObject player;

    public List<ItemPlacementData> itemData;

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
 
        placedObjects.Add(playerObject);

        return placedObjects;
    }
}

public abstract class PlacementData
{
    [Min(0)]
    public int minQuantity = 0;
    [Min(0)]
    [Tooltip("Max is inclusive")]
    public int maxQuantity = 0;
    public int Quantity
        => UnityEngine.Random.Range(minQuantity, maxQuantity + 1);
}

[Serializable]
public class ItemPlacementData : PlacementData
{
    public MapContentData itemData;
}

[Serializable]
public class EnemyPlacementData : PlacementData
{
    public GameObject enemyPrefab;
    public Vector2Int enemySize = Vector2Int.one;
}

[Serializable]
public class ContentPlacementData : PlacementData
{
    public GameObject contentPrefab;
    public Vector2Int contentSize = Vector2Int.one;
}