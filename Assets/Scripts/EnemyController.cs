using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour // need to reference the _enemy tile
{
    [Header("References")]
    public Tilemap myTilemap;
    public LoadMap loadMap;
    public MovePlayer movePlayer;
    public HealthSystem healthSystem;
    public Combat combat;

    [Header("Enemy Stats")]
    public int maxHealth = 30;
    public int currentHealth;
    public int enemyDamage = 5; 
    public float tileSize = 0.08f;
    private int direction;

    [Header("Enemy Position / Tiles")]
    public Vector3Int enemyPosition;
    public Transform enemyMovePoint;
    public TileBase enemyTile;
    public TileBase playerTile;

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
    private void Update()
    {
        if (combat.enemyTurn == false)
        {
            return;
        }
        MoveTowardsPlayer();
    }
    public void Initialize(Vector3Int position)
    {
        Vector3 enemyPosition = enemyMovePoint.transform.position;
        enemyMovePoint.position = new Vector3(
            Mathf.Round(enemyPosition.x / tileSize) * tileSize,
            Mathf.Round(enemyPosition.y / tileSize) * tileSize,
            enemyMovePoint.position.z
        );
    }

    public bool CanMove(int x, int y) // enemy is spawned outside map as a clone, and when running into wall, goes to shit
    {
        // setting new variable to determine current position
        Vector3Int cellPosition = new Vector3Int(x, y, 0);
        // Get the tile at the specified grid position
        TileBase tileAtPosition = myTilemap.GetTile(cellPosition);

        Debug.Log($"Enemy is checking tile at ({x}, {y}): {tileAtPosition}");

        // Allow movement if the tile is null (empty) or is explicitly _none
        if (tileAtPosition == null || tileAtPosition == loadMap._none)
        {
            Debug.Log("Enemy can walk");
            return true;
        }

        // cannot move on wall, chest, door, or enemy tiles
        if (tileAtPosition == loadMap._wall ||
            tileAtPosition == loadMap._door ||
            tileAtPosition == loadMap._chest)
        {
            Debug.Log($"Enemy is blocked at ({x}, {y}): {tileAtPosition}");
            return false;
        }
        if (tileAtPosition == playerTile)
        {
            combat.enemyTurn = true;
            return false;
        }
        if (tileAtPosition == enemyTile) // enemy is checking its clones ??
        {
            return false;
        }
        return false;
    }

    // ---------- ENEMY AI MOVEMENT ---------- //
    public void MoveTowardsPlayer() // need to find the player through it's TILE and chase them from that position
    {
        int enemyX = Mathf.RoundToInt(enemyMovePoint.position.x / tileSize);
        int enemyY = Mathf.RoundToInt(enemyMovePoint.position.y / tileSize);

        int targetDirectionX = Mathf.RoundToInt(enemyPosition.x - movePlayer.movePoint.position.x);
        int targetDirectionY = Mathf.RoundToInt(enemyPosition.y - movePlayer.movePoint.position.y);

        // increment target based on player pos
        int targetX = enemyX + targetDirectionX;
        int targetY = enemyY + targetDirectionY;

        // Check if the tile is walkable
        if (CanMove(targetX, targetY)) // Check if the target tile is walkable
        {
            // Update the move point's position using targetX,Y var previously selected
            enemyMovePoint.position = new Vector3(
                targetX * tileSize,
                targetY * tileSize,
                enemyMovePoint.position.z
            );
            DrawEnemy(enemyX, enemyY, targetX, targetY);
            combat.enemyTurn = false;
            Debug.Log($"Enemy moved to new position: {targetX}, {targetY}");
        }
        if (!CanMove(targetX, targetY))
        {
            return;
        }
    }
    void DrawEnemy(int previousX, int previousY, int currentX, int currentY)
    {
        Vector3Int previousPosition = new Vector3Int(previousX, previousY, 0);
        Vector3Int currentPosition = new Vector3Int(currentX, currentY, 0);

        if (myTilemap.HasTile(previousPosition))
        {
            myTilemap.SetTile(previousPosition, null);
        }
        // Place the player tile at the new position
        myTilemap.SetTile(currentPosition, enemyTile);
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
