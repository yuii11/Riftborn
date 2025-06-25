using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter Instance;
    public int killCount = 0;
    public int killsToWin = 30;
    public string winSceneName = "WinScene";
    public float winDelay = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddKill()
    {
        killCount++;
        Debug.Log($"Убито врагов: {killCount}");

        if (killCount >= killsToWin)
        {
            StartCoroutine(LoadWinSceneAfterDelay());
        }
    }

    private IEnumerator LoadWinSceneAfterDelay()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(winSceneName);
        loadOp.allowSceneActivation = false;

        yield return new WaitForSeconds(winDelay);

        // Здесь можно добавить анимацию победы или затемнение

        loadOp.allowSceneActivation = true; // Активируем сцену после задержки
    }
}
