using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunPickUp : MonoBehaviour
{
    public GameObject gunVisualPrefab; 
    public GameObject gunPrefab;
    public GameObject iconPrefab;
    private GameObject gunVisual; 
    private InventoryControler inventoryControler; 
    public KeyCode interactKey = KeyCode.E;
    [SerializeField] private GameObject pickupText; 
    [SerializeField] private GameObject pickUpSoundPrefab;

    void Start()
    {
        gunVisual = Instantiate(gunVisualPrefab, transform);
        gunVisual.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
        inventoryControler = GameObject.FindWithTag("Player").GetComponent<InventoryControler>();
    }

    void animateGun()
    {
        gunVisual.transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * 2) / 4, transform.position.z);
        gunVisual.transform.Rotate(new Vector3(0, 1, 0), 40f * Time.deltaTime);
    }

    void SpawnPickUpSound()
    {
        GameObject sound = Instantiate(pickUpSoundPrefab, transform.position, Quaternion.identity);
        sound.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume", 1f);
        sound.GetComponent<AudioSource>().Play();
        Destroy(sound, 2f); 
    }

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
        animateGun();
        if (Vector3.Distance(gunVisual.transform.position, Camera.main.transform.position) < 3f)
        {
            pickupText.GetComponent<TextMeshProUGUI>().text = "Press " + interactKey.ToString() + " to pick up the gun";
            pickupText.SetActive(true);
            // Check for input to pick up the gun
            if (Input.GetKeyDown(interactKey))
            {
                if(inventoryControler.GetWeaponsCount() >= inventoryControler.maxWeapons)
                {
                    inventoryControler.ReplaceWeapon(gunVisualPrefab, gunPrefab, iconPrefab, transform.position);
                }
                else
                {
                    inventoryControler.AddWeapon(gunVisualPrefab, gunPrefab, iconPrefab);
                }
                SpawnPickUpSound();
                Destroy(gameObject); 
            }
        } else {
            pickupText.SetActive(false);
        }
    }
}
