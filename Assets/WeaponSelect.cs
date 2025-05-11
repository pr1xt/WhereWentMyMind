using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    [SerializeField] private WeaponData[] weapons;

    [SerializeField] private GameManager gameManager;

    public void SelectPaintRoller()
    {
        WeaponSelector.Instance.weaponStartData = weapons[0];
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }

    public void SelectDrill()
    {
        WeaponSelector.Instance.weaponStartData = weapons[1];
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }

    public void SelectSkull()
    {
        WeaponSelector.Instance.weaponStartData = weapons[2];
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectRecipePrinter()
    {
        WeaponSelector.Instance.weaponStartData = weapons[3];
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectToster()
    {
        WeaponSelector.Instance.weaponStartData = weapons[4];
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectPhone()
    {
        WeaponSelector.Instance.weaponStartData = weapons[5];
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectGamblingGun()
    {
        WeaponSelector.Instance.weaponStartData = weapons[6];
        gameManager.sceneToLoad = "Game"; 
        gameManager.StartGame();
    }
    public void SelectRandom()
    {
        int randomIndex = Random.Range(0, weapons.Length);
        WeaponSelector.Instance.weaponStartData = weapons[randomIndex];
        gameManager.sceneToLoad = "Game";
        gameManager.StartGame();
    }
}
