using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string levelName;
    public string exportedAt;
    public List<LevelObjectData> objects = new();
}

[Serializable]
public class LevelObjectData
{
    public string prefabName;     // nom logique de l'objet
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}
