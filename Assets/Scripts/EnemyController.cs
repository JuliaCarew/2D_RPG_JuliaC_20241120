using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Tilemap myTilemap;
    public LoadMap loadMap;
    public MovePlayer movePlayer;
    public HealthSystem healthSystem; 

    [Header("Enemy Stats")]
    public Vector3Int enemyPosition;
    public int maxHealth = 30;
    public int currentHealth;
    public int enemyDamage = 5; 
    public TileBase enemyTile;

    void Start()
    {
        if (healthSystem == null)
        {
            healthSystem = GetComponent<HealthSystem>(); 
            if (healthSystem == null)
            {
                Debug.LogError($"HealthSystem is missing on {gameObject.name}. Please attach one.");
                return;
            }
        }
    }

    public void Initialize(Vector3Int position)
    {
        enemyPosition = position;
        transform.position = myTilemap.GetCellCenterWorld(position); // Set position using world coordinates
        Debug.Log($"Enemy initialized at {enemyPosition}");
    }
    
    // ---------- ENEMY AI MOVEMENT ---------- //
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

    // ---------- HEALTH SYSTEM ---------- //
    public void TakeDamage(int damage)
    {
        if (healthSystem != null)
        {
            healthSystem.TakeDamage(damage);
            healthSystem.UpdateHealthUI();
        }
    }
    public void Die()
    {
        if (healthSystem != null)
        {
            healthSystem.Die(enemyPosition);
        }
        Destroy(gameObject);
    }

}
