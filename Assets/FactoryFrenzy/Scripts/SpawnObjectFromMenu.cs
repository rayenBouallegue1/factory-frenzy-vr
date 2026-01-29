using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnObjectFromMenu : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    [Header("Feedback FP1-6")]
    [SerializeField] private VRFeedbackManager feedback;
    [SerializeField] private bool useRightHand = true;

    public void Spawn()
    {
        if (prefabToSpawn == null || spawnPoint == null)
        {
            Debug.LogError("[SpawnObjectFromMenu] Prefab ou SpawnPoint manquant");
            return;
        }

        GameObject obj = Instantiate(
            prefabToSpawn,
            spawnPoint.position,
            spawnPoint.rotation
        );

        //  FEEDBACK SPAWN (FP1-6)
        if (feedback != null)
        {
            feedback.SpawnFeedback(useRightHand);
        }
    }
}
