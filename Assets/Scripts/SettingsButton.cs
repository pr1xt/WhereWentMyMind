using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour {
	public Button yourButton;
    public GameObject menu;
    public GameObject settings;
	public GameObject pause;

	void Start () {
		yourButton.onClick.AddListener(TaskOnClick);
	}

    void OnDestroy(){
        yourButton.onClick.RemoveListener(TaskOnClick);
    }

	void TaskOnClick(){
		// check if it is the button in main menu or settings
		if (menu) {
			menu.SetActive(false);
			settings.SetActive(true);
		} else {
			settings.SetActive(true);
			pause.GetComponent<Renderer>().enabled = false;
		}
	}
}