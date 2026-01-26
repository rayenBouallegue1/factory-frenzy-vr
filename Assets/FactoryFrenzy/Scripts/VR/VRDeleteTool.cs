using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRDeleteTool : MonoBehaviour
{
    [Header("Ray")]
    [SerializeField] private XRRayInteractor ray;

    [Header("Input")]
    [SerializeField] private InputActionReference deleteAction; // ex: Trigger

    [Header("Mode")]
    public bool deleteMode = false;

    [Header("Highlight (optional)")]
    [SerializeField] private Material highlightMaterial;

    private Renderer lastRenderer;
    private Material[] lastMats;

    private void OnEnable()
    {
        if (deleteAction != null) deleteAction.action.performed += OnDeletePressed;
        if (deleteAction != null) deleteAction.action.Enable();
    }

    private void OnDisable()
    {
        if (deleteAction != null) deleteAction.action.performed -= OnDeletePressed;
        ClearHighlight();
    }

    private void Update()
    {
        if (!deleteMode)
        {
            ClearHighlight();
            return;
        }

        // Raycast XR
        if (ray != null && ray.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var deletable = hit.collider.GetComponentInParent<DeletableObject>();
            if (deletable != null)
                Highlight(deletable.gameObject);
            else
                ClearHighlight();
        }
        else
        {
            ClearHighlight();
        }
    }

    private void OnDeletePressed(InputAction.CallbackContext ctx)
    {
        if (!deleteMode) return;

        if (ray != null && ray.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var deletable = hit.collider.GetComponentInParent<DeletableObject>();
            if (deletable != null)
            {
                // Suppression
                Destroy(deletable.gameObject);
                ClearHighlight();
            }
        }
    }

    public void SetDeleteMode(bool enabled)
    {
        deleteMode = enabled;
        if (!deleteMode) ClearHighlight();
    }

    private void Highlight(GameObject go)
    {
        if (highlightMaterial == null) return;

        var r = go.GetComponentInChildren<Renderer>();
        if (r == null) return;

        if (lastRenderer == r) return;

        ClearHighlight();

        lastRenderer = r;
        lastMats = r.sharedMaterials;

        // Ajoute un matériau de highlight en plus (simple)
        var newMats = new Material[lastMats.Length + 1];
        for (int i = 0; i < lastMats.Length; i++) newMats[i] = lastMats[i];
        newMats[newMats.Length - 1] = highlightMaterial;

        r.materials = newMats;
    }

    private void ClearHighlight()
    {
        if (lastRenderer != null && lastMats != null)
            lastRenderer.materials = lastMats;

        lastRenderer = null;
        lastMats = null;
    }
}
