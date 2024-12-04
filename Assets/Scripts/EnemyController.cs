using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Tilemap myTilemap;
    public LoadMap loadMap;
    public MovePlayer movePlayer;
    public HealthSystem healthSystem;
    public Combat combat;

    [Header("Enemy Stats")]
    public Vector3Int enemyPosition;
    public int maxHealth = 30;
    public int currentHealth;
    public int enemyDamage = 5; 
    public TileBase enemyTile;
    public float tileSize = 0.08f;
    public Transform enemyMovePoint;
    private int direction;

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
        //enemyPosition = position;
        //transform.position = myTilemap.GetCellCenterWorld(position); // Set position using world coordinates
        //Debug.Log($"Enemy initialized at {enemyPosition}");

        Vector3 enemyPosition = enemyMovePoint.transform.position;
        enemyMovePoint.position = new Vector3(
            Mathf.Round(enemyPosition.x / tileSize) * tileSize,
            Mathf.Round(enemyPosition.y / tileSize) * tileSize,
            enemyMovePoint.position.z
        );
    }

    public bool CanMove(int x, int y)
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
        if (tileAtPosition == movePlayer.playerTile)
        {
            combat.EnemyAttacksPlayer(enemyDamage);
        }
        return false;
    }

    // ---------- ENEMY AI MOVEMENT ---------- //
    public void MoveTowardsPlayer()
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
            //Debug.Log($"Player moved to new position: {targetX}, {targetY}");
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
