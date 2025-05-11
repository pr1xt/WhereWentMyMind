using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public static WeaponSelector Instance;
    public WeaponData weaponStartData;
    
    private void Awake() {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
