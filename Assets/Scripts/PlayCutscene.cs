using UnityEngine;
using UnityEngine.EventSystems;

public class PlayCutscene: MonoBehaviour, IPointerClickHandler {
    public GameObject gameManager;

	public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.GetComponent<GameManager>().sceneToLoad = "MainMenu";
        gameManager.GetComponent<GameManager>().StartGame();
    }
}