using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRDeleteTool : MonoBehaviour
{
    [Header("Ray (Right Ray Interactor)")]
    [SerializeField] private XRRayInteractor ray;

    [Header("Right Controller (Action-based)")]
    [SerializeField] private ActionBasedController rightController;

    [Header("UI")]
    [SerializeField] private GameObject deleteModeText;   // "DELETE MODE ACTIVE" (GameObject)
    [SerializeField] private TMPro.TextMeshProUGUI debugText; // optionnel (un texte debug dans le menu)

    [Header("Mode")]
    [SerializeField] private bool deleteMode = false;

    [Header("Visual")]
    [SerializeField] private Color hoverColor = Color.red;
    [SerializeField] private float deleteDelay = 0.2f;

    private Renderer lastRenderer;
    private Color[] lastColors;

    private void Awake()
    {
        SetDeleteMode(false); // mode normal par défaut
    }

    public void SetDeleteMode(bool enabled)
    {
        deleteMode = enabled;

        if (deleteModeText != null)
            deleteModeText.SetActive(deleteMode);

        WriteDebug("Mode=" + (deleteMode ? "DELETE" : "NORMAL"));
        if (!deleteMode) ClearHoverColor();
        Debug.Log("CALLED SetDeleteMode => " + enabled);
    }

    private void Update()
    {
        if (!deleteMode)
        {
            ClearHoverColor();
            return;
        }

        // 1) Ray hit ?
        if (ray == null)
        {
            WriteDebug("ERREUR: Ray = NULL");
            return;
        }

        bool hitSomething = ray.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        if (!hitSomething)
        {
            ClearHoverColor();
            WriteDebug("DELETE: no hit");
            return;
        }

        var deletable = hit.collider.GetComponentInParent<DeletableObject>();
        if (deletable == null)
        {
            ClearHoverColor();
            WriteDebug("HIT: " + hit.collider.name + " (NOT deletable)");
            return;
        }

        // 2) Hover rouge
        ApplyHoverColor(deletable.gameObject);
        WriteDebug("HIT deletable: " + deletable.name);

        // 3) Input du contrôleur droit : Activate
        if (rightController == null)
        {
            WriteDebug("ERREUR: RightController = NULL");
            return;
        }

        // Trigger/Activate press
        if (rightController.activateAction.action != null &&
            rightController.activateAction.action.WasPressedThisFrame())
        {
            StartCoroutine(DeleteWithRedFlash(deletable.gameObject));
            ClearHoverColor();
        }
    }

    private IEnumerator DeleteWithRedFlash(GameObject go)
    {
        if (go == null) yield break;

        ApplyHoverColor(go);
        yield return new WaitForSeconds(deleteDelay);

        if (go != null)
            Destroy(go);
    }

    private void ApplyHoverColor(GameObject go)
    {
        var r = go.GetComponentInChildren<Renderer>();
        if (r == null) return;
        if (lastRenderer == r) return;

        ClearHoverColor();

        lastRenderer = r;
        var mats = r.materials;
        lastColors = new Color[mats.Length];

        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i] == null) { lastColors[i] = Color.white; continue; }

            if (mats[i].HasProperty("_BaseColor"))
                lastColors[i] = mats[i].GetColor("_BaseColor");
            else if (mats[i].HasProperty("_Color"))
                lastColors[i] = mats[i].color;
            else
                lastColors[i] = Color.white;

            if (mats[i].HasProperty("_BaseColor"))
                mats[i].SetColor("_BaseColor", hoverColor);
            if (mats[i].HasProperty("_Color"))
                mats[i].color = hoverColor;
        }
    }

    private void ClearHoverColor()
    {
        if (lastRenderer == null || lastColors == null) return;

        var mats = lastRenderer.materials;
        for (int i = 0; i < mats.Length && i < lastColors.Length; i++)
        {
            if (mats[i] == null) continue;

            if (mats[i].HasProperty("_BaseColor"))
                mats[i].SetColor("_BaseColor", lastColors[i]);
            if (mats[i].HasProperty("_Color"))
                mats[i].color = lastColors[i];
        }

        lastRenderer = null;
        lastColors = null;
    }

    private void WriteDebug(string msg)
    {
        if (debugText != null)
            debugText.text = msg;
    }
}
