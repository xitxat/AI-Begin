using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRoll : MonoBehaviour
{
    public float antiRoll = 5000.0f;
    public WheelCollider wheelLFront;
    public WheelCollider wheelRFront;
    public WheelCollider wheelLBack;
    public WheelCollider wheelRBack;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void GroundWheels(WheelCollider WL, WheelCollider WR)
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        // ask wheel if it is hitting something ie touching the ground
        bool groundedL = WL.GetGroundHit(out hit);

        // adjust suspension Left
        if (groundedL)
            travelL = (-WL.transform.InverseTransformPoint(hit.point).y - WL.radius) / WL.suspensionDistance;

        bool groundedR = WR.GetGroundHit(out hit);
        // adjust suspension Right
        if (groundedR)
            travelR = (-WR.transform.InverseTransformPoint(hit.point).y - WR.radius) / WR.suspensionDistance;

        //  Anti Roll Force
        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
            rb.AddForceAtPosition(WL.transform.up * -antiRollForce, WL.transform.position);

        if (groundedR)
            rb.AddForceAtPosition(WR.transform.up * -antiRollForce, WR.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        GroundWheels(wheelLFront, wheelRFront);
        GroundWheels(wheelRBack, wheelRBack);
        
    }
}
