using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;    // Панель меню паузы
    [SerializeField] private GameObject gameUICanvas;      // Игровой UI
    [SerializeField] private GameObject settingsPanel;     // Панель настроек
    [SerializeField] private Button continueButton;        // Кнопка "Продолжить"
    [SerializeField] private Button settingsButton;        // Кнопка "Настройки"
    [SerializeField] private Button exitButton;            // Кнопка "Выйти"
    [SerializeField] private Button backButton;            // Кнопка "Назад" в настройках
    [SerializeField] private Slider volumeSlider;          // Слайдер громкости
    [SerializeField] private Slider sensitivitySlider;     // Слайдер чувствительности
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape; // Клавиша для паузы

    private bool isPaused = false; // Флаг состояния паузы

    void Start()
    {
        // Убедимся, что меню паузы и настройки скрыты, а игровой UI виден
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (gameUICanvas != null)
        {
            gameUICanvas.SetActive(true);
        }

        // Назначаем обработчики для кнопок
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ContinueGame);
        }
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToPauseMenu);
        }

        // Настраиваем слайдеры и загружаем сохранённые значения
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 1f);
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        // Применяем начальную громкость
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    void Update()
    {
        // Проверяем нажатие клавиши паузы
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
            {
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    BackToPauseMenu(); // Возвращаемся из настроек в меню паузы
                }
                else
                {
                    ContinueGame(); // Возобновляем игру
                }
            }
            else
            {
                PauseGame(); // Открываем меню паузы
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        if (gameUICanvas != null)
        {
            gameUICanvas.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (gameUICanvas != null)
        {
            gameUICanvas.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OpenSettings()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    private void BackToPauseMenu()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    private void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    private void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        PlayerPrefs.Save();
        UpdateCameraSensitivity(sensitivity);
    }

    private void UpdateCameraSensitivity(float sensitivity)
    {
        // Используем FindFirstObjectByType вместо устаревшего FindObjectOfType
        FirstPersonCamera cameraController = FindFirstObjectByType<FirstPersonCamera>();
        if (cameraController != null)
        {
            cameraController.mouseSensitivity = sensitivity;
        }
        else
        {
            Debug.LogWarning("FirstPersonCamera не найден в сцене!");
        }
    }
}