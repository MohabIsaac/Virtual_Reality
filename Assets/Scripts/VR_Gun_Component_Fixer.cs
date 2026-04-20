using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Automatically adjusts Gun, Slider, and Magazine component values in the hierarchy 
/// for maximum VR stability (Zero-Lag, No-Wiggle) in Unity 6 and XRI 3.x.
/// Attach this to your main Gun object and right-click the component in the Inspector to "Fix Gun Components".
/// </summary>
[ExecuteInEditMode]
public class VR_Gun_Component_Fixer : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Recommended mass for small parts to prevent physics jitter.")]
    [SerializeField] private float minimumPartMass = 0.1f;

    [ContextMenu("Fix Gun Components")]
    public void FixComponents()
    {
        FixRigidbodies();
        FixJoints();
        FixInteractables();
        FixColliders();
        
        Debug.Log("<color=green><b>VR Gun Fixer:</b> All child components optimized for stability!</color>");
    }

    private void FixRigidbodies()
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>(true);
        foreach (var rb in bodies)
        {
            // Interpolate is critical for smooth VR movement
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            
            // Speculative is best for fast-moving mechanical components
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            
            // Extreme mass ratios (e.g. 0.001 vs 1.0) cause joint wiggling
            if (rb.mass < minimumPartMass)
            {
                rb.mass = minimumPartMass;
                Debug.Log($"Updated mass on {rb.name} to {minimumPartMass} for stability.");
            }
        }
    }

    private void FixJoints()
    {
        ConfigurableJoint[] joints = GetComponentsInChildren<ConfigurableJoint>(true);
        foreach (var joint in joints)
        {
            // Projection is the "hard-clamp" that prevents the slider from wiggling past its limit
            joint.projectionMode = JointProjectionMode.PositionAndRotation;
            joint.projectionDistance = 0.001f;
            
            // Preprocessing often causes high-frequency jitter at limits
            joint.enablePreprocessing = false;
        }
    }

    private void FixInteractables()
    {
        XRGrabInteractable[] grabs = GetComponentsInChildren<XRGrabInteractable>(true);
        foreach (var grab in grabs)
        {
            // Kinematic movement type on a Non-Kinematic Rigidbody is the "Zero-Lag" sweet spot in XRI
            grab.movementType = XRBaseInteractable.MovementType.Kinematic;
            
            // Ensure the grab doesn't have accidental smoothing that adds lag
            // In XRI 3.x, these are found in the component settings
            
            Debug.Log($"Optimized XRGrabInteractable on {grab.name}. Movement Type set to Kinematic.");
        }
    }

    private void FixColliders()
    {
        MeshCollider[] meshColliders = GetComponentsInChildren<MeshCollider>(true);
        int fixedCount = 0;
        foreach (var mc in meshColliders)
        {
            // XRI ClosestPoint throws an error if MeshColliders are not Convex
            if (!mc.convex)
            {
                mc.convex = true;
                fixedCount++;
            }
        }
        
        if (fixedCount > 0)
        {
            Debug.Log($"<color=yellow>Fixed {fixedCount} Mesh Colliders by enabling 'Convex'.</color>");
        }
    }
}
