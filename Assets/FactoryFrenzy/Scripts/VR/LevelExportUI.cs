using TMPro;
using UnityEngine;
using System.IO;

public class LevelExportUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private LevelExporter exporter;

    public void Export()
    {
        if (exporter == null)
        {
            Debug.LogError("[LevelExportUI] Missing reference: exporter.");
            return;
        }

        exporter.ExportWithName(""); // ignoré, le nom est auto

        if (statusText != null)
        {
            string file = Path.GetFileName(exporter.LastExportPath);
            statusText.text = exporter.LastExportOverwrote
                ? $" Overwritten: {file}"
                : $" Exported: {file}";
        }
    }
}
