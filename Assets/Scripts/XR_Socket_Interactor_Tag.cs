using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// A custom socket interactor that only accepts objects with a specific tag.
/// Works with Unity 6 and XR Interaction Toolkit 3.x.
/// </summary>
public class XR_Socket_Interactor_Tag : XRSocketInteractor
{
    [Header("Filtering Settings")]
    [Tooltip("The tag required for an object to be accepted by this socket.")]
    [SerializeField] private string targetTag = "Untagged";

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.CompareTag(targetTag);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(targetTag);
    }
}
