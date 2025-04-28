using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    public int health;
    private List<Renderer> enemyRenderers = new List<Renderer>();
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    public GameObject Coin;
    public GameObject HealthUp;
    public AudioSource hitSound;
    public GameObject deathSound;
    public EnemyMovement RudyMovement;
    public BugController NormalMovement;

    private void Start()
    {
        // Find all Renderer components in the enemy's hierarchy
        enemyRenderers.AddRange(GetComponentsInChildren<Renderer>());
        
        // Store the original color of each Renderer
        foreach (var renderer in enemyRenderers)
        {
            originalColors[renderer] = renderer.material.color;
        }
    }

    private void Update()
    {
        if(transform.localPosition.x < -15.5f || transform.localPosition.x > 15.5f || transform.localPosition.z < -15.5f || transform.localPosition.z > 15.5f){
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        // Change the color of all Renderers to red
        foreach (var renderer in enemyRenderers)
        {
            renderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        // Revert the color of all Renderers back to their original color
        foreach (var renderer in enemyRenderers)
        {
            renderer.material.color = originalColors[renderer];
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            KillEnemy();
        }
        else
        {
            StartCoroutine(FlashRed());
            hitSound.volume = PlayerPrefs.GetFloat("Volume", 1f);
            hitSound.Play();
        }
    }

    void KillEnemy()
    {
        GameObject deathSoundObject = Instantiate(deathSound, transform.position, Quaternion.identity);
        deathSoundObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume", 1f);
        deathSoundObject.GetComponent<AudioSource>().Play();
        Destroy(deathSoundObject, 3f);
        Destroy(gameObject);
        if (Random.Range(0, 100) < 20){
            Instantiate(Random.Range(0, 2) == 0 ? Coin : HealthUp, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        }
    }
    public void TurnOnEnemy(){
        if(RudyMovement != null) RudyMovement.detectionRange  = 20f;
        if(NormalMovement != null) NormalMovement.detectionRange  = 20f;
    }
        

}