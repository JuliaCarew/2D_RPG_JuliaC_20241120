using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Combat : MonoBehaviour
{
    [Header("References")]
    public MovePlayer movePlayer;
    //public LoadMap loadMap;
    public Tilemap myTilemap;
    public HealthSystem healthSystem;

    public List<Vector3Int> enemies = new List<Vector3Int>(); // generating list of all enemies in map
    public bool enemyTurn; // enemies take turns when true, player takes turn when false
    public float turnDelay = 0.2f;
    public GameObject damageTextPrefab;


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

    public void EnemyTurn() 
    {
        Debug.Log("Enemy's turn");

        foreach (var enemyPos in enemies)
        {
            if (Vector3Int.Distance(healthSystem.myTilemap.WorldToCell(healthSystem.transform.position), enemyPos) == 1) // If adjacent
            {
                Debug.Log($"Enemy at {enemyPos} attacks player!");
                healthSystem.TakeDamage(healthSystem.enemyDamage); // Enemy damages player
                ShowDamageText(healthSystem.enemyDamage, healthSystem.myTilemap.WorldToCell(healthSystem.transform.position)); // Show damage text for player
                break; // Only one attack per turn
            }
        }
        enemyTurn = false;
    }

    void CheckForPlayerNearEnemy(Vector3Int enemyPosition)
    {
        Vector3Int[] neighboringTiles = 
        {
            Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down,
            new Vector3Int(-1, 1, 0), new Vector3Int(1, 1, 0),
            new Vector3Int(-1, -1, 0), new Vector3Int(1, -1, 0)
        };

         foreach (var offset in neighboringTiles)
        {
            Vector3Int neighborPosition = enemyPosition + offset;
            TileBase tileAtPosition = myTilemap.GetTile(neighborPosition);

            if (tileAtPosition == movePlayer.playerTile) // FIXME: Player is within range
            {
                Debug.Log($"Enemy at {enemyPosition} attacks player at {neighborPosition}!");
                healthSystem.TakeDamage(healthSystem.enemyDamage);
                break;
            }
        }
    }

    public void PlayerTookTurn(Vector3Int playerPosition)
    {
        Debug.Log("Player's turn");

        List<Vector3Int> enemiesToRemove = new List<Vector3Int>(); // Temporary list to store enemies to remove

        foreach (var enemyPos in enemies)
        {
            if (Vector3Int.Distance(playerPosition, enemyPos) == 1) 
            {
                Debug.Log("Player attacks enemy!");
                enemiesToRemove.Add(enemyPos); // Mark enemy for removal
                ShowDamageText(healthSystem.playerDamage, enemyPos); // Show damage text
            }
        }
        foreach (var enemyPos in enemiesToRemove) // Remove enemies after the iteration
        {
            enemies.Remove(enemyPos);
        }
        enemyTurn = true;
        Invoke("EnemyTurn", turnDelay);
    }

    private void ShowDamageText(int damage, Vector3Int position)
    {
        // Create a new damage text object at the position
        GameObject damageTextObject = Instantiate(damageTextPrefab, myTilemap.CellToWorld(position), Quaternion.identity);
        TextMeshProUGUI damageText = damageTextObject.GetComponent<TextMeshProUGUI>();
        damageText.text = $"-{damage}";
        
        Destroy(damageTextObject, 1.5f);
    }
}
