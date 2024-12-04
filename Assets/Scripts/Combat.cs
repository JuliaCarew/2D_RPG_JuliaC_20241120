using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Combat : MonoBehaviour // FIXME: player stops taking input after y: 6
{
    [Header("References")]
    public MovePlayer movePlayer;
    public LoadMap loadMap;
    public Tilemap myTilemap;
    public HealthSystem playerHealthSystem;
    public HealthSystem enemyHealthSystem;
    public EnemyController enemy;

    [Header("Game Objects")]
    public GameObject gameOverUI;

    [Header("Turn Handler")]
    public bool enemyTurn; // enemies take turns when true, player takes turn when false
    public float turnDelay = 0.2f;

    void Start()
    {
        if (enemyTurn == false)
        {
            PlayerTookTurn();
        }
        if (enemyTurn == true)
        {
            EnemyTurn();
        }

        enemyTurn = false;
        gameOverUI.SetActive(false);
    }
   
    // ---------- PLAYER TURN ---------- // 
    public void PlayerTookTurn()
    {
        //enemyTurn = false;

        Debug.Log("Player's turn");

        if (HasTargetNeighbor())
        {
            Debug.Log("Player attacks enemy!");
            PlayerAttacksEnemy(playerHealthSystem.playerDamage);
            enemyTurn = true;

            if (enemy.healthSystem.currentHealth <= 0)
            {
                Debug.Log("Enemy defeated!");
                myTilemap.SetTile(enemy.enemyPosition, null);
                Destroy(enemy.gameObject);
            }
        }
        EnemyTurn();
    }

    // ---------- ENEMY TURN ---------- //
    public void EnemyTurn() 
    {
        //enemyTurn = true;

        Debug.Log("Enemy's turn");

        Vector3Int playerPosition = Vector3Int.RoundToInt(movePlayer.movePoint.position);

        if (HasTargetNeighbor())
        {
            Debug.Log("Enemy attacks player!");
            EnemyAttacksPlayer(enemy.enemyDamage);
            enemyTurn = false;
        }
    }

    // ---------- CHECK NEIGHBOR ---------- //
    private bool HasTargetNeighbor() // right now is checking grid which had bigger tiles, take from player movement?
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Skip the center tile
                if (x == 0 && y == 0)
                continue;

                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                // Get the tile at the specified grid position
                var tileAtPosition = myTilemap.WorldToCell(cellPosition);

                Vector3Int checkPosition = tileAtPosition + new Vector3Int(x, y, 0);

                // Compare the target position to the adjacent positions
                if (checkPosition == enemy.enemyPosition || checkPosition == movePlayer.movePoint.position)
                {
                    Debug.Log($"Target found at {checkPosition}");
                    return true;
                }
            }
        }
        return false;
    }

    // ---------- ATTACK HANDLERS ---------- //
    public void EnemyAttacksPlayer(int enemyDamage)
    {
        playerHealthSystem.TakeDamage(enemyDamage);
        Debug.Log("Player takes " + enemyDamage + " damage.");
        if (playerHealthSystem.currentHealth <= 0)
        {
            Debug.Log("Player has died! Game Over.");
            gameOverUI.SetActive(true); // Show GameOver UI
        }
    }
    public void PlayerAttacksEnemy(int playerDamage)
    {
        enemyHealthSystem.TakeDamage(playerDamage);
        Debug.Log("Enemy takes " + playerDamage + " damage.");
    }
}