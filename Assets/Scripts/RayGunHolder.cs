using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GunSystem : MonoBehaviour, IWeaponSystem
{
    public Animator gunAnimator;
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public CameraShake cameraShake; // Assign this in the inspector
    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.5f;
    public ParticleSystem MuzzleFlash;  
    public float Particledelay = 0.1f;
    public Transform currentWeapon;
    public string name;

    //bools 
    public bool shooting, readyToShoot, reloading;


    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public TMP_Text text;

    public AudioSource shootSound;
    public KeyCode reloadKey = KeyCode.R;
    public AudioSource reloadSound;
    public AudioSource OtherReloadSound1;
    public AudioSource OtherReloadSound2;
    public AudioSource OtherReloadSound3;
    private  List<AudioSource> reloadSounds = new List<AudioSource>();
    private int AudioIndex;

    public void Initialize(Camera camera, ParticleSystem muzzleFlash, TMP_Text ammoText)
    {
        fpsCam = camera;
        cameraShake = camera.GetComponent<CameraShake>();
        MuzzleFlash = muzzleFlash;
        text = ammoText;
        if(name =="Skull"){
            reloadSounds.Add(reloadSound);
            reloadSounds.Add(OtherReloadSound1);
            reloadSounds.Add(OtherReloadSound2);
            reloadSounds.Add(OtherReloadSound3);
        }
    }
    private void Awake()
    {
        text = (TextMeshProUGUI)GetComponent<TMP_Text>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private KeyCode LoadKey() {
        string keyString = PlayerPrefs.GetString("ReloadKey", "R");
        KeyCode keyCode;
        if (System.Enum.TryParse(keyString, out keyCode))
        {
            return keyCode;
        }
        else
        {
            return KeyCode.R; // Default to R if parsing fails
        }
    }

    private void Update()
    {
        reloadKey = LoadKey(); // Load the key from PlayerPrefs
        MyInput();

        //SetText
        if(text == null){
        text = (TextMeshProUGUI)GetComponent<TMP_Text>();
        }
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading) Reload();


        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && Time.timeScale != 0) {
            gunAnimator.SetBool("Shooting", true);
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        else gunAnimator.SetBool("Shooting", false);
    }
    
    void UpdateMuzzleFlash()
    {   
        GameObject activeWeapon = FindActiveWeapon();

        if (activeWeapon != null)
        {   
            
            Transform particlesTransform;
            if(name == "toster"){
                particlesTransform = activeWeapon.transform.Find("TostParticles");
                MuzzleFlash = particlesTransform.GetComponent<ParticleSystem>();
            }else if(name == "PaintRoller"){
                particlesTransform = activeWeapon.transform.Find("PaintParticles");
                MuzzleFlash = particlesTransform.GetComponent<ParticleSystem>();
            }else if(name == "Drill"){
                particlesTransform = activeWeapon.transform.Find("DrillParticles");
                MuzzleFlash = particlesTransform.GetComponent<ParticleSystem>();
            }else if(name == "Phone"){
                particlesTransform = activeWeapon.transform.Find("PhoneParticles");
                MuzzleFlash = particlesTransform.GetComponent<ParticleSystem>();
            }else if(name == "Skull"){
                particlesTransform = activeWeapon.transform.Find("SkullParticles");
                MuzzleFlash = particlesTransform.GetComponent<ParticleSystem>();
            }
            
        }
    } 
    private GameObject FindActiveWeapon()
    {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

        foreach (GameObject weapon in weapons)
        {
            if (weapon.activeSelf) 
            {
                return weapon;
            }
        }

        return null; 
    }  
    private void Shoot()
    {
        readyToShoot = false;
        
        shootSound.volume = PlayerPrefs.GetFloat("Volume", 1f);
        shootSound.Play();
        UpdateMuzzleFlash();
        if (MuzzleFlash != null)
        {
            
            MuzzleFlash.Stop();
            MuzzleFlash.Play();
        }
 
        if (cameraShake != null)
        {
            cameraShake.ShakeCamera(shakeDuration, shakeIntensity);
        }

        bulletsLeft--;
        bulletsShot--;


        Invoke("ResetShot", timeBetweenShooting);


        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        if(name =="Skull"){
            AudioIndex = Random.Range(0, 4);
            reloadSounds[AudioIndex].volume = PlayerPrefs.GetFloat("Volume", 1f);
            reloadSounds[AudioIndex].Play();
            Debug.Log(AudioIndex+"; "+reloadSounds[AudioIndex]);
            
        } else {
            reloadSound.volume = PlayerPrefs.GetFloat("Volume", 1f);
            reloadSound.Play();
        }
        Invoke("ReloadFinished", reloadTime);
        gunAnimator.SetBool("Reloading", true);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        gunAnimator.SetBool("Reloading", false);
    }
    private void OnEnable()
    {
        gunAnimator.SetBool("Shooting", false);
        gunAnimator.SetBool("Reloading", false);
        gunAnimator.Play("Idle", -1, 0f); 
    }
}
