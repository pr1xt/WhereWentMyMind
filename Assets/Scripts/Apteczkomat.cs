using System.Collections;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Apteczkomat : MonoBehaviour
{
    public GameObject apteczkaPrefab;
    public Animator gamblerAnimator;
    public float SpawnX;
    public float SpawnY;
    public float SpawnZ;
    private bool isGambling = false;
    public KeyCode interactKey = KeyCode.E; // Default key for interaction
    private float animationLength = 9.5f;
    [SerializeField] private GameObject payApteczkaText; 

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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 3f)
        {
            payApteczkaText.GetComponent<TextMeshProUGUI>().text = $"Press {interactKey} to buy health pack";
            payApteczkaText.SetActive(true);

            if (Input.GetKeyDown(interactKey))
            {
                if (player.TryGetComponent<PlayerControler>(out var playerCoins)) {
                    if (playerCoins.coins >= 1 && !isGambling)
                    {
                        playerCoins.coins -= 1;
                        isGambling = true;
                        gamblerAnimator.SetBool("isPlaying", true);
                        gamblerAnimator.Play("GiveMeThatApteczka");
                        Invoke(nameof(EndAnim), animationLength);
                    }
                }
            }
        } else {
            payApteczkaText.SetActive(false);
        }
    }

    void EndAnim()
    {
        isGambling = false;
        Instantiate(apteczkaPrefab, new Vector3(transform.position.x + SpawnX, transform.position.y + SpawnY, transform.position.z + SpawnZ), transform.rotation);
    }
}