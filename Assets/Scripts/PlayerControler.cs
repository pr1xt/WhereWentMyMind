using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerControler : MonoBehaviour
{
    private readonly int healthBarMaxOfset = 381;
    private int health = 100;
    private int maxHealth = 100;

    public int coins = 0;
    // [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private Volume postEffectVolume;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(0, health);
        hitSound.volume = PlayerPrefs.GetFloat("Volume", 1f);
        hitSound.Play();
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth); // Prevent overhealing
    }

    private void Die()
    {
        Time.timeScale = 0;
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject != deathScreen)
            {
                child.gameObject.SetActive(false);
            }
        }
        deathScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // healthText.text = $"Health: {health}";
        healthBar.transform.localPosition = new Vector3(healthBarMaxOfset - healthBarMaxOfset * (health / (float)maxHealth), 0, 0);
        coinsText.text = coins.ToString();
        postEffectVolume.weight = 1 - (health / (float)maxHealth);
        if(health <= 0)
        {
            Die();
        }
        
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
}
