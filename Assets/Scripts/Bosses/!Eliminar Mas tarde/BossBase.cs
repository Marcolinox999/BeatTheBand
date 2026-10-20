using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBase : BaseHealth
{
   [SerializeField] protected BossPhase[] phases;
   protected BossPhase currentPhase;
   [SerializeField]protected int attackTime;
   protected bool attacking = false;
   protected int phaseIndex;
   protected enum BossMoves
   {
      Wander,
      AttackPh1,
      AttackPh2,
      AttackPh3,
   }
   protected BossMoves CurrentMove;

   protected bool useMove;

   private void OnEnable()
   {
      ChangePhase(0);
      MoveChoose();
   }
   protected virtual void Update()
   {
      if (CurrentHealth <= currentPhase.healthThreshold)
      {
         ChangePhase(phaseIndex+1);
      }
      if (CurrentHealth <= 0 && !attacking)
      {
         Die();
      }
   }

   protected virtual void MoveChoose()
   {
      if (attacking) return;
      useMove = Random.value < 0.5f;
      Debug.Log(useMove);
      if (!useMove)
      {
         CurrentMove = BossMoves.Wander;
         StartCoroutine(WanderAttack());
      }
   }
   protected void ChangePhase(int index)
   {
      phaseIndex = index;
      currentPhase = phases[phaseIndex];

      Debug.Log("Fase actual: " + phaseIndex);
   }
   public override void ApplyDamage(float damage)
   {
      base.ApplyDamage(damage);
      if (damage <= 0f || CurrentHealth <= 0f) return;
      
   }

   protected override void SpriteDamage()
   {
      
   }

   protected override void ParticleDamage()
   {
      
   }

   protected override void Die()
   {
      //Muere
      Destroy(gameObject);
   }

   protected IEnumerator WanderAttack()
   {
      attacking = true;
      int randomAttack= Random.Range(0,currentPhase.attacks.Length);
      currentPhase.attacks[randomAttack].gameObject.SetActive(true);
      yield return new WaitForSeconds(attackTime);
      currentPhase.attacks[randomAttack].gameObject.SetActive(false);
      yield return new WaitForSeconds(5);
      attacking = false;
      MoveChoose();
   }
}
