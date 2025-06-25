using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;  // ������ �����
    [SerializeField] private float spawnInterval = 5f;// �������� ������ (� ��������)
    [SerializeField] private int maxEnemies = 10;     // ������������ ���������� ������

    private int currentEnemyCount = 0;                // ������� ���������� ������

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