using UnityEngine;

[System.Serializable]
public class BossPhase
{
    [Header("Phase Settings")]
    public float healthThreshold;
    public float transitionPause = 1f;

    [Header("Transition Effect")]
    public GameObject transitionEffect;

    [Header("Attacks")]
    public GameObject[] attacks;
}