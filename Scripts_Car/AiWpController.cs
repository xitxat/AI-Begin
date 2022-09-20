using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  REFERENCING: DriveTyre, AntiRoll, AvoidDetector

public class AiWpController : MonoBehaviour
{
    [Header("Car Settings")]
    public float steeringSensitivity = 0.01f;
    public float lookAhead = 25;    // tracker distance
    public float maxTorque = 300;
    public float maxSteerAngle = 80;
    public float maxBrakeTorque = 1000;
    public float accelCornerMax = 15;
    public float brakeCornerMax = 10;
    public float accelVelocityThreshold = 20;
    public float brakeVelocityThreshold = 10;
    public float antiRoll = 5000;
    public float trackerSpeed = 30.0f; // min max hardcoded in ProgTrkr. linr 80


    DriveTyre[] dts;
    public Circuit circuit;
    Vector3 target;
    int currentWP = 0;
    Rigidbody rb;       // Car body
    public GameObject brakeLight;
    GameObject tracker;
        int currentTrackerWP = 0;
    AvoidDetector avoid; // get it script reference




    void Start()
    {
        // s plural components!: wheels and add to array
        dts = GetComponentsInChildren<DriveTyre>();
        target = circuit.waypoints[currentWP].transform.position;
        rb = GetComponent<Rigidbody>();

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;

        // bring in DriveTyre stuff
        avoid = GetComponent<AvoidDetector>();  // set it
        GetComponent<AntiRoll>().antiRoll = antiRoll;

        foreach(DriveTyre d in dts)
        {
            d.maxTorque = maxTorque;
            d.maxSteerAngle = maxSteerAngle;
            d.maxBrakeTorque = maxBrakeTorque;

        }

    }



    // Blank Look At Obj for car to follow              //   ~~~~~ Progress Tracker
        void ProgressTracker() 
        {
            Debug.DrawLine(transform.position, tracker.transform.position);
            if (Vector3.Distance(transform.position, tracker.transform.position) > lookAhead)
        {

            trackerSpeed -= 1.0f;
            if (trackerSpeed < 2.0f) trackerSpeed = 2;
            return;
        }
            // if too close to car
            if(Vector3.Distance(transform.position, tracker.transform.position) < lookAhead / 2.0f)
        {
            trackerSpeed += 1.0f;
            if (trackerSpeed > 30.0f) trackerSpeed = 30;

        }


        // lookAt: snap turn
        tracker.transform.LookAt(circuit.waypoints[currentTrackerWP].transform.position);
                 // move tracker
                 tracker.transform.Translate(0, 0, trackerSpeed * Time.deltaTime);    // (smooth T.dt) speed of tracker

            if (Vector3.Distance(tracker.transform.position, circuit.waypoints[currentTrackerWP]
                                        .transform.position) < 5)                   // Next Narker
            {
                currentTrackerWP++;
                if(currentTrackerWP >= circuit.waypoints.Length)
                    currentTrackerWP = 0;
            }

        } 


    // Update is called once per frame
        void Update()
        {
            ProgressTracker();
        //  car's target is the tracker
            target = tracker.transform.position;

            // set up angle
             //  get target relative to car
             Vector3 localTarget;    // set by avoidance time

        if (Time.time < avoid.avoidTime)
        {
            localTarget = tracker.transform.right * avoid.avoidPath;
        }
        else
        {
            localTarget = transform.InverseTransformPoint(target);
        }


            //float distanceToTarget = Vector3.Distance(target, transform.position);
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg; // convert to Degs

            // .mag to find if travelling fwd or bkwrd 
            float s = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(rb.velocity.magnitude);  // steering * by max steer angle

            // BRAKING WITH ANGLE
            float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
            float cornerFactor = corner / 90.0f;

            //  CORNER ACCELATION calcs
            float a = 1;    //  acceleration, normalised (clamped -1,1), * by max torque
                            // made public for testing - header
            if (corner > accelCornerMax && rb.velocity.magnitude > accelVelocityThreshold)
                //  corner accel
                a = Mathf.Lerp(0, 1, 1 - cornerFactor);

            //  Corner Braking Calcs
            float b = 0;    //  Brake normalised * by max brake torque
            if (corner > brakeCornerMax && rb.velocity.magnitude > brakeVelocityThreshold)
                b = Mathf.Lerp(0, 1, cornerFactor); // closer to 1 the harder the braking


        // Use if No Progress Tracker
        //  Brake
        //if (distanceToTarget < 10)
        //    {
        //        b = 0.5f;    //normalised
        //    }

        if (avoid.reverse)
        {
            a = -1 * a;
            s = -1 * s;
        }

            for (int i = 0; i < dts.Length; i++)
            {
                // make Go() in DriveTyre public
                dts[i].Go(a, s, b);
            }

            //   BRAKE LIGHT
            if (b > 0)
            {
                brakeLight.SetActive(true);
            }
            else
            {
                brakeLight.SetActive(false);
            }

            // Use if No Progress Tracker
            // Go to next Wpoint
            //if (distanceToTarget < 3)
            //{
            //    currentWP++;
            //    if (currentWP >= circuit.waypoints.Length)
            //        currentWP = 0; ;

            //    // go back to start
            //    target = circuit.waypoints[currentWP].transform.position;
            //}



        }   // update


   

}  