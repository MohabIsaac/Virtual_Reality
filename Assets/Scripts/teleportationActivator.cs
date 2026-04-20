using UnityEngine;
using UnityEngine.InputSystem;


public class teleportationActivator : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor teleportInteractor;
    public InputActionProperty teleportActivationAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        teleportInteractor.gameObject.SetActive(false);
        teleportActivationAction.action.performed += actionPerformed;
    }

    private void actionPerformed(InputAction.CallbackContext context)
    {
        teleportInteractor.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (teleportActivationAction.action.WasReleasedThisFrame())
        {
            teleportInteractor.gameObject.SetActive(false);
        }

    }
}
