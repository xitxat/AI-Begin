using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//                                          ~~~~    REFERENCING Drive.cs (input controller)


public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public float bumpUp = 5.0f;     //  lookAhead multiplyer
    Drive ds;                       // ref the Drive contrlr script


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ds = target.GetComponent<Drive>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);

        // or SEEK chickens
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position - fleeVector);
    }

    void Pursue()                                                   //  ~~~~    ~Location Prediction
    {
        Vector3 targetDir = target.transform.position - transform.position;

        // Tight Angles with TransformVector
        // Put both into same relative space
        //  Get angle
        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));


        // Conditions for SEEK: moving, in front, behind, angle degree btwn vectors, 
        // Trnslation
        if ((toTarget >90 && relativeHeading <20) ||  ds.currentSpeed < 0.01f)   // beware of 0 FLOAT rounding errors
        {
            Seek(target.transform.position);
            return;
        }

        // how far to look infront of the target                                    
        float lookAhead = targetDir.magnitude/(agent.speed + ds.currentSpeed);
        Seek(target.transform.position + target.transform.forward*lookAhead ); // fwd auto normalised to 1, * bumpUp
    }

    void Evade()
    {
        //   same as PURSUE but replace Seek() w. Flee()
        Vector3 targetDir = target.transform.position - transform.position;

        float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead); // fwd auto normalised to 1, * bumpUp

    }

    //  Wander using a circle  + cirum jitter point
    // wanderTaarget needs to be nonlocal updated here

    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;    // make sure NavMesh is big enough
        float wanderDistance = 8;  // make smaller than RADIUS. smoothness
        float wanderJitter = 5;    // randomness

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                       Random.Range(-1.0f, 1.0f) * wanderJitter);
        // move jitterPOint back onto circle
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3( 0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);

    }

    void Hide()
    {
        //fIND THE CLOSEST SPOT
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for(int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            //   distance from cop to tree
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;

            //  Position of tree. +  a bit more to go behind it.
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 5;

            // Is next tree closer?
            if(Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Seek(chosenSpot);
    }

    void CleverHide()
    {
        //fIND THE CLOSEST SPOT: to the backside of an objects collider.
        // Turn the exiting Ray around to get exit spot
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0]; // init to 1st obj

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            //   distance from cop to tree
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;

            //  Position of tree. +  a bit more to go behind it.
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 15;   // check this val for testing

            // Is next tree closer?
            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        // Raycast back of Collider.
        Collider hideCol = chosenGO.GetComponent<Collider>();   // not performant
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit  info;
        float distance = 100.0f;                                // val must be bigger than hidePos multiplyer ^
        // get backside hit and store in info
        hideCol.Raycast(backRay, out info, distance);


        // Seek(chosenSpot); // used for HIde()
        Seek(info.point + chosenDir.normalized * 5);
    }
    
    // bot will hide if it can see Purserer. Sneak up on me.
    bool ICanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - transform.position;

        // Calc Angles:  this obj LOS - dir to target 
        float lookAngle = Vector3.Angle(transform.forward , rayToTarget);

        // Hide if within angle and Rayhit
        if(lookAngle < 60 && Physics.Raycast(transform.position, rayToTarget, out raycastInfo))
        {
            // if hit the correct Tag / seems like can't see thru objects.
            if (raycastInfo.transform.gameObject.tag == "cop")
                return true;
        }
        return false;

        // turn a bit
        //this.transform.Rotate(0, 60, 0, Space.Self);

    }

    bool CanSeeMe()
    {
        Vector3 rayToTarget = transform.position - target.transform.position ;

        // Calc Angles:  this obj LOS - dir to target 
        float lookAngle = Vector3.Angle(target.transform.forward, rayToTarget);

        // IS OTHER looking at me. EVASIVENESS
        if (lookAngle < 60 )
        {
                return true;
        }
        return false;
    }

    bool coolDown = false;
    void BehaviourCooldown()
    {
        coolDown = false;
    }

    bool IsTargetInRange()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 10)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        //  Seek(target.transform.position);
        //  Flee(target.transform.position);
        //  Pursue();
        //  Evade();
        //  Wander();
        //  Hide();



                        //  if cooling down / hiding
        if (!coolDown)  //  moving
        {
            if (!IsTargetInRange())
            {
                Wander();
            }

            else if (ICanSeeTarget() && CanSeeMe())
            {
                CleverHide();
                coolDown = true;
                //  delay
                Invoke("BehaviourCooldown", 5); // inactive hide time
            }
            else
                Pursue();
        }



    }
}
