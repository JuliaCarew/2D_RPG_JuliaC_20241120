using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMove : MonoBehaviour
{
    public TileBase playerTile; 
    public TileBase enemyTile; // updating the enemy sprite
    public Tilemap myTilemap;
    public LoadMap loadMap;
    public MovePlayer movePlayer;

    public float tileSize = 0.08f;
    public float speed = 5.0f;
    public Transform enemyMovePoint;

    private void Update()
    {
        UpdateEnemyPosition();
    }
    bool CanMove(int x, int y) // need to reference all enemies, use list? 
    {
        // setting new variable to determine current position
        Vector3Int cellPosition = new Vector3Int(x, y, 0);
        // Get the tile at the specified grid position
        TileBase tileAtPosition = myTilemap.GetTile(cellPosition);

        //Debug.Log($"Checking tile at ({x}, {y}): {tileAtPosition}");

        // Allow movement if the tile is null (empty) or is explicitly _none
        if (tileAtPosition == null || tileAtPosition == loadMap._none)
        {
            //Debug.Log("CanWalk");
            return true;
        }

        // cannot move on wall, chest, door, or enemy tiles
        if (tileAtPosition == loadMap._wall ||
            tileAtPosition == loadMap._door ||
            tileAtPosition == loadMap._chest)
        {
            //Debug.Log($"Blocked tile at ({x}, {y}): {tileAtPosition}");
            return false;
        }
        if (tileAtPosition == playerTile)
        {
            //combat.PlayerTookTurn(); 
        }
        return false;
    }

    void UpdateEnemyPosition()
    {
        var player = movePlayer.movePoint;

        // call list of enemies made from map generation
        // iterate through list for .count and move position based on playerpos
        Vector2 playerPos = (player.transform.position - transform.position).normalized;
        // for (i = 0; i > enemypositions.Count, i++)
        // OR foreach (var enemy in enemyPositions) // would it work with diff enemies each map?
        //Vector3Int enemyCellPosition = myTilemap.WorldToCell(((TileBase)loadMap.enemyPositions).position);

        // need to get enemy positions based on the optput of the list in LoadMap

    }
    //void MoveEnemyPosition()
    //{
    //    var player = movePlayer.movePoint;
    //    TileBase enemyTile = loadMap._enemy;

    //    int enemyX = Mathf.RoundToInt(enemyMovePoint.position.x / tileSize); 
    //    int enemyY = Mathf.RoundToInt(enemyMovePoint.position.y / tileSize);
    //    // call movetowardsplayer
    //    Vector2 targetplayer = (player.transform.position - transform.position).normalized;
    //    transform.position = Vector2.MoveTowards(transform.position, targetplayer, speed * Time.deltaTime); // need to change tileseze to speed equivalent

    //    int targetplayerX = enemyX;
    //    int targetplayerY = enemyY;

    //    Debug.Log($"Moving enemy position... {enemyX} {enemyY} compare to {enemyCellPosition} target player {targetplayer}");

    //    if (CanMove(targetplayerX, targetplayerY))
    //    {
    //        Debug.Log("Enemy CAN move");
    //        // Update the move point's position using targetX,Y var previously selected
    //        enemyMovePoint.position = new Vector3(
    //            targetplayerX * tileSize,
    //            targetplayerY * tileSize,
    //            enemyMovePoint.position.z
    //        );
    //        DrawEnemy(enemyX, enemyY, targetplayerX, targetplayerY);  // Draw at the new position
    //        FindObjectOfType<Combat>().EnemyTurn();
    //        //Debug.Log($"Player moved to new position: {targetX}, {targetY}");
    //    }
    //}

    void DrawEnemy(int previousX, int previousY, int currentX, int currentY)
    {
        TileBase enemyTile = loadMap._enemy;

        Vector3Int previousPosition = new Vector3Int(previousX, previousY, 0);
        Vector3Int currentPosition = new Vector3Int(currentX, currentY, 0);

        if (myTilemap.HasTile(previousPosition))
        {
            myTilemap.SetTile(previousPosition, null);
        }
        Debug.Log("Drawing enemy");
        // Place tile at the new position
        myTilemap.SetTile(currentPosition, enemyTile);
    }
}
// on enemy turn, they either move towards player or attack player if in neighboring tile
// need to have AI pathing, line of sight? take player radius from vector game & give it to enemies?
// needs to be so that every time playermove is successful, enemy move is called. if enemy detects player withinneighbors call enemyturn