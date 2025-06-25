using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;  // Префаб врага
    [SerializeField] private float spawnInterval = 5f;// Интервал спавна (в секундах)
    [SerializeField] private int maxEnemies = 10;     // Максимальное количество врагов

    private int currentEnemyCount = 0;                // Текущее количество врагов

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                currentEnemyCount++;
                enemy.GetComponent<EnemyHealth>().OnDeath += DecreaseEnemyCount;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void DecreaseEnemyCount()
    {
        currentEnemyCount--;
    }
}