using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectableObject : MonoBehaviour
{
    private EditableObject editable;

    void Awake()
    {
        editable = GetComponent<EditableObject>();
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        SelectionManager.current = editable;
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (SelectionManager.current == editable)
            SelectionManager.current = null;
    }
}