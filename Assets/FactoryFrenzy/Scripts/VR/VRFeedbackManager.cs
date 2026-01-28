using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRFeedbackManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sfxSpawn;
    [SerializeField] private AudioClip sfxDelete;
    [SerializeField] private AudioClip sfxGrab;

    [Header("Haptics")]
    [SerializeField] private XRBaseController leftController;
    [SerializeField] private XRBaseController rightController;

    [Header("Default Haptics")]
    [Range(0f, 1f)] public float spawnAmplitude = 0.25f;
    public float spawnDuration = 0.08f;

    [Range(0f, 1f)] public float deleteAmplitude = 0.7f;
    public float deleteDuration = 0.12f;

    [Range(0f, 1f)] public float grabAmplitude = 0.12f;
    public float grabDuration = 0.05f;

    [Header("Mode Feedback")]
    [SerializeField] private AudioClip sfxDeleteMode;
    [Range(0f, 1f)] public float modeAmplitude = 0.2f;
    public float modeDuration = 0.06f;


    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    // ---- API simple ----
    public void SpawnFeedback(bool rightHand = true)
    {
        Play(sfxSpawn);
        Haptic(rightHand, spawnAmplitude, spawnDuration);
    }

    public void DeleteFeedback(bool rightHand = true)
    {
        Play(sfxDelete);
        Haptic(rightHand, deleteAmplitude, deleteDuration);
    }

    public void GrabFeedback(bool rightHand = true)
    {
        Play(sfxGrab);
        Haptic(rightHand, grabAmplitude, grabDuration);
    }

    // ---- Helpers ----
    private void Play(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    public void DeleteModeFeedback(bool rightHand = true)
    {
        Play(sfxDeleteMode);
        Haptic(rightHand, modeAmplitude, modeDuration);
    }

    private void Haptic(bool rightHand, float amplitude, float duration)
    {
        XRBaseController ctrl = rightHand ? rightController : leftController;
        if (ctrl == null) return;

        // amplitude: 0..1
        ctrl.SendHapticImpulse(amplitude, duration);
    }
    public void GrabRight() => GrabFeedback(true);
    public void GrabLeft() => GrabFeedback(false);

    public void DeleteModeOnRight() => DeleteModeFeedback(true);
    public void DeleteModeOnLeft() => DeleteModeFeedback(false);

}
