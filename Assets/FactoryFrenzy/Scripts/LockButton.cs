using UnityEngine;

public class LockButton : MonoBehaviour
{
    public void ToggleLock()
    {
        if (SelectionManager.current != null)
            SelectionManager.current.ToggleLock();
    }
}