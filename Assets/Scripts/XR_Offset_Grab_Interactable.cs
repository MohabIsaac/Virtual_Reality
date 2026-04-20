using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Custom grab interactable that supports dynamic offset grabbing when using Direct Interactors.
/// Works with Unity 6 and XR Interaction Toolkit 3.x.
/// </summary>
public class XR_Offset_Grab_Interactable : XRGrabInteractable
{
    private Vector3 initialAttachLocalPos;
    private Quaternion initialAttachLocalRot;

    protected override void Awake()
    {
        base.Awake();

        // Create or find attach point
        if (!attachTransform)
        {
            GameObject grabPivot = new GameObject("Grab Pivot");
            grabPivot.transform.SetParent(transform, false);
            attachTransform = grabPivot.transform;
        }

        initialAttachLocalPos = attachTransform.localPosition;
        initialAttachLocalRot = attachTransform.localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // If grabbed by a Direct Interactor, move the attach point to the interactor
        if (args.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = args.interactorObject.transform.position;
            attachTransform.rotation = args.interactorObject.transform.rotation;
        }
        else
        {
            // Reset to original local offset for Ray Interactors or Sockets
            attachTransform.localPosition = initialAttachLocalPos;
            attachTransform.localRotation = initialAttachLocalRot;
        }

        base.OnSelectEntered(args);
    }
}