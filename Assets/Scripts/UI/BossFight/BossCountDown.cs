using System;
using Player;
using TMPro;
using UnityEngine;

public class BossCountDown : MonoBehaviour
{
    [Header("Boss Countdown")]
    [SerializeField] private BasicBoss basicBoss;
    [SerializeField] private BasicMovementBoss bossBall;
    [SerializeField] private TextMeshProUGUI bossCountdownText;
    [SerializeField] private float countDown;
    private float timer;

    private void Awake()
    {
        timer  = countDown;
    }

    private void Update()
    {
     timer -= Time.deltaTime;
     int visualtimer = (int)timer;
     bossCountdownText.text = visualtimer.ToString("F2");
     Debug.Log((timer-visualtimer) /1);
    }
}
