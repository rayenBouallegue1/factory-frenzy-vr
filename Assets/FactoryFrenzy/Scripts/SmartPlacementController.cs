using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class SmartPlacementController : MonoBehaviour
{
    [Header("Mode")]
    public bool smartMode = true;
    private SnapPoint snappedTo; // le snap point actuel de cet objet
    [Header("Rotation snap (degrees)")]
    [Min(0f)] public float rotationStep = 10f;

    [Header("Snap Search")]
    [Min(0f)] public float extraSnapRadius = 0.05f;
    public LayerMask snapPointLayer = ~0; // optionnel si tu filtres via layer

    [Header("Input (toggle smart mode)")]
    public XRNode inputSource = XRNode.RightHand;
    public InputHelpers.Button toggleButton = InputHelpers.Button.PrimaryButton;
    public float toggleThreshold = 0.1f;

    [Header("Behavior")]
    public bool snapRotationToPoint = true;
    public bool markSnapPointOccupied = true;

    private XRGrabInteractable grab;
    private SnapObject snapObj;
    private bool togglePressedLastFrame;

    private SnapPoint currentBest;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        snapObj = GetComponent<SnapObject>();
    }

    private void OnEnable()
    {
        grab.selectExited.AddListener(OnReleased);
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grab.selectExited.RemoveListener(OnReleased);
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
        HideAllSnapVisuals();
    }

    private void Update()
    {
        HandleToggle();

        if (!grab.isSelected)
        {
            currentBest = null;
            HideAllSnapVisuals();
            return;
        }

        if (!smartMode || snapObj == null)
        {
            HideAllSnapVisuals();
            return;
        }

        UpdateSnapVisuals();
        if (currentBest == null) return;

        if (currentBest.CanAccept(snapObj))
            currentBest.ShowValid();
        else
            currentBest.ShowInvalid();
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (!smartMode || snapObj == null) return;

        var best = FindBestSnapPoint();
        HideAllSnapVisuals();

        if (best == null) return;
        if (!best.CanAccept(snapObj)) return;

        // 🔴 ICI : vérification du snap réel
        float d = Vector3.Distance(
            snapObj.Pivot.position,
            best.transform.position
        );

        float snapDist = best.snapRadius + extraSnapRadius;
        if (d > snapDist) return; // ❌ trop loin → pas de snap

        // ✅ Snap autorisé
        PlaceOnSnapPoint(best);

        if (markSnapPointOccupied)
        {
            best.SetOccupied(true);
            snappedTo = best;
        }
    }
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Si on était posé sur un SnapPoint, on le libère pour pouvoir re-snap
        if (snappedTo != null && markSnapPointOccupied)
        {
            snappedTo.SetOccupied(false);
            snappedTo = null;
        }
    }

    private void HandleToggle()
    {
        var device = InputDevices.GetDeviceAtXRNode(inputSource);
        if (!device.isValid) return;

        if (InputHelpers.IsPressed(device, toggleButton, out bool pressed, toggleThreshold))
        {
            if (pressed && !togglePressedLastFrame)
                smartMode = !smartMode;

            togglePressedLastFrame = pressed;
        }
    }

    private void ConstrainRotationStep()
    {
        if (rotationStep <= 0.01f) return;

        var euler = transform.rotation.eulerAngles;
        euler.x = SnapAngle(euler.x, rotationStep);
        euler.y = SnapAngle(euler.y, rotationStep);
        euler.z = SnapAngle(euler.z, rotationStep);
        transform.rotation = Quaternion.Euler(euler);
    }

    private static float SnapAngle(float angle, float step)
        => Mathf.Round(angle / step) * step;

    private SnapPoint FindBestSnapPoint()
    {
        var pivot = snapObj.Pivot;
        var points = SnapRegistry.Points;

        SnapPoint best = null;
        float bestDist = float.MaxValue;

        for (int i = 0; i < points.Count; i++)
        {
            var sp = points[i];
            if (sp == null) continue;

            // Layer filter optionnel (si tu utilises un layer dédié)
            if (((1 << sp.gameObject.layer) & snapPointLayer.value) == 0) continue;

            float maxDist = sp.previewRadius;
            float d = Vector3.Distance(pivot.position, sp.transform.position);

            if (d <= maxDist && d < bestDist)
            {
                bestDist = d;
                best = sp;
            }
        }

        return best;
    }

    private void PlaceOnSnapPoint(SnapPoint sp)
    {
        // On veut amener le pivot exactement sur le SnapPoint
        Transform pivot = snapObj.Pivot;
        Vector3 deltaPivotToObj = pivot.position - transform.position;
        transform.position = sp.transform.position - deltaPivotToObj;

        if (snapRotationToPoint)
            transform.rotation = sp.transform.rotation;
        else
            ConstrainRotationStep();
    }

    private void HideAllSnapVisuals()
    {
        var points = SnapRegistry.Points;
        for (int i = 0; i < points.Count; i++)
            points[i]?.Hide();
    }
    private void UpdateSnapVisuals()
    {
        var pivot = snapObj.Pivot;
        var points = SnapRegistry.Points;

        SnapPoint best = null;
        float bestDist = float.MaxValue;

        for (int i = 0; i < points.Count; i++)
        {
            var sp = points[i];
            if (sp == null) continue;

            float d = Vector3.Distance(pivot.position, sp.transform.position);

            if (d <= sp.previewRadius)
            {
                if (sp.CanAccept(snapObj))
                    sp.ShowValid();
                else
                    sp.ShowInvalid();

                if (d < bestDist)
                {
                    bestDist = d;
                    best = sp;
                }
            }
            else
            {
                sp.Hide();
            }
        }

        currentBest = best;
    }
}