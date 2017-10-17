
using UnityEngine;
using System.Collections;

public class shot : MonoBehaviour{


    ///////////////////////////////////////////////////////////////////VARIABLES///////////////////////////////////////////////////////////////////////

    
 public Rigidbody bulletCasing;
    
 public int ejectSpeed = 100;
    
 public double fireRate = 0.5;
 
 private double nextFire = 0.0;


   
    void Update()
    {

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
             Rigidbody bullet;

            nextFire = Time.time + fireRate;

            bullet = Instantiate(bulletCasing, transform.position, transform.rotation);
          
            bullet.velocity = transform.TransformDirection(Vector3.left* ejectSpeed);
          
        }



 }

}