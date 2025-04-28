using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    private string[] screneNames = { "GamePaintRoller", "GameDrill", "GameSkull", "GameRecipePrinter", "GameToster", "GamePhone", "GameGamblingGun" };
    [SerializeField] private GameManager gameManager;

    public void SelectPaintRoller()
    {
        gameManager.sceneToLoad = "GamePaintRoller"; 
        gameManager.StartGame();
    }

    public void SelectDrill()
    {
        gameManager.sceneToLoad = "GameDrill"; 
        gameManager.StartGame();
    }

    public void SelectSkull()
    {
        gameManager.sceneToLoad = "GameSkull"; 
        gameManager.StartGame();
    }
    public void SelectRecipePrinter()
    {
        gameManager.sceneToLoad = "GameRecipePrinter"; 
        gameManager.StartGame();
    }
    public void SelectToster()
    {
        gameManager.sceneToLoad = "GameToster"; 
        gameManager.StartGame();
    }
    public void SelectPhone()
    {
        gameManager.sceneToLoad = "GamePhone"; 
        gameManager.StartGame();
    }
    public void SelectGamblingGun()
    {
        gameManager.sceneToLoad = "GameGamblingGun"; 
        gameManager.StartGame();
    }
    public void SelectRandom()
    {
        int randomIndex = Random.Range(0, screneNames.Length);
        gameManager.sceneToLoad = screneNames[randomIndex];
        gameManager.StartGame();
    }
}
