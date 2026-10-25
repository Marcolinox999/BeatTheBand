using UnityEngine;

public class BeatSquashStretch : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private bool anchorToGround = true;
    [SerializeField] private float beatSpeedMultiplier = 1f;
    [SerializeField] private float stretchAmount = 0.15f;
    [SerializeField] private float squashAmount = 0.08f;
    [SerializeField] private float animationSharpness = 8f;

    [Header("Sequence")]
    [SerializeField] private bool alternateStretchSquash = false;
    [SerializeField] private bool waitOneBeatBeforeStart = false;

    [Header("Extra")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Sprite normalLevel;
    [SerializeField] private Sprite BWLevel;
    [SerializeField] private int levelNumber = 0;

    [Header("Colors")]
    [SerializeField] private bool useColors = false;
    [SerializeField] private Color notBeatenColor = Color.white;
    [SerializeField] private Color beatenColor = Color.white;

    [Tooltip("Optional. Assign a material that supports sprite tint (e.g. Sprites-Default) " +
             "if your current material ignores the color")]
    [SerializeField] private Material overrideMaterial;

    [Tooltip("Logs the applied color whenever the beaten state changes")]
    [SerializeField] private bool debugColors = false;

    private Vector3 baseScale;
    private Vector3 basePosition;

    private float beatTimer;
    private float bpm;

    private float waitTimer;
    private bool hasWaited;

    private bool isStretched;
    private Vector2 currentMultiplier = Vector2.one;

    private SpriteRenderer spriteRenderer;      // Used to swap the sprite
    private SpriteRenderer[] spriteRenderers;   // All renderers, used to apply the color

    private bool? lastBeatenState = null;       // Only used to avoid spamming the debug log

    private void Start()
    {
        baseScale = transform.localScale;
        basePosition = transform.position;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (spriteRenderers.Length > 0)
        {
            spriteRenderer = spriteRenderers[0];
            spriteRenderer.sprite = normalLevel;

            if (overrideMaterial != null)
            {
                foreach (SpriteRenderer sr in spriteRenderers)
                    sr.sharedMaterial = overrideMaterial;
            }
        }
        else
        {
            Debug.LogError(name + ": no SpriteRenderer found on the object or its children.");
        }

        if (particles != null && GameManager.Instance != null && IsValidLevel())
            particles.Play(!GameManager.Instance.beatenLevels[levelNumber]);

        if (BeatManager.instance != null)
            bpm = BeatManager.instance.BPM;
        else
            Debug.LogWarning(name + ": no BeatManager found, the icon will not bounce.");

        hasWaited = !waitOneBeatBeforeStart;
        isStretched = true;
    }

    private bool IsValidLevel()
    {
        return GameManager.Instance != null
            && GameManager.Instance.beatenLevels != null
            && levelNumber >= 0
            && levelNumber < GameManager.Instance.beatenLevels.Length;
    }

    private void SetColor(Color color)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
            sr.color = color;
    }

    private void Update()
    {
        if (spriteRenderer == null || !IsValidLevel()) return;

        if (!GameManager.Instance.beatenLevels[levelNumber])
        {
            // Level not completed: dance (particles keep playing)
            spriteRenderer.sprite = BWLevel;

            Bounce();
        }
        else
        {
            // Level completed: stop dancing and stop particles
            spriteRenderer.sprite = normalLevel;

            ResetTransform();
            currentMultiplier = Vector2.one;

            if (particles != null)
            {
                particles.Clear();
                particles.Pause();
            }
        }
    }

    // The color is applied after Update and after any Animator, so nothing overrides it
    private void LateUpdate()
    {
        if (!useColors || spriteRenderers == null || spriteRenderers.Length == 0 || !IsValidLevel()) return;

        bool isBeaten = GameManager.Instance.beatenLevels[levelNumber];
        Color targetColor = isBeaten ? beatenColor : notBeatenColor;

        SetColor(targetColor);

        if (debugColors && lastBeatenState != isBeaten)
        {
            lastBeatenState = isBeaten;
            Debug.Log(name + " | beaten: " + isBeaten
                + " | target: " + targetColor
                + " | applied: " + spriteRenderers[0].color
                + " | renderers tinted: " + spriteRenderers.Length
                + " | material: " + spriteRenderers[0].sharedMaterial.name);
        }
    }

    private void Bounce()
    {
        // A BPM of 0 would make beatDuration 0 and freeze the while loop below
        if (bpm <= 0f) return;

        float beatDuration = 60f / bpm * beatSpeedMultiplier;
        if (beatDuration <= 0f) return;

        if (!hasWaited)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer < beatDuration)
            {
                ResetTransform();
                return;
            }

            hasWaited = true;
            beatTimer = 0f;
            isStretched = true;
        }

        beatTimer += Time.deltaTime;

        while (beatTimer >= beatDuration)
        {
            beatTimer -= beatDuration;
            isStretched = !isStretched;
        }

        float scaleX;
        float scaleY;

        if (alternateStretchSquash)
        {
            Vector2 target = isStretched
                ? new Vector2(1f - squashAmount, 1f + stretchAmount)
                : new Vector2(1f + squashAmount, 1f - squashAmount);

            float lerpFactor = 1f - Mathf.Exp(-animationSharpness * Time.deltaTime);
            currentMultiplier = Vector2.Lerp(currentMultiplier, target, lerpFactor);

            scaleX = currentMultiplier.x;
            scaleY = currentMultiplier.y;
        }
        else
        {
            float t = beatTimer / beatDuration;
            float pulse = Mathf.Pow(Mathf.Sin(t * Mathf.PI), animationSharpness);

            scaleY = 1f + stretchAmount * pulse;
            scaleX = 1f - squashAmount * pulse;
        }

        Vector3 newScale = new Vector3(baseScale.x * scaleX, baseScale.y * scaleY, baseScale.z);

        transform.localScale = newScale;

        if (anchorToGround)
        {
            float addedHeight = newScale.y - baseScale.y;

            transform.position = new Vector3(basePosition.x, basePosition.y + addedHeight * 0.5f, basePosition.z);
        }
        else
        {
            transform.position = basePosition;
        }
    }

    private void ResetTransform()
    {
        transform.localScale = baseScale;
        transform.position = basePosition;
    }
}