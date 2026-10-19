using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PabloMonos : BossBase
{
    [SerializeField]protected new BossMoves CurrentMove;
    private Transform target;

    [Header("Boss Parameters")] 
    [SerializeField] private GameObject boss;
    [SerializeField] private float bossSpeed;
    [SerializeField] private GameObject upMonkeyAttack;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    protected override void MoveChoose()
    {
        if (!useMove) return;
            base.MoveChoose();
            switch (phaseIndex)
            {
                case 0:
                    CurrentMove = BossMoves.AttackPh1;
                    Debug.Log("Dash");
                    break;
                case 1:
                    CurrentMove = BossMoves.AttackPh2;
                    Debug.Log("Up");
                    break;
                case 2:
                    CurrentMove = BossMoves.Wander;
                    Debug.Log("Wander");
                    break;
            }
            
    }

    protected override void Update()
    {
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
        var direction = target.position.y - transform.position.y <= 0?Vector3.up:Vector3.down;
        transform.Translate(direction * (bossSpeed * Time.deltaTime));
    }

    private void AttackPh2()
    {
    }

    private void AttackPh1()
    {
        boss.transform.position = math.lerp(boss.transform.position, new Vector3(transform.position.x,transform.position.y+10,transform.position.z), bossSpeed * Time.deltaTime);
        if (Vector3.Distance(boss.transform.position,new Vector3(transform.position.x, transform.position.y + 10, transform.position.z)) < 1)
        {
            upMonkeyAttack.SetActive(true);
            Search();
        }
    }
}
