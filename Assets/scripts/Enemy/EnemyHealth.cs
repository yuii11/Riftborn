using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    public float currentHealth;

    public event Action OnDeath;  // Событие смерти

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            FindObjectOfType<KillCounter>().AddKill();
            EnemyCounter.Instance.AddKill();
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();  // Уведомляем о смерти
        Destroy(gameObject);
    }
}