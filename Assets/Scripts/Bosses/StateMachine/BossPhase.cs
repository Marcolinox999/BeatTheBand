using UnityEngine;

[System.Serializable]
public class BossPhase
{
    [Header("Phase Settings")]
    public float healthThreshold;

    [Tooltip("Tiempo que el boss espera antes de comenzar esta fase.")]
    public float transitionPause = 1f;

    [Header("Transition Effect")]
    [Tooltip("GameObject que se activará durante la transición a esta fase.")]
    public GameObject transitionEffect;

    [Header("Attacks")]
    public GameObject[] attacks;
}