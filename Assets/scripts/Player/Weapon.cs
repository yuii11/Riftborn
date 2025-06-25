using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public int killCount = 0;
    [SerializeField] private TextMeshProUGUI killText; // UI-текст на оружии

    private void Start()
    {
        UpdateKillText();
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        enemy.OnDeath -= OnEnemyKilled; // снимаем старую подписку (если была)
        enemy.OnDeath += OnEnemyKilled; // подписываем заново
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
