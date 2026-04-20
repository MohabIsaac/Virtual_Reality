using System.Collections;
using UnityEngine;

/// <summary>
/// A target dummy that moves in a 3-staged sequence: Descend, Move Away, and Pop Up.
/// </summary>
public class TargetDummy : MonoBehaviour
{
    [Header("Movement Sequence")]
    [Tooltip("How far the dummy descends (down) at spawn.")]
    [SerializeField] private float descendDistance = 0.5f;
    [Tooltip("How far the dummy moves away (forward) down the lane.")]
    [SerializeField] private float travelDistance = 15f;
    [Tooltip("How far the dummy transitions up (up) at the end.")]
    [SerializeField] private float popUpDistance = 1.0f;
    [Tooltip("Speed of all movement stages.")]
    [SerializeField] private float moveSpeed = 3.0f;

    [Header("Interaction")]
    [Tooltip("Points awarded when hit.")]
    [SerializeField] private int scorePoints = 10;
    [Tooltip("Tag to check on the collision object.")]
    [SerializeField] private string bulletTag = "Bullet";

    [Header("Audio")]
    [Tooltip("Sound played when the target is hit.")]
    [SerializeField] private AudioClip hitSound;

    private bool _isHit = false;

    private void Start()
    {
        // Start the staged movement sequence
        StartCoroutine(MovementSequence());
    }

    private IEnumerator MovementSequence()
    {
        // 1. Descend
        Vector3 descendTarget = transform.position + Vector3.down * descendDistance;
        yield return StartCoroutine(MoveTowardsTarget(descendTarget));

        // 2. Move Away (Linear forward)
        Vector3 awayTarget = transform.position + transform.forward * travelDistance;
        yield return StartCoroutine(MoveTowardsTarget(awayTarget));

        // 3. Pop Up
        Vector3 finalTarget = transform.position + Vector3.up * popUpDistance;
        yield return StartCoroutine(MoveTowardsTarget(finalTarget));
    }

    private IEnumerator MoveTowardsTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isHit) return;

        // Detect hit from bullet (check tag or name)
        if (collision.gameObject.CompareTag(bulletTag) || collision.gameObject.name.ToLower().Contains("bullet"))
        {
            _isHit = true;
            OnHit();
        }
    }

    private void OnHit()
    {
        // Add score to manager
        if (TargetManager.Instance != null)
        {
            TargetManager.Instance.AddScore(scorePoints);
        }

        // Play hit sound (Spatialized at target position)
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        // Destroy target
        Destroy(gameObject);
    }
}
