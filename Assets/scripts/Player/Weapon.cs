using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public int killCount = 0;
    [SerializeField] private TextMeshProUGUI killText; // UI-����� �� ������

    private void Start()
    {
        UpdateKillText();
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        enemy.OnDeath -= OnEnemyKilled; // ������� ������ �������� (���� ����)
        enemy.OnDeath += OnEnemyKilled; // ����������� ������
    }


    private void OnEnemyKilled()
    {
        killCount++;
        UpdateKillText();
    }

    private void UpdateKillText()
    {
        killText.text = "" + killCount;
    }
}
