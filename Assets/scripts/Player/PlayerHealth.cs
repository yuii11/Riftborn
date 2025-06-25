using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;       // ������������ ��������
    [SerializeField] private Slider healthBar;          // ������ �� ������� ��������
    [SerializeField] private TextMeshProUGUI healthText;           // ������ �� ����� ��������
    [SerializeField] private float deathDelay = 2f;     // �������� ����� ��������� �� GameOver
    [SerializeField] private Image damageOverlay;       // UI Image ��� ������� �����������
    [SerializeField] private float maxOverlayAlpha = 0.6f; // ������������ ������������ ��� ������ ��������
    [SerializeField] private float hitFlashAlpha = 0.8f;   // ������������ ��� �����
    [SerializeField] private float hitFlashDuration = 0.5f; // ������������ �������� �����

    private int currentHealth;
    private Coroutine hitFlashCoroutine;

    public int CurrentHealth { get { return currentHealth; } } // ������ ��� �������� ��������

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
        StartHitFlash(); // ��������� �������� ����������� ��� �����
        Debug.Log("����� ������� ����: " + damage + ". ��������: " + currentHealth);

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
            // ����������� ���������� ������ ��� �������� <= 50
            if (currentHealth <= 50)
            {
                // ������������ ������������ �� 0 (��� 50 HP) �� maxOverlayAlpha (��� 0 HP)
                float healthRatio = (float)currentHealth / 50f; // �� 1 (50 HP) �� 0 (0 HP)
                baseAlpha = (1f - healthRatio) * maxOverlayAlpha; // �� 0 �� maxOverlayAlpha
            }
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, baseAlpha);
        }
    }

    private void StartHitFlash()
    {
        if (damageOverlay != null)
        {
            // ������������� ���������� ��������, ���� ��� �������
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

        // ������� �����������
        while (Time.time < startTime + hitFlashDuration / 2)
        {
            float t = (Time.time - startTime) / (hitFlashDuration / 2);
            float alpha = Mathf.Lerp(baseAlpha, hitFlashAlpha, t);
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, alpha);
            yield return null;
        }

        // ������� ���������
        startTime = Time.time;
        while (Time.time < startTime + hitFlashDuration / 2)
        {
            float t = (Time.time - startTime) / (hitFlashDuration / 2);
            float alpha = Mathf.Lerp(hitFlashAlpha, baseAlpha, t);
            damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, alpha);
            yield return null;
        }

        // ������������� �������� ��������
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.g, damageOverlay.color.b, baseAlpha);
        hitFlashCoroutine = null;
    }

    private void Die()
    {
        Debug.Log("����� �����!");
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