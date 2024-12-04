using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    public Tilemap myTilemap;
    public Vector3Int tilePosition;
    public EnemyController enemyController;

    [Header("Health Settings")]
    public int maxHealth = 50;
    public SpriteRenderer spriteRenderer;
    public int currentHealth;
    public int playerDamage = 10;
    private Color originalColor;

    [Header("UI Reference")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI enemyhealthText;

    private Coroutine flashCoroutine;

    void Awake()
    {
        currentHealth = maxHealth;
        enemyController.currentHealth = enemyController.maxHealth; 
    }
    void Start()
    {
        //Debug.Log($"Initial health set to {currentHealth}.");
        UpdateHealthUI();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }     
    // ---------- DAMAGE ---------- //
    public void TakeDamage(int damage)
    {
        Debug.Log($"Taking damage: {damage}. Health before damage: {currentHealth}");
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        Debug.Log($"Health after damage: {currentHealth}");
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(tilePosition);
        }
        else
        {
            FlashRed();
        }
    }
    // ---------- DIE ---------- //
    public void Die(Vector3Int position)
    {
        Debug.Log(gameObject.name + " has died!");

        if (myTilemap != null)
        {
            myTilemap.SetTile(position, null);
            Debug.Log($"Tile at {position} set to null.");
        }
    }
    // ---------- UI ---------- //
    public void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}";
            Debug.Log($"Updated health UI: {healthText.text}");
        }
        else
        {
            Debug.LogWarning("healthText is not assigned in HealthSystem.");
        }

        if (enemyhealthText != null && enemyController != null)
        {
            enemyhealthText.text = $"Enemy HP: {enemyController.currentHealth}";
            Debug.Log($"Updated enemy health UI: {enemyhealthText.text}");
        }
        else if (enemyhealthText == null)
        {
            Debug.LogWarning("enemyhealthText is not assigned in HealthSystem.");
        }
    }
    // ---------- DMG RED INDICATOR ---------- // FIXME: not working :/
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
        spriteRenderer.color = originalColor;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        enemyController.currentHealth = enemyController.maxHealth;
        UpdateHealthUI();
    }
}