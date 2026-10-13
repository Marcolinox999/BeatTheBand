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

    private Vector3 baseScale;
    private Vector3 basePosition;

    private float beatTimer;
    private float bpm;

    private float waitTimer;
    private bool hasWaited;
    
    private bool isStretched;
    private Vector2 currentMultiplier = Vector2.one;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        baseScale = transform.localScale;
        basePosition = transform.position;

        if (particles != null)
            particles.Play(!GameManager.Instance.beatenLevels[levelNumber]);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalLevel;

        bpm = BeatManager.instance.BPM;
        
        hasWaited = !waitOneBeatBeforeStart;
        isStretched = true;
    }

    private void Update()
    {
        if (GameManager.Instance.beatenLevels[levelNumber])
        {
            Bounce();
        }
        else
        {
            spriteRenderer.sprite = BWLevel;

            ResetTransform();
            currentMultiplier = Vector2.one;

            if (particles != null)
            {
                particles.Clear();
                particles.Pause();
            }
        }
    }

    private void Bounce()
    {
        float beatDuration = 60f / bpm * beatSpeedMultiplier;
        
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