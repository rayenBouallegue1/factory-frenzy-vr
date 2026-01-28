using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    [Header("Snap Rules")]
    public SnapGroup acceptsGroup = SnapGroup.Platform;
    [Header("UX Radius")]
    public float previewRadius = 1.2f; // visible de loin
    public float snapRadius = 0.25f;   // snap réel

    public bool occupied = false;
    public bool allowIfOccupied = false;

    [Header("Visual")]
    public SnapPointVisual visual;

    private void OnEnable() => SnapRegistry.Register(this);
    private void OnDisable() => SnapRegistry.Unregister(this);

    public bool CanAccept(SnapObject obj)
    {
        if (obj == null) return false;
        if (obj.snapGroup != acceptsGroup) return false;
        if (occupied && !allowIfOccupied) return false;
        return true;
    }

    public void SetOccupied(bool value) => occupied = value;

    public void Hide() { if (visual) visual.Hide(); }
    public void ShowValid() { if (visual) visual.ShowValid(); }
    public void ShowInvalid() { if (visual) visual.ShowInvalid(); }


}