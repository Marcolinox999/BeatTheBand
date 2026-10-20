using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PabloMonos : BossBase
{
    [SerializeField]protected new BossMoves CurrentMove;
    private Transform target;

    [Header("Boss Parameters")] 
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject upMonkeyAttack;
    [SerializeField] private Transform desiredUpPoint;
    [SerializeField] private Transform desiredDownPoint;
    private bool canPhase;
    private int attackStage;

    private new void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        boss.transform.position = desiredDownPoint.position;
    }

    protected override void MoveChoose()
    {
        base.MoveChoose();
        if (!useMove) return;
            switch (phaseIndex)
            {
                case 0:
                    CurrentMove = BossMoves.AttackPh1;
                    break;
                case 1:
                    CurrentMove = BossMoves.AttackPh2;
                    break;
                case 2:
                    CurrentMove = BossMoves.Wander;
                    break;
            }
            
    }

    protected override void Update()
    {
        base.Update();
        switch (CurrentMove)
        {
            case BossMoves.AttackPh1:
                AttackPh1();
                break;
            case BossMoves.AttackPh2:
                AttackPh2();
                break;
            case BossMoves.AttackPh3:
                AttackPh3();
                break;
            case BossMoves.Wander:
                CurrentMove = BossMoves.Wander;
                break;
        }
    }

    private void AttackPh3()
    {
    }

    private void Search()
    {
        var direction =target.position.y - transform.position.y <= 0?Vector3.down:Vector3.up;
        transform.Translate(direction * (currentPhase.bossSpeed * Time.deltaTime));
        if (canPhase)
        {
            StartCoroutine(WaitBetweenPhases(5));
            attacking = true;
            canPhase = false;
        }

    }

    private void Down()
    {
        boss.transform.position = math.lerp(boss.transform.position, desiredDownPoint.position, currentPhase.bossSpeed * Time.deltaTime);
        if (Vector3.Distance(boss.transform.position,desiredDownPoint.position) < 0.1)
        {
            attackStage = 0;
            MoveChoose();
            upMonkeyAttack.SetActive(false);
        }
    }

    private void AttackPh2()
    {
    }

    private void AttackPh1()
    {
        switch (attackStage)
        {
            case 0:
                Up();
                break;
            case 1:
                Search();
                break;
            case 2:
                Down();
                break;
            case 3:
                Down();
                break;
        }
    }

    private void Up()
    {
        boss.transform.position = math.lerp(boss.transform.position, desiredUpPoint.position, currentPhase.bossSpeed * Time.deltaTime);
        if (Vector3.Distance(boss.transform.position,desiredUpPoint.position) < 0.1)
        {
            upMonkeyAttack.SetActive(true);
            attackStage++;
            StartCoroutine(WaitBetweenPhases(10));
        }
    }

    private IEnumerator WaitBetweenPhases(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        attackStage++;
    }
}
