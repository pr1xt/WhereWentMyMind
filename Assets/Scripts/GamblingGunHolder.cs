using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public class GamblingGunHolder : MonoBehaviour, IWeaponSystem
{
    public Animator gunAnimator;
    //Gun stats
    public float timeBetweenShooting, reloadTime;
    public int magazineSize;
    public bool allowButtonHold;
    int bulletsLeft;
    public CameraShake cameraShake;
    public float shakeDuration;
    public float shakeIntensity ;
    public ParticleSystem lemonParticle;
    public ParticleSystem grapeParticle;  
    public ParticleSystem nfruitParticle;  
    public ParticleSystem currentParticle;
    public Transform currentWeapon;
    public KeyCode reloadKey = KeyCode.R;

    //bools 
    public bool shooting, readyToShoot, reloading; 


    //Reference
    public Camera fpsCam;
    public TMP_Text text; 

    public AudioSource shootSound;
    public AudioSource reloadSound;

    private enum RollOptions
    {
        lemon,
        grape,
        nfruit,
        seven,
    }
    RollOptions PreRolled = RollOptions.lemon;
    RollOptions Rolled = RollOptions.lemon;
    public void Initialize(Camera camera, ParticleSystem muzzleFlash, TMP_Text ammoText)
    {
        fpsCam = camera;
        cameraShake = camera.GetComponent<CameraShake>();
        currentParticle = muzzleFlash;
        text = ammoText;
        
    }
    void Roll() {
        PreRolled = Rolled;
        Rolled = (RollOptions)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(RollOptions)).Length);
    }

    private void Reload()
    {
        reloading = true;
        reloadSound.volume = PlayerPrefs.GetFloat("Volume", 1f);
        reloadSound.Play();
        gunAnimator.SetBool("Reload", true);
        Roll();
        Debug.Log("Reload Gambling "+PreRolled+" to " +Rolled);
    
        if(Rolled == RollOptions.lemon && PreRolled == RollOptions.lemon){
            gunAnimator.Play("Reload Lemon to Lemon");
        }else if(Rolled == RollOptions.grape && PreRolled == RollOptions.lemon){
            gunAnimator.Play("Reload Lemon to Grape");
        }else if(Rolled == RollOptions.nfruit && PreRolled == RollOptions.lemon){
            gunAnimator.Play("Reload Lemon to Watermelon");
        }else if(Rolled == RollOptions.seven && PreRolled == RollOptions.lemon){
            gunAnimator.Play("Reload Lemon to Seven");
        }else if(Rolled == RollOptions.lemon && PreRolled == RollOptions.grape){
            gunAnimator.Play("Reload Grape to Lemon");
        }else if(Rolled == RollOptions.grape && PreRolled == RollOptions.grape){
            gunAnimator.Play("Reload Grape to Grape");
        }else if(Rolled == RollOptions.nfruit && PreRolled == RollOptions.grape){
            gunAnimator.Play("Reload Grape to Watermelon");
        }else if(Rolled == RollOptions.seven && PreRolled == RollOptions.grape){
            gunAnimator.Play("Reload Grape to Seven");
        }else if(Rolled == RollOptions.lemon && PreRolled == RollOptions.nfruit){
            gunAnimator.Play("Reload Watermelon to Lemon");
        }else if(Rolled == RollOptions.grape && PreRolled == RollOptions.nfruit){
            gunAnimator.Play("Reload Watermelon to Grape");
        }else if(Rolled == RollOptions.nfruit && PreRolled == RollOptions.nfruit){
            gunAnimator.Play("Reload Watermelon to Watermelon");
        }else if(Rolled == RollOptions.seven && PreRolled == RollOptions.nfruit){
            gunAnimator.Play("Reload Watermelon to Seven");
        }else if(Rolled == RollOptions.lemon && PreRolled == RollOptions.seven){
            gunAnimator.Play("Reload Seven to Lemon");
        }else if(Rolled == RollOptions.grape && PreRolled == RollOptions.seven){
            gunAnimator.Play("Reload Seven to Grape");
        }else if(Rolled == RollOptions.nfruit && PreRolled == RollOptions.seven){
            gunAnimator.Play("Reload Seven to Watermelon");
        }else if(Rolled == RollOptions.seven && PreRolled == RollOptions.seven){
            gunAnimator.Play("Reload Seven to Seven");
        }

        
        Invoke("ReloadFinished", reloadTime);
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
        MyInput();
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        reloadKey = LoadKey(); // Load the key from PlayerPrefs
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);


        if (Input.GetKeyDown(reloadKey) && bulletsLeft <= magazineSize && !reloading) Reload();


        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && Time.timeScale != 0) {
            Debug.Log(" readyToShoot: " +readyToShoot+" shooting: " +shooting+" reloading: " +reloading+" bulletsLeft: " +bulletsLeft);
            Shoot();
        }
    }
    
    private void Shoot()
    {
        readyToShoot = false;
        shootSound.volume = PlayerPrefs.GetFloat("Volume", 1f);
        shootSound.Play();
        Debug.Log(" Nigga shot: " +Rolled);

        if(Rolled == RollOptions.lemon){
            gunAnimator.Play("Shot Fired Lemon");
            currentParticle = lemonParticle;
        }else if(Rolled == RollOptions.grape){
            gunAnimator.Play("Shot Fired Grape");
            currentParticle = grapeParticle;
        }else if(Rolled == RollOptions.nfruit){
            gunAnimator.Play("Shot Fired Watermelon");
            currentParticle = nfruitParticle;
        }else if(Rolled == RollOptions.seven){
            gunAnimator.Play("Shot Fired Seven");
            nfruitParticle.Play();
            grapeParticle.Play();
            lemonParticle.Play();
        }
        
        if (currentParticle != null)
        {
            
            currentParticle.Stop();
            currentParticle.Play();
        }
        Invoke("ResetAnim", 0.5f);
        
        

 
        if (cameraShake != null)
        {
            cameraShake.ShakeCamera(shakeDuration, shakeIntensity);
        }



        bulletsLeft--;


        Invoke("ResetShot", timeBetweenShooting);
    }
    private void ResetAnim()
    {
        if(Rolled == RollOptions.lemon){
            gunAnimator.Play("Idle");
        }else if(Rolled == RollOptions.grape){
            gunAnimator.Play("Grape Idle");
        }else if(Rolled == RollOptions.nfruit){
            gunAnimator.Play("Watermelon Idle");
        }else if(Rolled == RollOptions.seven){
            gunAnimator.Play("Seven Idle");
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
  
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        if(Rolled == RollOptions.lemon){
            gunAnimator.Play("Idle");
        }else if(Rolled == RollOptions.grape){
            gunAnimator.Play("Grape Idle");
        }else if(Rolled == RollOptions.nfruit){
            gunAnimator.Play("Watermelon Idle");
        }else if(Rolled == RollOptions.seven){
            gunAnimator.Play("Seven Idle");
            currentParticle = null;
        }
        readyToShoot = true;
        gunAnimator.SetBool("Reload", false);
        gunAnimator.SetBool("Ready", true);
    }
}
