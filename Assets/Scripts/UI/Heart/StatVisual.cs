using System.Collections;
using UnityEngine;

public class StatVisual : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private BaseHealth playerHealth;
    [SerializeField] private WeaponManager weapon;

    [Header("Heart Parts")]
    [SerializeField] private SpriteRenderer heartMouth, heartHP, heartColor;

    [Header("Sprites")]
    [SerializeField] private Sprite[] hpSprites, colorSprites;
    [SerializeField] private Sprite idleFace, missedFace, okFace, perfectFace;
    [Header("Face")]
    [SerializeField] private float faceDuration = 0.4f;
    
    private Coroutine faceRoutine;

    private void OnEnable()
    {
        weapon.OnAttackScored += ChangeFace;
    }

    private void OnDisable()
    {
        weapon.OnAttackScored -= ChangeFace;
    }

    private void Start()
    {
        heartMouth.sprite = idleFace;
    }

    private void Update()
    {
        int health = Mathf.RoundToInt(playerHealth.CurrentHealth);
        int index = Mathf.Clamp(health - 1, 0, hpSprites.Length - 1);

        heartHP.sprite = hpSprites[index];
        heartColor.sprite = colorSprites[weapon.weaponID];
    }

    private void ChangeFace(BeatManager.Score score)
    {
        Sprite face = score switch
        {
            BeatManager.Score.Missed => missedFace,
            BeatManager.Score.Ok => okFace,
            BeatManager.Score.Perfect => perfectFace,
            _ => idleFace
        };
        if (faceRoutine != null)
            StopCoroutine(faceRoutine);

        faceRoutine = StartCoroutine(ShowFace(face));
    }

    private IEnumerator ShowFace(Sprite face)
    {
        heartMouth.sprite = face;
        yield return new WaitForSeconds(faceDuration);
        heartMouth.sprite = idleFace;
        faceRoutine = null;
    }
}