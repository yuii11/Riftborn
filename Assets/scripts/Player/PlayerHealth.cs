using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;       // Максимальное здоровье
    [SerializeField] private Slider healthBar;          // Ссылка на полоску здоровья
    [SerializeField] private TextMeshProUGUI healthText;           // Ссылка на текст здоровья
    [SerializeField] private float deathDelay = 2f;     // Задержка перед переходом на GameOver
    [SerializeField] private Image damageOverlay;       // UI Image для эффекта покраснения
    [SerializeField] private float maxOverlayAlpha = 0.6f; // Максимальная прозрачность при низком здоровье
    [SerializeField] private float hitFlashAlpha = 0.8f;   // Прозрачность при ударе
    [SerializeField] private float hitFlashDuration = 0.5f; // Длительность анимации удара

    private int currentHealth;
    private Coroutine hitFlashCoroutine;

    public int CurrentHealth { get { return currentHealth; } } // Геттер для текущего здоровья

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateDamageOverlay();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthUI();
        UpdateDamageOverlay();
        StartHitFlash(); // Запускаем анимацию покраснения при ударе
        Debug.Log("Игрок получил урон: " + damage + ". Здоровье: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    private void UpdateDamageOverlay()
    {
        if (damageOverlay != null)
        {
            float baseAlpha = 0f;
            // Покраснение начинается только при здоровье <= 50
            if (currentHealth <= 50)
            {
                // Масштабируем прозрачность от 0 (при 50 HP) до maxOverlayAlpha (при 0 HP)
                float healthRatio = (float)currentHealth / 50f; // От 1 (50 HP) до 0 (0 HP)
                baseAlpha = (1f - healthRatio) * maxOverlayAlpha; // От 0 до maxOverlayAlpha
            }
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, baseAlpha);
        }
    }

    private void StartHitFlash()
    {
        if (damageOverlay != null)
        {
            // Останавливаем предыдущую анимацию, если она активна
            if (hitFlashCoroutine != null)
            {
                StopCoroutine(hitFlashCoroutine);
            }
            hitFlashCoroutine = StartCoroutine(HitFlashEffect());
        }
    }

    private IEnumerator HitFlashEffect()
    {
        if (damageOverlay == null) yield break;

        float startTime = Time.time;
        float baseAlpha = currentHealth <= 50 ? (1f - (float)currentHealth / 50f) * maxOverlayAlpha : 0f;

        // Быстрое покраснение
        while (Time.time < startTime + hitFlashDuration / 2)
        {
            float t = (Time.time - startTime) / (hitFlashDuration / 2);
            float alpha = Mathf.Lerp(baseAlpha, hitFlashAlpha, t);
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, alpha);
            yield return null;
        }

        // Плавное затухание
        startTime = Time.time;
        while (Time.time < startTime + hitFlashDuration / 2)
        {
            float t = (Time.time - startTime) / (hitFlashDuration / 2);
            float alpha = Mathf.Lerp(hitFlashAlpha, baseAlpha, t);
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, alpha);
            yield return null;
        }

        // Устанавливаем конечное значение
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, baseAlpha);
        hitFlashCoroutine = null;
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene("GameOver");
    }
}