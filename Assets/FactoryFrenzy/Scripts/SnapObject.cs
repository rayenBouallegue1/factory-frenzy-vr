using UnityEngine;

public enum SnapGroup
{
    None,
    Platform,
    Trap,
    Checkpoint,
    Start,
    Finish
}

public class SnapObject : MonoBehaviour
{
    [Header("Snap Object")]
    public SnapGroup snapGroup = SnapGroup.Platform;

    [Tooltip("Point de référence utilisé pour mesurer la distance au SnapPoint.")]
    public Transform pivot;

    private void Reset()
    {
        pivot = transform; // fallback
    }

    public Transform Pivot => pivot != null ? pivot : transform;
}