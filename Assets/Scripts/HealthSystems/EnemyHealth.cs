using UnityEngine;

public class EnemyHealth : BaseHealth
{
   protected override void Die()
   {
      Debug.Log("I'm a boss and I was killed");
   }
}
