using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Combat : MonoBehaviour
{
    public List<Vector3Int> enemyPositions = new List<Vector3Int>(); // generating list of all enemies in map
    public bool enemyTurn; // enemies take turns when true, player takes turn when false
    // reference healthsystem 
    public int currentHealth;
    public int maxHealth = 50;
    //int rndDamage = Random.Range(5,12);
    public int enemyDamage = 5; // Random.Range(5,10);
    public int playerDamage = 10; // Random.Range(7,12);
    public MovePlayer movePlayer;
    public Tilemap tilemap;

    void Start()
    {
        currentHealth = maxHealth;
        enemyTurn = false;
    }

    void Update()
    {
        if (!enemyTurn)
        {
            return;
        }
    }

    public void PlayerTookTurn(Vector3Int playerPosition)
    {
        Debug.Log("Player's turn");

        foreach (var enemyPos in enemyPositions)
        {
            if (Vector3Int.Distance(playerPosition, enemyPos) == 1) // Adjacent tiles
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

         for (int i = enemyPositions.Count - 1; i >= 0; i--)
        {
            Vector3Int enemyPos = enemyPositions[i];
            Vector3Int playerPosition = tilemap.WorldToCell(movePlayer.movePoint.position);
            
            if (Vector3Int.Distance(enemyPos, playerPosition) == 1)
            {
                Debug.Log("Enemy attacks player!");
                TakeDamage(enemyDamage);
                continue; // Enemy doesn't move if it attacks
            }

            // Otherwise, path toward the player
            MoveEnemyToPlayer(i, playerPosition);
        }
        enemyTurn = false;
    }
    private void MoveEnemyToPlayer(int enemyIndex, Vector3Int playerPosition)
    {

    }
    void AttackEnemy(Vector3Int enemyPosition)
    {
        Debug.Log($"Damaging enemy at {enemyPosition}!");
        enemyPositions.Remove(enemyPosition);
        tilemap.SetTile(enemyPosition, null);
        // lerp between furthest walkable tiles
    }
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player takes {damage} damage! Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player died :(");
        // set active false
        //tilemap.SetTile(null);
    }
}
