using TMPro;
using UnityEngine;
using System.IO;

public class LevelExportUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputLevelName;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private LevelExporter exporter;

    public void Export()
    {
        if (exporter == null || inputLevelName == null)
        {
            Debug.LogError("[LevelExportUI] Missing references (exporter or inputLevelName).");
            return;
        }

        string levelName = inputLevelName.text;
        exporter.ExportWithName(levelName);

        if (statusText != null)
        {
            string file = Path.GetFileName(exporter.LastExportPath);
            statusText.text = exporter.LastExportOverwrote
                ? $"Overwritten: {file}"
                : $"Exported: {file}";
        }
    }
}
