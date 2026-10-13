using System.Collections;
using UnityEngine;

public class MissFeedback : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;

    [Header("Color")]
    [SerializeField] private Color normalColor = Color.black;
    [SerializeField] private Color missColor = Color.red;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeMagnitude = 0.1f;
    

    private SpriteRenderer _barSprite;
    private AudioSource  _audioSource;
    private Transform _barTransform;
    private Vector3 _originalPosition;
    private Coroutine _colorRoutine;
    private Coroutine _shakeRoutine;

    private void Awake()
    {       
        _barSprite = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        
        _barTransform = _barSprite.transform;
        _originalPosition = _barTransform.localPosition;
        _barSprite.color = normalColor;
    }

    private void OnEnable()
    {
        weaponManager.OnAttackScored += HandleAttackScored;
    }

    private void OnDisable()
    {
        weaponManager.OnAttackScored -= HandleAttackScored;

        _barTransform.localPosition = _originalPosition;
        _barSprite.color = normalColor;
    }

    private void HandleAttackScored(BeatManager.Score score)
    {
        if (score != BeatManager.Score.Missed) return;

        _audioSource.Play();
        
        if (_colorRoutine != null) StopCoroutine(_colorRoutine);
        _colorRoutine = StartCoroutine(ColorRoutine(weaponManager.MissBeats));

        if (_shakeRoutine != null) StopCoroutine(_shakeRoutine);
        _shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ColorRoutine(int penaltyBeats)
    {
        _barSprite.color = missColor;

        float secondsPerBeat = 60f / BeatManager.instance.BPM;
        yield return new WaitForSeconds(penaltyBeats * secondsPerBeat);

        _barSprite.color = normalColor;
        _colorRoutine = null;
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float strength = shakeMagnitude * (1f - elapsed / shakeDuration);
            Vector2 offset = Random.insideUnitCircle * strength;
            _barTransform.localPosition = _originalPosition + new Vector3(offset.x, offset.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _barTransform.localPosition = _originalPosition;
        _shakeRoutine = null;
    }
}