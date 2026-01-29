using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnObjectFromMenu : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    public void Spawn()
    {
        if (prefabToSpawn == null || spawnPoint == null)
        {
            Debug.LogError("Prefab ou SpawnPoint manquant");
            return;
        }

        GameObject obj = Instantiate(
            prefabToSpawn,
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}
