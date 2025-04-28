using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    void Update()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            transform.LookAt(player.position);
        }
    }
}
