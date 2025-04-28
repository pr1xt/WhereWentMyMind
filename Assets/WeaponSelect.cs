using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    private string[] screneNames = { "GamePaintRoller", "GameDrill", "GameSkull", "GameRecipePrinter", "GameToster", "GamePhone", "GameGamblingGun" };
    [SerializeField] private GameManager gameManager;

    public void SelectPaintRoller()
    {
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }

    public void SelectDrill()
    {
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }

    public void SelectSkull()
    {
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectRecipePrinter()
    {
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectToster()
    {
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectPhone()
    {
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectGamblingGun()
    {
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectRandom()
    {
        int randomIndex = Random.Range(0, screneNames.Length);
        // gameManager.sceneToLoad = screneNames[randomIndex];
        gameManager.sceneToLoad = "Game";
        gameManager.StartGame();
    }
}
