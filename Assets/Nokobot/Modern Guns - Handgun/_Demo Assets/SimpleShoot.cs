using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;
    [Tooltip("Minimum time between shots")][SerializeField] private float fireRate = 0.1f;
    private float nextFireTime;
    private bool hasSlide = false;

    public AudioSource source;
    public AudioClip fireClip;
    public AudioClip reload;
    public AudioClip dryFireClip;
    public Magazine magazine;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor socketInteractor;

    public void AddMagazine(SelectEnterEventArgs args)
    {
        magazine = args.interactableObject.transform.GetComponent<Magazine>();
        source.PlayOneShot(reload);
        hasSlide = false;
    }

    public void RemoveMagazine(SelectExitEventArgs args)
    {
        magazine = null;
        source.PlayOneShot(reload);
    }

    public void Slide()
    {

        hasSlide = true;
        source.PlayOneShot(reload);
    }

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        socketInteractor.selectEntered.AddListener(AddMagazine);
        socketInteractor.selectExited.AddListener(RemoveMagazine);
    }

    void Update()
    {
        // // //If you want a different input, change it here
        // if (Input.GetButtonDown("Fire1"))
        // {
        //     //Calls animation on the gun that has the relevant animation events that will fire
        //     gunAnimator.SetTrigger("Fire");
        // }
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        // Double check bullet count at the exact moment of firing
        if (magazine == null || magazine.numberOfBullets <= 0)
            return;

        // Consume the bullet here so it matches the visual/audio event
        magazine.numberOfBullets--;

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        source.PlayOneShot(fireClip);
        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

    public void PullTheTrigger()
    {
        // Prevent double-firing within the fire rate window
        if (Time.time < nextFireTime)
            return;

        if (magazine != null && magazine.numberOfBullets > 0)
        {
            nextFireTime = Time.time + fireRate;
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            source.PlayOneShot(dryFireClip);
        }
    }

}
