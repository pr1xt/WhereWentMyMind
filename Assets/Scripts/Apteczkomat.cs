using System.Collections;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Apteczkomat : MonoBehaviour
{
    public GameObject apteczkaPrefab;
    public Animator gamblerAnimator;
    public int SpawnX;
    public int SpawnZ;
    private bool isGambling = false;
    public KeyCode interactKey = KeyCode.E; // Default key for interaction
    private float animationLength = 9.5f;

    private KeyCode LoadKey() {
        string keyString = PlayerPrefs.GetString("InteractKey", "E");
        KeyCode keyCode;
        if (System.Enum.TryParse(keyString, out keyCode))
        {
            return keyCode;
        }
        else
        {
            return KeyCode.E;
        }
    }
    
    void Update()
    {
        interactKey = LoadKey(); // Load the key from PlayerPrefs
        GameObject InteractText = GameObject.FindWithTag("InteractText");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 3f)
        {
            if (InteractText != null)
            {
                InteractText.GetComponent<TextMeshProUGUI>().text = $"Press {interactKey} to gamble";
            } else {
                Debug.Log("InteractText not found");
            }

            if (Input.GetKeyDown(interactKey))
            {
                if (player.TryGetComponent<PlayerControler>(out var playerCoins)) {
                    if (playerCoins.coins >= 5 && !isGambling)
                    {
                        playerCoins.coins -= 5;
                        isGambling = true;
                        gamblerAnimator.SetBool("isPlaying", true);
                        gamblerAnimator.Play("GiveMeThatApteczka");
                        Invoke(nameof(EndAnim), animationLength);
                    }
                }
            }
        } else {
            if (InteractText != null)
            {
                InteractText.GetComponent<TextMeshProUGUI>().text = "";
            } else {
                Debug.Log("InteractText not found");
            }
        }
    }

    void EndAnim()
    {
        isGambling = false;
        Instantiate(apteczkaPrefab, new Vector3(transform.position.x + SpawnX, transform.position.y, transform.position.z-SpawnX), transform.rotation);
    }
}