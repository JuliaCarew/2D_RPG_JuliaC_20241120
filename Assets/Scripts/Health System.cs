using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    public MovePlayer movePlayer;
    public LoadMap loadMap;
    public Tilemap myTilemap;

    // set both player and enemy tiles to red when takign damage
    // need references to both tiles since theyre instanced in-game (to change color)
    private TileBase _player;
    private TileBase _enemy;

    [Header("Health System")]
    //public HealthSystem healthSystem = new HealthSystem(); // FIXME: causes stack overflow
    // reference healthsystem 
    public int currentHealth;
    public int maxHealth = 50;
    //int rndDamage = Random.Range(5,12);
    public int enemyDamage = 5; // Random.Range(5,10);
    public int playerDamage = 10; // Random.Range(7,12);

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // make tile Color.red
        // public void SetColor(Vector3Int position, Color color);
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
        //GetComponent<Tilemap>().SetTile(position, null); // can't set tile w/o getting position, but needs to be specific to enemy/player
    }
}
// make a helper mathod that clamps the maximum health at 50, and the minimum health at 0
// return value to clamp, minimum, maximum

// separate healthsystem logic into here, leave the rest do be the turns