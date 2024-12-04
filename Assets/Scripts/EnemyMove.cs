using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMove : MonoBehaviour
{
    public Tilemap myTilemap;
    public LoadMap loadMap;
    public MovePlayer movePlayer;
    public Vector3Int enemyPosition; 

    public void MoveTowardsPlayer()
    {
        // Get player's position as a tilemap coordinate
        Vector3Int playerPosition = Vector3Int.RoundToInt(myTilemap.WorldToCell(movePlayer.movePoint.position));

        // Calculate direction vector toward the player
        Vector3Int direction = playerPosition - enemyPosition;
        direction.Clamp(new Vector3Int(-1, -1, 0), new Vector3Int(1, 1, 0)); // Limit movement to one tile step

        // Determine the new position
        Vector3Int newPosition = enemyPosition + direction;

        // Check if the tile is walkable
        if (CanMove(newPosition))
        {
            // Update tilemap and enemy position
            myTilemap.SetTile(enemyPosition, null); // Clear old position
            enemyPosition = newPosition;
            transform.position = myTilemap.GetCellCenterWorld(newPosition); // Move enemy visually
            myTilemap.SetTile(enemyPosition, loadMap._enemy); // Set the tile for the enemy's new position
        }
    }

    bool CanMove(Vector3Int position)
    {
        TileBase tile = myTilemap.GetTile(position);
        return tile == null || tile == loadMap._none; // Walkable if null or designated walkable tile
    }
}
