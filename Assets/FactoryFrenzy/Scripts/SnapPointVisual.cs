using UnityEngine;

public class SnapPointVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject root;
    [SerializeField] private Renderer[] renderers;

    [Header("Materials")]
    public Material validMat;
    public Material invalidMat;

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }

    public void ShowValid()
    {
        if (root != null)
            root.SetActive(true);

        Apply(validMat);
    }

    public void ShowInvalid()
    {
        if (root != null)
            root.SetActive(true);

        Apply(invalidMat);
    }

    private void Apply(Material mat)
    {
        if (mat == null || renderers == null) return;

        foreach (var r in renderers)
        {
            if (r != null)
                r.sharedMaterial = mat;
        }
    }
}