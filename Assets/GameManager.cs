using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera cutsceneCamera; 
    public GameObject menuUI;
    public PlayableDirector cutsceneDirector;
    private bool cutscenePlayed = false;
    public string sceneToLoad = "Game"; 

    public void StartGame()
    {
        cutsceneCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        menuUI.SetActive(false); 
        cutsceneDirector.Play();
        cutscenePlayed = true;
    }
    void Update()
    {
        if ((cutscenePlayed && cutsceneDirector.state != PlayState.Playing) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            cutsceneDirector.Stop();
            cutsceneCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(false);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

