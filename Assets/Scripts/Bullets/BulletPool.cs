using System.Collections.Generic;
using UnityEngine;

namespace Bullets
{
    public class BulletPool : MonoBehaviour
    {
        private static BulletPool instance;

        public static BulletPool Instance
        {
            get
            {
                if (instance == null)
                    Debug.LogError("BulletPool Instance Not Found");
                return instance;
            }
        }
    
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int initialPoolSize;
    
        private List<Bullet> bulletPool = new List<Bullet>();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
            }
            AddBulletsToPool(initialPoolSize);
        }

        private void AddBulletsToPool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab);
                bullet.gameObject.SetActive(false);
                bulletPool.Add(bullet);
                bullet.transform.parent = transform;
            }
        }

        public Bullet GetBullet()
        {
            for (int i = 0; i < bulletPool.Count; i++)
            {
                if (!bulletPool[i].gameObject.activeSelf)
                {
                    bulletPool[i].gameObject.SetActive(true);
                    return bulletPool[i];
                }
            }
            AddBulletsToPool(1);
            bulletPool[bulletPool.Count - 1].gameObject.SetActive(true);
            return bulletPool[bulletPool.Count - 1];
        }
    }
}
