using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private int totalKillsToWin = 30;
    private int currentKills = 0;

    public void AddKill()
    {
        currentKills++;
        int remaining = totalKillsToWin - currentKills;
        counterText.text = $"Óáèòî: {currentKills} / {totalKillsToWin}";
    }

    public int GetKills() => currentKills;
}
