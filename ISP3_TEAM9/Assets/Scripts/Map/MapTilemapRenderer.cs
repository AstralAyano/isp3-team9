using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTilemapRenderer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPos)
    {
        PaintTiles(floorPos, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> pos, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in pos)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePos, tile);
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (MapWallTypeManager.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (MapWallTypeManager.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (MapWallTypeManager.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (MapWallTypeManager.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (MapWallTypeManager.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (MapWallTypeManager.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (MapWallTypeManager.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (MapWallTypeManager.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (MapWallTypeManager.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (MapWallTypeManager.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (MapWallTypeManager.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (MapWallTypeManager.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (MapWallTypeManager.wallBottomEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }
}
