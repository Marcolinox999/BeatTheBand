using Unity.VisualScripting;
using UnityEngine;

public class BeatSquashStretch : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float beatSpeedMultiplier = 1f;
    [SerializeField] private float stretchAmount = 0.15f;
    [SerializeField] private float squashAmount = 0.08f;
    [SerializeField] private float animationSharpness = 8f;

    [Header("Ground Anchor")]
    [SerializeField] private bool anchorToGround = true;

    [Header("Extra")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Sprite normalLevel;
    [SerializeField] private Sprite BWLevel;
    [SerializeField] private int levelNumber = 0;
    

    private Vector3 baseScale;
    private Vector3 basePosition;
    
    private float beatTimer;
    private float bpm;

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

            transform.localScale = baseScale;
            transform.position = basePosition;

            if (particles != null)
            {
                particles.Clear();
                particles.Pause();
            }
        }
    }

    private void Bounce()
    {
        float beatDuration = 60f / bpm *  beatSpeedMultiplier;

        beatTimer += Time.deltaTime;

        if (beatTimer > beatDuration)
            beatTimer -= beatDuration;

        float t = beatTimer / beatDuration;

        float pulse = Mathf.Sin(t * Mathf.PI);
        pulse = Mathf.Pow(pulse, animationSharpness);

        float scaleY = 1f + stretchAmount * pulse;
        float scaleX = 1f - squashAmount * pulse;

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
}