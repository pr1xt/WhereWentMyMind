using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class Weapon
{
    public GameObject visualPrefab;
    public GameObject weaponPrefab;
    public GameObject weapon;
    public GameObject iconPrefab;
}

public class InventoryControler : MonoBehaviour
{
    private int currentWeaponIndex = 0;
    public TextMeshProUGUI ammoText;
    [SerializeField] private GameObject weaponStartVisualPrefab;
    [SerializeField] private GameObject weaponStartPrefab;
    [SerializeField] private GameObject weaponStartIconPrefab;
    [SerializeField] private GameObject weaponPickupPrefab;
    [SerializeField] private RectTransform inventoryUI;
    [SerializeField] private GameObject inventoryUIItemBorderPrefab;
    public int maxWeapons = 2;
    private List<Weapon> weapons = new();
    public MapControler mapController;

    public int GetWeaponsCount()
    {
        return weapons.Count;
    }

    private void UpdateWeaponUI()
    {
        foreach (RectTransform child in inventoryUI)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject weaponIcon = Instantiate(inventoryUIItemBorderPrefab, inventoryUI);
            Debug.Log(weapons[i].iconPrefab);
            Instantiate(weapons[i].iconPrefab, weaponIcon.transform);
            if (i == currentWeaponIndex)
            {
                weaponIcon.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                weaponIcon.GetComponent<RawImage>().color = new Color(255, 255, 0, 255);
            }
        }
    }

    public void AddWeapon(GameObject visualPrefab, GameObject weaponPrefab, GameObject iconPrefab)
    {
        if (weapons.Count >= maxWeapons)
        {
            Debug.Log("Inventory is full. Cannot add more weapons.");
            return;
        }

        Weapon newWeapon = new()
        {
            visualPrefab = visualPrefab,
            weaponPrefab = weaponPrefab,
            weapon = Instantiate(weaponPrefab, Camera.main.transform),
            iconPrefab = iconPrefab
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


    public void ReplaceWeapon(GameObject newVisualPrefab, GameObject newWeaponPrefab, GameObject newIconPrefab, Vector3 oldWeaponPosition)
    {
        GameObject gunPickUp = Instantiate(weaponPickupPrefab, oldWeaponPosition, Quaternion.identity);
        gunPickUp.GetComponent<GunPickUp>().gunVisualPrefab = weapons[currentWeaponIndex].visualPrefab;
        gunPickUp.GetComponent<GunPickUp>().gunPrefab = weapons[currentWeaponIndex].weaponPrefab;
        gunPickUp.GetComponent<GunPickUp>().iconPrefab = weapons[currentWeaponIndex].iconPrefab;

        Destroy(weapons[currentWeaponIndex].weapon);

        Weapon newWeapon = new()
        {
            visualPrefab = newVisualPrefab,
            weaponPrefab = newWeaponPrefab,
            weapon = Instantiate(newWeaponPrefab, Camera.main.transform),
            iconPrefab = newIconPrefab
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
        AddWeapon(weaponStartVisualPrefab, weaponStartPrefab, weaponStartIconPrefab);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1)){
            SwitchWeapon(0);
        }
        if(Input.GetKey(KeyCode.Alpha2)){
            SwitchWeapon(1);
        }
    }
}
