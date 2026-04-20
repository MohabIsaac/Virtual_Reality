using UnityEngine;
using TMPro;

/// <summary>
/// Monitors a gun and its magazine to display the current ammo count in world space.
/// </summary>
public class AmmoDisplay : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The handgun script to monitor.")]
    [SerializeField] private SimpleShoot gun;
    
    [Tooltip("Universal component for both World-Space and Canvas-based TextMeshPro.")]
    [SerializeField] private TMP_Text ammoText;

    private void Update()
    {
        if (gun == null || ammoText == null) return;

        UpdateAmmoCount();
    }

    private void UpdateAmmoCount()
    {
        if (gun.magazine != null)
        {
            int currentAmmo = gun.magazine.numberOfBullets;
            
            // Update the text
            ammoText.text = currentAmmo.ToString();
            
            // Helpful color coding for quick reference
            if (currentAmmo == 0) ammoText.color = Color.red;
            else if (currentAmmo <= 2) ammoText.color = Color.yellow;
            else ammoText.color = Color.white;
        }
        else
        {
            // Display empty state if no magazine is socketed
            ammoText.text = "--";
            ammoText.color = Color.gray;
        }
    }
}
