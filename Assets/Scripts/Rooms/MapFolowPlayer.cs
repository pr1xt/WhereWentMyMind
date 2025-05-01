using UnityEngine;

public class MapFolowPlayer : MonoBehaviour
{
    private Transform playerTransform;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found in the scene. Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.localPosition = new Vector3(-playerTransform.position.x, -playerTransform.position.z, 0); ;
        }
    }
}
