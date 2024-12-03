using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    // public MovePlayer movePlayer;
    // public LoadMap loadMap;
    public Tilemap myTilemap;
    public Vector3Int tilePosition;
    public SpriteRenderer spriteRenderer;

    [Header("Health Settings")]
    public int maxHealth = 50;
    public int currentHealth;
    public int enemyDamage = 5; 
    public int playerDamage = 10;

    [Header("UI Reference")]
    public TextMeshProUGUI healthText;
    //public Transform damageTextSpawnPoint;

    private Coroutine flashCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"Initial health set to {currentHealth}.");
        UpdateHealthUI();
    }     
    
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        UpdateHealthUI();
        //ShowDamageText(damage);
        FlashRed();

        Debug.Log($"Entity takes {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("something died :(");

        if (myTilemap != null)
        {
            myTilemap.SetTile(tilePosition, null);
            Debug.Log($"Tile at {tilePosition} set to null.");
        }
        else{
            Debug.LogWarning("Tilemap reference not assigned");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}";
            Debug.Log($"Updated health UI: {healthText.text}");
        }
        else
        {
            Debug.LogWarning("HealthText UI reference is not assigned!");
        }
    }

    // private void ShowDamageText(int damage)
    // {
    //     if (damageTextPrefab != null && damageTextSpawnPoint != null)
    //     {
    //         GameObject damageText = Instantiate(damageTextPrefab, damageTextSpawnPoint.position, Quaternion.identity);
    //         TextMeshProUGUI textComponent = damageText.GetComponent<TextMeshProUGUI>();
    //         if (textComponent != null)
    //         {
    //             textComponent.text = $"-{damage}";
    //         }
    //         Destroy(damageText, 1.0f); // Destroy after 1 second
    //     }
    // }

    private void FlashRed()
    {
        if (spriteRenderer == null) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRedCoroutine());
    }

    private IEnumerator FlashRedCoroutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
// make a helper mathod that clamps the maximum health at 50, and the minimum health at 0
// return value to clamp, minimum, maximum

// separate healthsystem logic into here, leave the rest do be the turns