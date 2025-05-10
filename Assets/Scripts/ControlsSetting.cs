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
        // Loading the key from PlayerPrefs
        string keyString = PlayerPrefs.GetString(controlType + "Key", buttonText.text);
        KeyCode keyCode;
        if (System.Enum.TryParse(keyString, out keyCode))
        {
            key = keyCode;
        } else
        {
            // If the key is not found, set it to the default value (buttonText.text)
            
            key = (KeyCode) System.Enum.Parse(typeof(KeyCode), buttonText.text);
        }
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
        PlayerPrefs.Save();
    }
}