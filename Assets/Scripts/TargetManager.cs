using UnityEngine;
using TMPro;

/// <summary>
/// Manages spawning of targets and tracking of the player's score.
/// </summary>
public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("The TextMeshPro element used to display the player's score.")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Spawning Settings")]
    [Tooltip("The prefab to spawn as a target dummy.")]
    [SerializeField] private GameObject targetPrefab;
    [Tooltip("The location where targets will spawn.")]
    [SerializeField] private Transform spawnPoint;
    [Tooltip("How often a new target should be spawned.")]
    [SerializeField] private float spawnInterval = 5f;

    private int _score = 0;
    private float _nextSpawnTime;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
        _nextSpawnTime = Time.time + 1f; // Initial delay
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            SpawnTarget();
            _nextSpawnTime = Time.time + spawnInterval;
        }
    }

    /// <summary>
    /// Adds points to the player's score and updates the UI.
    /// </summary>
    public void AddScore(int points)
    {
        _score += points;
        UpdateScoreUI();
    }

    private void SpawnTarget()
    {
        if (targetPrefab != null && spawnPoint != null)
        {
            Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {_score}";
        }
    }
}
