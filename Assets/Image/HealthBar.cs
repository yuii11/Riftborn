using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    public Transform target; // Цель, на которую будет направлена полоска
    public Image healthFill; // Заполняющая часть полоски
    public EnemyHealth health;

    private void Start()
    {
        // Находим игрока по тегу
        FindPlayer();
    }
    private void LateUpdate()
    {
        // Поворачиваем полоску к камере
        if (target != null)
        {
            transform.LookAt(Camera.main.transform);
        }
        else
        {

        }
    }

    public void Update()
    {
        // Обновляем заполнение полоски
        if (healthFill != null)
        {
            healthFill.fillAmount = health.currentHealth / health.maxHealth;
        }
        else
        {

        }
    }
    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            target = playerObj.transform;
            Debug.Log("Игрок найден для HealthBar: " + playerObj.name);
        }
        else
        {
            Debug.LogWarning("Игрок с тегом '" + playerTag + "' не найден для HealthBar!");
        }
    }
}
