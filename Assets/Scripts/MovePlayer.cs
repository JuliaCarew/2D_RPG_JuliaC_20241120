using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovePlayer : MonoBehaviour
{
    public LoadMap loadMap;
    //public Combat combat;
    public Tilemap myTilemap;
    public Transform movePoint;
    public GameObject playerSpawnPoint;
    public float tileSize = 0.08f;
    // need to snap player's position to tile, maybe by dividing position by tileSize !!

    // updating the player sprite
    public TileBase playerTile;

    void Start()
    {
        movePoint.parent = null;  // allows movepoint to dictate player's direction/ can be moved on it's own    
        ResetPosition();
    }
    void Update()
    {
        MovePosition();
    }
    public bool CanMove(int x, int y)
    {
        // setting new variable to determine current position
        Vector3Int cellPosition = new Vector3Int(x, y, 0);      
        // Get the tile at the specified grid position
        TileBase tileAtPosition = myTilemap.GetTile(cellPosition);

        Debug.Log($"Checking tile at ({x}, {y}): {tileAtPosition}");

        // Allow movement if the tile is null (empty) or is explicitly _none
        if (tileAtPosition == null || tileAtPosition == loadMap._none)
        {
            Debug.Log("CanWalk");
            return true; 
        }

        // cannot move on wall, chest, door, or enemy tiles
        if (tileAtPosition == loadMap._wall ||
            tileAtPosition == loadMap._door ||
            tileAtPosition == loadMap._chest)
        {
            Debug.Log($"Blocked tile at ({x}, {y}): {tileAtPosition}");
            return false;
        }
        if (tileAtPosition == loadMap._enemy)
        {
            // combat.PlayerAttack(); // get reference to combat script method for player attack
        }
        if (tileAtPosition == loadMap._winTile)
        {
            LevelComplete();
            return false;
        }
        return false;
    }

    // set player to start position whenever completing a level
    void ResetPosition()
    {
        Vector3 spawnPosition = playerSpawnPoint.transform.position;
        movePoint.position = new Vector3(
            Mathf.Round(spawnPosition.x / tileSize) * tileSize,
            Mathf.Round(spawnPosition.y / tileSize) * tileSize,
            movePoint.position.z
        );
        DrawPlayer(0, 0, 
            Mathf.RoundToInt(spawnPosition.x / tileSize), 
            Mathf.RoundToInt(spawnPosition.y / tileSize)
        );   
        //Debug.Log($"Spawn position set to {spawnPosition}"); 
    }

    public void MovePosition()
    {
        // set player's current pos using movePoint & tileSize
        int playerX = Mathf.RoundToInt(movePoint.position.x / tileSize); // need to keep decimal bc its a small number but cast to int
        int playerY = Mathf.RoundToInt(movePoint.position.y / tileSize); // problems because this is rounding to 1 !! 
        // moving on X, Y
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // can only move vertically/ horizontally, not diagonally
        if (horizontal != 0) vertical = 0;
        if (vertical != 0) horizontal = 0;
        Debug.Log($"Input detected: Horizontal={horizontal}, Vertical={vertical}");

        // increment target based on player pos
        int targetX = playerX;
        int targetY = playerY;

        if (horizontal > 0) targetX += 1;  // Move right
        else if (horizontal < 0) targetX -= 1;  // Move left

        if (vertical > 0) targetY += 1;  // Move up
        else if (vertical < 0) targetY -= 1;  // Move down

        //Debug.Log($"MovePosition target: ({targetX}, {targetY})");

        // Check if the target tile is walkable
        if (CanMove(targetX, targetY))
        {   
            // Update the move point's position using targetX,Y var previously selected
            movePoint.position = new Vector3(targetX * tileSize, targetY * tileSize, movePoint.position.z);     
            DrawPlayer(playerX, playerY, targetX, targetY);  // Draw the player at the new position
            Debug.Log($"Player moved to new position: {targetX}, {targetY}");
        }
        else
        {
            Debug.Log($"Cannot move to position: {targetX}, {targetY}");
        }
    }
    // if player detects _winTile, reset position + get next map
    void LevelComplete()
    {
        Debug.Log("Level complete! Getting a new map...");
        loadMap.LoadPremadeMap();
        ResetPosition();
    }
    
    public void DrawPlayer(int previousX, int previousY, int currentX, int currentY)
    {
        TileBase noneTile = loadMap._none;
        Vector3Int previousPosition = new Vector3Int(previousX, previousY, 0);
        Vector3Int currentPosition = new Vector3Int(currentX, currentY, 0);

        if (myTilemap.HasTile(previousPosition)) 
        {
            myTilemap.SetTile(previousPosition, noneTile);
        }

        // Place the player tile at the new position
        myTilemap.SetTile(currentPosition, playerTile);
    }
}
