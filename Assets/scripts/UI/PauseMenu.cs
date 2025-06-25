using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;    // ������ ���� �����
    [SerializeField] private GameObject gameUICanvas;      // ������� UI
    [SerializeField] private GameObject settingsPanel;     // ������ ��������
    [SerializeField] private Button continueButton;        // ������ "����������"
    [SerializeField] private Button settingsButton;        // ������ "���������"
    [SerializeField] private Button exitButton;            // ������ "�����"
    [SerializeField] private Button backButton;            // ������ "�����" � ����������
    [SerializeField] private Slider volumeSlider;          // ������� ���������
    [SerializeField] private Slider sensitivitySlider;     // ������� ����������������
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape; // ������� ��� �����

    private bool isPaused = false; // ���� ��������� �����

    void Start()
    {
        // ��������, ��� ���� ����� � ��������� ������, � ������� UI �����
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

        // ��������� ����������� ��� ������
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

        // ����������� �������� � ��������� ���������� ��������
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

        // ��������� ��������� ���������
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    void Update()
    {
        // ��������� ������� ������� �����
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
            {
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    BackToPauseMenu(); // ������������ �� �������� � ���� �����
                }
                else
                {
                    ContinueGame(); // ������������ ����
                }
            }
            else
            {
                PauseGame(); // ��������� ���� �����
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
        // ���������� FindFirstObjectByType ������ ����������� FindObjectOfType
        FirstPersonCamera cameraController = FindFirstObjectByType<FirstPersonCamera>();
        if (cameraController != null)
        {
            cameraController.mouseSensitivity = sensitivity;
        }
        else
        {
            Debug.LogWarning("FirstPersonCamera �� ������ � �����!");
        }
    }
}