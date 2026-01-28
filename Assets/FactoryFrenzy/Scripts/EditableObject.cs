using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EditableObject : MonoBehaviour
{
    [Header("Snap")]
    public bool snapEnabled = false;
    public float snapDistance = 0.3f;
    public LayerMask snapLayer;

    [Header("Rotation")]
    public float rotationStep = 10f;

    [Header("Lock")]
    public bool isLocked = false;

    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (!snapEnabled) return;

        // Evite de snap quand c’est lock (optionnel)
        if (isLocked) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, snapDistance, snapLayer);

        foreach (var hit in hits)
        {
            // Snap sur le point le plus proche (ici: premier trouvé)
            transform.position = hit.transform.position;
            break;
        }
    }

    // ✅ Bouton Snap ON/OFF
    public void ToggleSnap()
    {
        snapEnabled = !snapEnabled;
    }

    // ✅ Bouton Rotation +10
    public void RotatePlus()
    {
        transform.Rotate(0, rotationStep, 0);
    }

    // ✅ Bouton Rotation -10
    public void RotateMinus()
    {
        transform.Rotate(0, -rotationStep, 0);
    }

    // ✅ Bouton Lock/Unlock
    public void ToggleLock()
    {
        isLocked = !isLocked;
        if (grab != null) grab.enabled = !isLocked;
    }
}