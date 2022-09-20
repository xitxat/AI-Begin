using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  Script Placement: Green Tank
//  Slots: Tank Renders Obg's
//  Used with ShellAI

public class FireShell : MonoBehaviour
{
    public GameObject bullet;
    public GameObject turret;
    public GameObject enemy;
    public Transform turretBase;
    float speed = 10;           // distance to attack?
    float rotSpeed = 5;
    float moveSpeed = 1;

    public float interval = 3f;
    float nextActionTime = 0f;


 
    void CreateBullet()
    {
        // GObj shell wraps itself around each bullet
        GameObject shell =        Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        //  Add flight characteristics
        shell.GetComponent<Rigidbody>().velocity = speed*turretBase.forward;
    }

    float? RotateTurret()                       //~~~~~ High / Low trajetory switch
    {
        // bool refing the CalcAng low / high option
        float? angle = CalculateAngle(false);
        if (angle != null)
        {   // compensate for current X
            turretBase.localEulerAngles = new Vector3(360f -(float)angle , 0f ,0f);
        }
        return angle;
    }

                               
    //  EQ. tan(a) =(stuff)/gx
    //   x? allows return null
    //  remomve Y axis from calc
    float? CalculateAngle(bool low)         //~~~~~   CALCULATE ANGLE
    {
        Vector3 targetDir = enemy.transform.position - transform.position;
        float y = targetDir.y;
        targetDir.y = 0;
        // X: the distance between the 2 Objs. -1 fudge factor for bullet to tank center offset
        float x = targetDir.magnitude -1f;
        float gravity = 9.8f;   //antigravity wtf
        float sSqr = speed * speed;
        float underTheSqrRoot = (sSqr*sSqr) -gravity * (gravity * x*x+2*y*sSqr);

        //  don't go into imaginary -sqrR #'s
        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (low)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
        }
        else
            return null;

    }



    //                                          ~~~~    UPDATE
    void Update()
    {
        
        //  When normalized, a vector keeps the same direction but its length is 1.0.
        // ID the enemy
        Vector3 direction = (enemy.transform.position - transform.position).normalized;

        //   turn around, and stay on the ground 0,
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //   Slerp: Spherical lerp, non snapping
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

       float? angle = RotateTurret();


        //~~~~~~~TRY fire every x sec : ok.  PLUS move into range: OK
        if (angle == null)
        {

            transform.Translate(0, 0, Time.deltaTime * moveSpeed); // new
        }

        if (Time.time > nextActionTime)
        {
            nextActionTime += interval;

            if (angle != null)
            {
                CreateBullet();
            }

        }
          // END TRY




          //~~~~ORIGINAL~~~~//
        //if (angle != null)
        //{
        //    CreateBullet(); 
        //}
        //else
        //{
        //    //  move tank
        //    transform.Translate(0, 0, Time.deltaTime * moveSpeed);
        //}

    }
}
