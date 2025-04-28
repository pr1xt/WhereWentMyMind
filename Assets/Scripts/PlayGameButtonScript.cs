using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;
using Unity.VisualScripting;

public class PlayGameButton : MonoBehaviour {
	
	
	public Button yourButton;
    public GameManager manager;

	void Start () {
		yourButton.onClick.AddListener(TaskOnClick);
	}

    void OnDestroy(){
        yourButton.onClick.RemoveListener(TaskOnClick);
    }

	void TaskOnClick(){

		manager.StartGame();
	}
	

}