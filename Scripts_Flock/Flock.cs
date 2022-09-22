using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  ~~~~    \\
//  DROP ONTO PREFAB
//  ref. FlockSheepManager.cs



public class Flock : MonoBehaviour
{
    Animator anim;
    float speed;

    //  Spacial limits for each fish: Bounds
    //  True when at boundry
    //  Don't join flock
    bool turning = false;

    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        anim = this.GetComponent<Animator>();

                                                                //  ~~   ANIMATION OFFSET 
        //      Animator anim;
        //      anim = this.GetComponent<Animator>();
        // Hier. Char. Animator Controler. State Box. Parameters - add Float xOffset. Inspector. Cycle Offset + param.
        anim.SetFloat("swimOffset", Random.Range(0.0f, 1.0f));

        // Adjust tail anim to swim speed
        // Hier. Char. Animator Controler. State Box. Parameters - add Float swimSpeed. Inspector. Speed Multiplyer + param. = 1.
        anim.SetFloat("swimSpeed", speed);
        // copy to update / if not turning ie.else


    }


    void Update()
    {
                                                                //  ~~  BOUNDRY TEST
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);
        if (!b.Contains(transform.position))    // out of box
        {
            turning = true;
        }
        else
        turning = false;

        if (turning)
        {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;    //  set dir to go back to center, then set facing dir
            transform.rotation = Quaternion.Slerp(transform.rotation,                   
                                           Quaternion.LookRotation(direction),
                                                    FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
            {
                                                                    //  ~~ SPEED
                if (Random.Range(0, 100) < 10)
                {
                    speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
                }

                //  Update Rule TIMER
                //  <% = straighter path
                if (Random.Range(0, 100) < 1)
                {
                    ApplyRules();                                       // ~~ RULES
                }

            anim.SetFloat("swimSpeed", speed);

        }


        this.transform.Translate(0,0, speed * Time.deltaTime);  // ~~ MOVE
    }

    void ApplyRules()
    {
        // Average the  Groups dir, group's center , avoid neighbour

        GameObject[] gameObjs;              //   ref the prefabs in the mnager
        gameObjs = FlockManager.FM.allFish;

        Vector3 vCenter = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;      //   antiCollision vector
        float groupSpeed = 0.01f;
        float neighbourDistance;
        int groupSize = 0;

        //  neighbourDistance. Addition
        foreach (GameObject go in gameObjs)              
        {
            if (go != this.gameObject)                  // Don't compare to self
            {
                neighbourDistance = Vector3.Distance(go.transform.position, transform.position);
                if(neighbourDistance <= FlockManager.FM.neighbourDistance)
                {
                    vCenter += go.transform.position;   // add all fish pos together
                    groupSize++;

                    if(neighbourDistance < 1.0f)        // neighbour too close
                    {
                        vAvoid = vAvoid +(transform.position - go.transform.position);
                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    groupSpeed = groupSpeed + anotherFlock.speed;
                }
            }
        }

                                                            //  ~~ GOAL LOCATION & SPEED
        if(groupSize > 0)
        {
            vCenter = vCenter / groupSize + (FlockManager.FM.goalPos -transform.position);      //  goal
            speed = groupSpeed / groupSize;
            if (speed > FlockManager.FM.maxSpeed)                                                //  speed
            {
                speed = FlockManager.FM.maxSpeed;
            }


            Vector3 direction = (vCenter + vAvoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                        Quaternion.LookRotation(direction),
                                                        FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
    }
}
