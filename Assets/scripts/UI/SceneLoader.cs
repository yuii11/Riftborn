using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{ 
    [SerializeField] private int sceneInd;
    public Button startButton;
    public void StartGame()
    {
        SceneTransitionManager.singleton.GoToSceneAsync(sceneInd);
    }
    void Start()
    { 

        //Hook events
        startButton.onClick.AddListener(StartGame);
    }
}
