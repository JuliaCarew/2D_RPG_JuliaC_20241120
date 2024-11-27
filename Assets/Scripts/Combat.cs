using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Combat : MonoBehaviour
{
    [Header("References")]
    public MovePlayer movePlayer;
    public LoadMap loadMap;
    public Tilemap myTilemap;

    public List<Vector3Int> enemies = new List<Vector3Int>(); // generating list of all enemies in map
    // this list is not counting the enemies generated in the map ? they would have to be placed beforehand
    public bool enemyTurn; // enemies take turns when true, player takes turn when false

    void Start()
    {
        enemyTurn = false;
    }

    void Update()
    {
        if (!enemyTurn)
        {
            return;
        }
    }

    void CheckNeighbor(int x, int y)
    {
        int count = 0;
        // make array in a grid ranging from -1 to 1 ( starts from bottom left)
        for (int check_x = -1; check_x < 2; check_x++)
        {
            for (int check_y = -1; check_y < 2; check_y++)
            { // so the loop doesnt count the current cell/ itself
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tileAtPosition = myTilemap.GetTile(cellPosition);

                if (check_y == 0 && check_x == 0)
                {
                    continue;
                }
                if (tileAtPosition == movePlayer.playerTile)
                {
                    EnemyTurn();
                    //TakeDamage();
                    count++;
                }
            }
        }
    }

    public void PlayerTookTurn(Vector3Int playerPosition)
    {
        Debug.Log("Player's turn");

        foreach (var enemyPos in enemies)
        {
            if (Vector3Int.Distance(playerPosition, enemyPos) == 1) 
            {
                Debug.Log("Player attacks enemy!");
                AttackEnemy(enemyPos);
            }
        }

        enemyTurn = true;
        Invoke("EnemyTurn", 0.5f);
    }
    public void EnemyTurn() 
    {
        Debug.Log("Enemy's turn");

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            //Vector3Int enemyPos = enemyPositions[i];
            //Vector3Int playerPosition = myTilemap.WorldToCell(movePlayer.movePoint.position);
            
            //if (Vector3Int.Distance(enemyPos, playerPosition) == 1)
            //{
            //    Debug.Log("Enemy attacks player!");
            //    TakeDamage(enemyDamage);
            //    continue; // Enemy doesn't move if it attacks
            //}

            // Otherwise, path toward the player
            //MoveEnemyToPlayer(i, playerPosition);
        }
        enemyTurn = false;
    }
    void AttackEnemy(Vector3Int enemyPosition)
    {
        Debug.Log($"Damaging enemy at {enemyPosition}!");
        enemies.Remove(enemyPosition);
        //tilemap.SetTile(enemyPosition, null);
        // lerp between furthest walkable tiles
    }
}
