using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            menu.SetActive(true);
            settings.SetActive(false);
        } else {
            settings.SetActive(false);
            pause.GetComponent<Renderer>().enabled = true;
        }
	}
}
