using UnityEngine;

namespace Bullets
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField] private float shootCooldown;
        [SerializeField] private  RadialShotSettings shotSettings;

        private float shootCooldownTimer = 0f;

        private void Update()
        {
            shootCooldownTimer -= Time.deltaTime;

            if (shootCooldownTimer <= 0f)
            {
                ShotAttack.RadialShot(transform.position, transform.up, shotSettings);
                shootCooldownTimer += shootCooldown;
            }
        }
    }
}
