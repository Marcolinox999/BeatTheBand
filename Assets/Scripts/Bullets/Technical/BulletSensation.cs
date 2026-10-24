using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletSensation : MonoBehaviour
{
   [SerializeField] private float bouncyScale;
   private float normalScale;
   private bool scaling = true;

   private void Awake()
   {
      normalScale = transform.localScale.x;
   }

   private void Update()
   {
      if (scaling)
      {
         float actualScale = math.lerp(transform.localScale.x, bouncyScale, 3* Time.fixedDeltaTime);
         transform.localScale = new Vector3(actualScale, actualScale, actualScale);
      }
      else
      {
         float actualScale = math.lerp(transform.localScale.x, normalScale,  3*Time.fixedDeltaTime);
         transform.localScale = new Vector3(actualScale, actualScale, actualScale);
      }

      if (transform.localScale.x - bouncyScale <= 0.001f)
      {
         scaling = false;
      }
      else if (normalScale -transform.localScale.x <= 0.001f)
      {
         scaling = true;
      }
   }
   
}
