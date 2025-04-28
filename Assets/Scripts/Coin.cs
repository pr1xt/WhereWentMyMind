using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject pickupSoundPrefab;

    void SpawnPickupSound()
    {
        GameObject sound = Instantiate(pickupSoundPrefab, transform.position, Quaternion.identity);
        Destroy(sound, 2f); 
    }

    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {     
            if (other.TryGetComponent<PlayerControler>(out var playerCoins))
            {
                playerCoins.coins += 1;
                SpawnPickupSound();
                Destroy(gameObject);
            }
        }
    }
    
}
