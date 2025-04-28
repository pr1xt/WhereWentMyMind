using UnityEngine;

public class HelthUp : MonoBehaviour
{
    [SerializeField] private GameObject pickupSoundPrefab;

    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    void SpawnPickupSound()
    {
        GameObject sound = Instantiate(pickupSoundPrefab, transform.position, Quaternion.identity);
        Destroy(sound, 2f); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerControler>(out var playerHealth))
            {
                if (playerHealth.Health < playerHealth.MaxHealth)
                {
                    playerHealth.Heal(25);
                    SpawnPickupSound();
                    Destroy(gameObject);
                }
            }
        }
    }
}
