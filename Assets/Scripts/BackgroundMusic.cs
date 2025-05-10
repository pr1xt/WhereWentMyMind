using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private void Awake() {
        SceneManager.LoadScene("MainMenu");
    }
}
