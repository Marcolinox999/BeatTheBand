using UnityEngine;

[System.Serializable]
public class BossPhase
{
    [Header("Phase Settings")]
    public float healthThreshold;
    public float transitionPause = 1f;
    public float bossSpeed;
    [Header("Transition Effect")]
    public GameObject transitionEffect;

    [Header("Attacks")]
    public GameObject[] attacks;
    public float attackDuration = 2f;
    public float waitBetweenAttacks = 3f;
    [Tooltip("0:PTP,1:Jump,2:Stay")]
    [Range(0,2)]public int movingType;
    [Tooltip("0:Normal,1:Accelerate,2:Decelerate,3:Arc,4:Sinusoidal")]
    [Range(0, 4)] public int movingStyle;
}