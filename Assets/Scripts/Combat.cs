using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Combat : MonoBehaviour
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
        enemyTurn = false;
        gameOverUI.SetActive(false);
    }

    // ---------- PLAYER TURN ---------- //
    public void PlayerTookTurn(Vector3Int playerPosition)
    {
        //Debug.Log("Player's turn");

        if (CheckAdjacentTiles(playerPosition, enemy.enemyPosition))
        {
            Debug.Log("Player attacks enemy!");
            PlayerAttacksEnemy(playerHealthSystem.playerDamage);

            if (enemy.healthSystem.currentHealth <= 0)
            {
                Debug.Log("Enemy defeated!");
                myTilemap.SetTile(enemy.enemyPosition, null);
                Destroy(enemy.gameObject);
            }
        }
        enemyTurn = true;
        Invoke("EnemyTurn", turnDelay);
    }

    // ---------- ENEMY TURN ---------- //
    public void EnemyTurn() 
    {
        //Debug.Log("Enemy's turn");

        Vector3Int playerPosition = Vector3Int.RoundToInt(movePlayer.movePoint.position);

        if (CheckAdjacentTiles(enemy.enemyPosition, playerPosition))
        {
            Debug.Log("Enemy attacks player!");
            EnemyAttacksPlayer(enemy.enemyDamage);
        }
        enemyTurn = false;
    }

    // ---------- CHECK NEIGHBOR ---------- //
    private bool CheckAdjacentTiles(Vector3Int currentPosition, Vector3Int targetPosition)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Skip the center tile
                if (x == 0 && y == 0)
                continue;

                Vector3Int checkPosition = currentPosition + new Vector3Int(x, y, 0);

                // Compare the target position to the adjacent positions
                if (checkPosition == targetPosition)
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
// player is starting to take their own damage (10 dmg)
// reset after starting game not working
// enemy damage needs to only trigger when around enemy, not far away & not on null tile