using System;
using System.IO;
using UnityEngine;

public class LevelExporter : MonoBehaviour
{
    [Header("Default name if empty")]
    [SerializeField] private string defaultLevelName = "MyLevel";

    public string LastExportPath { get; private set; }
    public bool LastExportOverwrote { get; private set; }

    public void ExportWithName(string levelName)
    {
        // 1) Nom par défaut si vide
        if (string.IsNullOrWhiteSpace(levelName))
            levelName = defaultLevelName;

        levelName = SanitizeFileName(levelName);

        // 2) Construire la data
        LevelData data = BuildLevelData(levelName);

        // 3) JSON
        string json = JsonUtility.ToJson(data, true);

        // 4) Chemin + écrasement
        string path = Path.Combine(Application.persistentDataPath, levelName + ".json");
        LastExportOverwrote = File.Exists(path);

        File.WriteAllText(path, json);

        LastExportPath = path;

        Debug.Log(LastExportOverwrote
            ? $"[LevelExporter]  Fichier écrasé => {path}"
            : $"[LevelExporter]  Export OK => {path}");
    }

    private LevelData BuildLevelData(string levelName)
    {
        var deletables = FindObjectsOfType<DeletableObject>();

        LevelData data = new LevelData
        {
            levelName = levelName,
            exportedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        foreach (var d in deletables)
        {
            Transform t = d.transform;

            data.objects.Add(new LevelObjectData
            {
                prefabName = CleanCloneName(t.gameObject.name),
                position = t.position,
                rotation = t.rotation,
                scale = t.localScale
            });
        }

        return data;
    }

    private string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name.Trim();
    }

    private string CleanCloneName(string name)
    {
        // Unity ajoute souvent "(Clone)" => on le retire
        return name.Replace("(Clone)", "").Trim();
    }
}
