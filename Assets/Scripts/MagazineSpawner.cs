using UnityEngine;

/// <summary>
/// A utility script that handles spawning magazines at a specific location, 
/// intended for use with VR interaction event listeners (e.g. Buttons).
/// </summary>
public class MagazineSpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    [Tooltip("The magazine prefab to spawn (use the Full version if possible).")]
    [SerializeField] private GameObject magazinePrefab;
    [Tooltip("The location where the new magazines will appear.")]
    [SerializeField] private Transform spawnPoint;

    [Header("Settings")]
    [Tooltip("Small delay between spawns to prevent accidental double-spawns.")]
    [SerializeField] private float spawnCooldown = 0.5f;

    [Header("Audio")]
    [Tooltip("Sound played when a magazine is spawned.")]
    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioSource audioSource;

    private float _nextLaunchTime;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Instantiates a new magazine prefab at the defined spawn point. 
    /// Assign this method to your XR Simple Interactable (Button) Select Entered event.
    /// </summary>
    public void SpawnNewMagazine()
    {
        // Enforce the cooldown to prevent accidental duplicates
        if (Time.time < _nextLaunchTime) return;

        if (magazinePrefab != null && spawnPoint != null)
        {
            _nextLaunchTime = Time.time + spawnCooldown;
            Instantiate(magazinePrefab, spawnPoint.position, spawnPoint.rotation);

            // Play the spawn sound
            if (audioSource != null && spawnSound != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }
        }
        else
        {
            Debug.LogError("MagazineSpawner: Prefab or Spawn Point is not assigned in the Inspector!");
        }
    }
}
