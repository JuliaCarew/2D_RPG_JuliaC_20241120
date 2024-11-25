using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float playerAggroRadius = 5f; // if the enemy is within certain circle aggro radius, enter combat
    public float enemySpeed = 5f;
    public bool enemyAggro;
    // set up enemy list to check all enemy tiles - assign state when checked 
    // reference healthsystem 

    void Start()
    {
        
    }

    void Update()
    {
        GetCombatState();
    }

    void GetCombatState()
    {
        // if enemy || player state == dead
        // return;

        // if enemy is withing playerAggro radius
        // enemyAggro == true
        // Attack() (take turns withing method)
        // call enemy pathing


    }
    public void Die()
    {
        // if enemy || player health <= 0
        // set active false
    }
    void EnemyPathing()
    {
        // lerp between furthest walkable tiles
    }

    void Attack()
    {
        // player attack first, then enemy
        // get tile that the enemy is on
        // damage that tile with damage -= health;
    }
    // combat states: idle(pathing), aggro(chase, enter combat state), win(exit combat state), lose(die)
}
