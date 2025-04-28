using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class ControlsSetting : MonoBehaviour
{
    public KeyCode key;
    public Button button;
    bool isWaitingForKey = false;
    public TMPro.TextMeshProUGUI buttonText;
    public String controlType;
    public GameObject gamblingGun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (!isWaitingForKey) {
            StartCoroutine(waitForKeyPress());
        }
    }

    private IEnumerator waitForKeyPress()
    {
        isWaitingForKey = true;
        Debug.Log("Naciśnij dowolny klawisz...");

        //Wait for key press
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        //Get the key that was pressed
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                key = keyCode;
                isWaitingForKey = false;
                ChangeKey();
                break;
            }
        }
            
        yield return null;
    }

    void ChangeKey()
    {
        // Update the button text
        buttonText.text = key.ToString();
        PlayerPrefs.SetString(controlType + "Key", key.ToString());
        Debug.Log(controlType + "Keystring: " + key.ToString() + "keycode: " + key);
        PlayerPrefs.Save();
    }
}