using UnityEngine;

public class MobilePlatformMover : MonoBehaviour
{
    [Header("Path")]
    public Transform platform;          // la partie qui bouge (souvent this.transform)
    public Vector3 startPosition;
    public Vector3 endPosition;

    [Header("Timing")]
    [Min(0.1f)] public float moveDuration = 3f;

    [Header("Preview")]
    public bool loop = true;

    private float _t;
    private bool _isPreviewing;
    private Vector3 _initialPos;

    private void Awake()
    {
        if (platform == null) platform = transform;
        _initialPos = platform.position;

        // valeurs par défaut : départ = position actuelle, arrivée = +1m en X
        startPosition = platform.position;
        endPosition = platform.position + new Vector3(1f, 0f, 0f);
    }

    private void Update()
    {
        if (!_isPreviewing) return;

        if (moveDuration <= 0.01f) moveDuration = 0.01f;

        _t += Time.deltaTime / moveDuration;

        float pingPongT = loop ? Mathf.PingPong(_t, 1f) : Mathf.Clamp01(_t);
        platform.position = Vector3.Lerp(startPosition, endPosition, pingPongT);

        if (!loop && _t >= 1f)
        {
            StopPreview(resetToStart: false);
        }
    }

    // --- API appelée par l'UI ---
    public void SetStartToCurrent()
    {
        startPosition = platform.position;
    }

    public void SetEndToCurrent()
    {
        endPosition = platform.position;
    }

    public void SetDuration(float seconds)
    {
        moveDuration = Mathf.Max(0.1f, seconds);
    }

    public void StartPreview()
    {
        _isPreviewing = true;
        _t = 0f;
        // on part du start au preview
        platform.position = startPosition;
    }

    public void StopPreview(bool resetToStart = true)
    {
        _isPreviewing = false;
        _t = 0f;
        if (resetToStart) platform.position = startPosition;
    }

    public void ResetToInitial()
    {
        StopPreview(false);
        platform.position = _initialPos;
        startPosition = _initialPos;
        endPosition = _initialPos + new Vector3(1f, 0f, 0f);
        moveDuration = 3f;
    }
}
