using System;
using Player;
using TMPro;
using UnityEngine;

public class BossCountDown : MonoBehaviour
{
    [Header("Boss Countdown")]
    [SerializeField] private BasicBoss basicBoss;
    [SerializeField] private BasicMovementBoss bossBall;
    [SerializeField] private PlayerBehaviour player;
    [SerializeField] private TextMeshProUGUI bossCountdownText;
    [SerializeField] private float countDown;
    private float timer;
    
    private void Awake()
    {
        timer  = countDown;
        basicBoss.enabled = false;
        bossBall.enabled = false;
        player.enabled = false;
    }
    
    
    private void Update()
    {
     timer -= Time.deltaTime;
     int visualtimer = (int)timer;
     bossCountdownText.text = visualtimer.ToString("F0");
     if (timer <= 0)
     {
         player.enabled = true;
         basicBoss.enabled = true;
         bossBall.enabled = true;
         Destroy(gameObject);
     }
    }
}