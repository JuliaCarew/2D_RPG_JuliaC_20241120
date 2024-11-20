using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovePlayer : MonoBehaviour
{
    public LoadMap LoadMap;

    public Tilemap myTilemap;
    public Transform movePoint;
    public float tileSize = 0.08f;
    // need to snap player's position to tile, maybe by dividing position by tileSize !!

    // updating the player sprite
    public TileBase playerTile;
    private TileBase previousTile;

    void Start()
    {
        movePoint.parent = null;  // allows movepoint to dictate player's direction/ can be moved on it's own    
    }
    void Update()
    {
        MovePosition();
    }
    public bool CanMove(float x, float y)
    {
        // setting new variable to determine current position
        var gridPosition = new Vector3(x, y, 0);
        // checking tiles, needs to ref. MapBehaviors _tiles to collide correctly
        TileBase wallTile = LoadMap._wall;
        TileBase doorTile = LoadMap._door;
        TileBase chestTile = LoadMap._chest;
        TileBase enemyTile = LoadMap._enemy;
        TileBase noneTile = LoadMap._none;
        TileBase checkerTile = LoadMap._checker;

        // Get the tile at the specified grid position
        TileBase tileAtPosition = myTilemap.GetTile(myTilemap.WorldToCell(gridPosition));

        // cannot move on wall, chest, door, or enemy tiles
        if (tileAtPosition == wallTile ||
            tileAtPosition == doorTile ||
            tileAtPosition == chestTile ||
            tileAtPosition == enemyTile)
        {
            Debug.Log($"Cannot move on {tileAtPosition} at: {x}, {y}");
            return false;
        }
        // you can move on 'none' tiles
        if (tileAtPosition == noneTile || tileAtPosition == checkerTile)
        {
            Debug.Log($"CAN move on {tileAtPosition} at: {x}, {y}");
            return true;
        }
        return false;
    }
    public void MovePosition()
    {
        // set player's current pos using movePoint & tileSize
        var playerX = movePoint.position.x / tileSize; // need to keep decimal bc its a small number but cast to int
        var playerY = movePoint.position.y / tileSize; // problems because this is rounding to 1 !! 
        // moving on X, Y
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // can only move vertically/ horizontally, not diagonally
        if (horizontal != 0)
        {
            vertical = 0;
        }
        // increment target based on player pos
        var targetX = playerX;
        var targetY = playerY;

        if (horizontal > 0) targetX += 1; // move RIGHT
        else if (horizontal < 0) targetX -= 1; // move LEFT

        if (vertical > 0) targetY += 1; // move UP
        else if (vertical < 0) targetY -= 1; // move DOWN

        // Check if the target tile is walkable
        if (CanMove(targetX, targetY))
        {   // stores previous pos as player pos
            var previousX = playerX;
            var previousY = playerY;

            // Update the move point's position using targetX,Y var previously selected
            movePoint.position = new Vector3(targetX , targetY * tileSize, movePoint.position.z);

            // Draw the player at the new position
            //DrawPlayer(previousX, previousY, targetX, targetY);
        }
    }
    //public void DrawPlayer(float previousX, float previousY, float currentX, float currentY)
    //{
    //    TileBase noneTile = LoadMap._none;
    //    TileBase checkerTile = LoadMap._checker;
    //    // Replace the previous tile with none
    //    if (myTilemap.HasTile(new Vector3Int(previousX, previousY, 0)))
    //    {
    //        myTilemap.SetTile(new Vector3Int(previousX, previousY, 0), noneTile);
    //    }

    //    // Place the player tile at the new position
    //    myTilemap.SetTile(new Vector3Int(currentX, currentY, 0), playerTile);
    //}
}
// make a SetToStart() that finds the tile nearest to 0,0 and draws the player, call at the start -- irrelevant ??
