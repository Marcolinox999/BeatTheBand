using UnityEngine;

namespace Bullets
{
    public class Bullet : MonoBehaviour
    {
        private const float maxLifetime = 3f;
        private float lifetime = 0f;
    
        public Vector2 Velocity;
    
        private void Update()
        {
            transform.position += (Vector3) Velocity * Time.deltaTime;
            lifetime += Time.deltaTime;

            if (lifetime > maxLifetime)
            {
                Disable();
            }
        }

        private void Disable()
        {
            lifetime = 0f;
            gameObject.SetActive(false);
        }
    }
}
