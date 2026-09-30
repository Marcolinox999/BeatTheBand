using Bullets;
using UnityEngine;

public class HomingBullet : BaseBullet
{
    private GameObject target;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Boss");
    }
    protected override void Movement()
    {
         Vector3 desiredLocation =new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, 0).normalized;
         transform.Translate(desiredLocation * (speed * Time.deltaTime));
    }
}
