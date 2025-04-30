using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class Weapon
{
    public WeaponData weaponData;
    public GameObject weapon;
}

public class InventoryControler : MonoBehaviour
{
    private int currentWeaponIndex = 0;
    public TextMeshProUGUI ammoText;
    [SerializeField] private WeaponData weaponStartData;
    [SerializeField] private GameObject weaponPickupPrefab;
    [SerializeField] private RectTransform inventoryUI;
    [SerializeField] private GameObject inventoryUIItemBorderPrefab;
    [SerializeField] private GameObject ammoUI;
    public int maxWeapons = 2;
    private List<Weapon> weapons = new();
    public MapControler mapController;
    public KeyCode weapon1Key = KeyCode.Alpha1;
    public KeyCode weapon2Key = KeyCode.Alpha2;

    private KeyCode LoadKey(string keyName, string defaultKey) {
        string keyString = PlayerPrefs.GetString(keyName, defaultKey);
        KeyCode keyCode;
        if (System.Enum.TryParse(keyString, out keyCode))
        {
            return keyCode;
        }
        else
        {
            if (System.Enum.TryParse(defaultKey, out keyCode))
            {
                return keyCode;
            }
        }

        return KeyCode.Alpha1; // There was an error without it, it has to return something in case it can't parse both the saved and default key
    }

    public int GetWeaponsCount()
    {
        return weapons.Count;
    }

    private void UpdateWeaponUI()
    {
        ammoUI.GetComponent<RawImage>().texture = weapons[currentWeaponIndex].weaponData.ammoUiImage;
        foreach (RectTransform child in inventoryUI)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject weaponIcon = Instantiate(inventoryUIItemBorderPrefab, inventoryUI);
            Instantiate(weapons[i].weaponData.iconPrefab, weaponIcon.transform);
            if (i == currentWeaponIndex)
            {
                weaponIcon.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                weaponIcon.GetComponent<RawImage>().color = new Color(120,120,120,0.8f);
            }
        }
    }

    public void AddWeapon(WeaponData weaponData)
    {
        if (weapons.Count >= maxWeapons)
        {
            Debug.Log("Inventory is full. Cannot add more weapons.");
            return;
        }

        Weapon newWeapon = new()
        {
            weaponData = weaponData,
            weapon = Instantiate(weaponData.weaponPrefab, Camera.main.transform),
        };

        IWeaponSystem weaponSystem = newWeapon.weapon.GetComponent<IWeaponSystem>();
        if (weaponSystem != null)
        {
            weaponSystem.Initialize(
                Camera.main,
                Camera.main.GetComponentInChildren<ParticleSystem>(),
                ammoText
            );
        }
        else
        {
            Debug.LogWarning("Weapon does not implement IWeaponSystem");
        }

        weapons.Add(newWeapon);

        if (weapons.Count > 1)
            weapons[currentWeaponIndex].weapon.SetActive(false);

        currentWeaponIndex = weapons.Count - 1;
        weapons[currentWeaponIndex].weapon.SetActive(true);
        UpdateWeaponUI();
    }


    public void ReplaceWeapon(WeaponData weaponData, Vector3 oldWeaponPosition)
    {
        GameObject gunPickUp = Instantiate(weaponPickupPrefab, oldWeaponPosition, Quaternion.identity);
        gunPickUp.GetComponent<GunPickUp>().weaponData = weapons[currentWeaponIndex].weaponData;

        Destroy(weapons[currentWeaponIndex].weapon);

        Weapon newWeapon = new()
        {
            weaponData = weaponData,
            weapon = Instantiate(weaponData.weaponPrefab, Camera.main.transform),
        };

        IWeaponSystem weaponSystem = newWeapon.weapon.GetComponent<IWeaponSystem>();
        if (weaponSystem != null)
        {
            weaponSystem.Initialize(
                Camera.main,
                Camera.main.GetComponentInChildren<ParticleSystem>(),
                ammoText
            );
        }
        else
        {
            Debug.LogWarning("New weapon does not implement IWeaponSystem");
        }

        weapons[currentWeaponIndex] = newWeapon;

        
        mapController.UpdateIconsOnMap();
        UpdateWeaponUI();
    }

    public void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
        {
            Debug.Log("Invalid weapon index. Cannot switch to weapon.");
            return;
        }
        if(weapons[currentWeaponIndex].weapon.GetComponent<GunSystem>() != null && weapons[currentWeaponIndex].weapon.GetComponent<GunSystem>().reloading){
            Debug.Log("Cannot switch weapons while reloading.");
            return;
        }
        if(weapons[currentWeaponIndex].weapon.GetComponent<GamblingGunHolder>() != null && weapons[currentWeaponIndex].weapon.GetComponent<GamblingGunHolder>().reloading){
            Debug.Log("Cannot switch weapons while reloading.");
            return;
        }
        weapons[currentWeaponIndex].weapon.SetActive(false);
        currentWeaponIndex = weaponIndex;
        weapons[currentWeaponIndex].weapon.SetActive(true);
        UpdateWeaponUI();
    }
    

    void Start()
    {
        AddWeapon(weaponStartData);
    }

    void Update()
    {
        weapon1Key = LoadKey("Weapon1Key", "Alpha1");
        weapon2Key = LoadKey("Weapon2Key", "Alpha2");
        if(Input.GetKey(weapon1Key)){
            SwitchWeapon(0);
        }
        if(Input.GetKey(weapon2Key)){
            SwitchWeapon(1);
        }
    }
}
