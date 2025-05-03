using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class CutscenesButton: MonoBehaviour {
	public Button cutscenesButton;
    public GameObject menu;
	public GameObject cutscenes;

	void Start () {
		cutscenesButton.onClick.AddListener(OpenCutscenes);
	}

    void OnDestroy(){
        cutscenesButton.onClick.RemoveListener(OpenCutscenes);
    }

	void OpenCutscenes(){
		menu.SetActive(false);
		cutscenes.SetActive(true);
	}
}