using UnityEngine;
using UnityEngine.UI;

public class DeleteToggleBinder : MonoBehaviour
{
    public Toggle toggle;
    public VRDeleteTool deleteTool;

    private void Awake()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        Debug.Log("[DeleteToggleBinder] Toggle isOn = " + isOn);
        if (deleteTool != null)
            deleteTool.SetDeleteMode(isOn);
    }
}
