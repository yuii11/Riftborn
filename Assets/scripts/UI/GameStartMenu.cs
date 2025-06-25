using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject options;
    public GameObject loading;
    public GameObject levels;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button optionButton;
    public Button quitButton;
    public Button levelButton;

    [SerializeField] private int sceneInd;
    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(EnableOption);
        quitButton.onClick.AddListener(QuitGame);
        levelButton.onClick.AddListener(EnableLevels);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }
    public void EnableLevels()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        levels.SetActive(true);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        ActiveLoading();


        SceneTransitionManager.singleton.GoToSceneAsync(sceneInd);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        levels.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        levels.SetActive(false);
    }
    public void EnableOption()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        levels.SetActive(false);
    }
    public void EnableAbout()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        levels.SetActive(false);
    }
    public void ActiveLoading()
    {
        mainMenu.SetActive(false);
        loading.SetActive(true);
        options.SetActive(false);
        levels.SetActive(false);
    }
}
