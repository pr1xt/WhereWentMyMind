using System.Collections;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GamblingMachine : MonoBehaviour
{
    public GameObject[] pickUps;
    public Animator gamblerAnimator;
    public int SpawnX;
    public int SpawnZ;
    private bool isGambling = false;
    [SerializeField] private AudioSource gamblingSound;
    public KeyCode interactKey = KeyCode.E; // Default key for interaction
    private float animationLength = 7.0f;

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
                    if (playerCoins.coins >= 1 && !isGambling)
                    {
                        playerCoins.coins -= 1;
                        isGambling = true;
                        gamblerAnimator.SetBool("isPlaying", true);
                        gamblingSound.volume = PlayerPrefs.GetFloat("Volume", 1f);
                        gamblingSound.Play();
                        if (UnityEngine.Random.Range(0, 100) < 50) 
                        {
                            // wait until the animation is done before dropping loot
                            gamblerAnimator.Play("win");
                            Invoke(nameof(Win), animationLength);
                        } else {
                            gamblerAnimator.Play("loss");
                            Invoke(nameof(EndAnim), animationLength);
                        }
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
    }

    void Win()
    {
        isGambling = false;
        Instantiate(pickUps[Random.Range(0, pickUps.Length)], new Vector3(transform.position.x + SpawnX, transform.position.y, transform.position.z-SpawnX), transform.rotation);
    }
}