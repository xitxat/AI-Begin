using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveTyre : MonoBehaviour
{
    //      THIS SCRIPT PLACED ON WHEEL COLLIDERS
    // NB   Rotate Wheels
    //      Setup master collider list with the wheel colliders
    //      empty inside car, copy wheel rotatable sub mesh, move to new collider folder
    //      drag rotatable sub wheel mesh and drop into GObj Colliders corresponding slot..
    //          REMOVE mesh renderer and mesh filter from new colliders


    public WheelCollider WC;
    public GameObject wheelMesh; // for collider slot
    public float maxTorque = 200;  // motor speed?
    public float maxSteerAngle = 60;
    public float maxBrakeTorque = 500;
    public bool canSteer = false;


    // Start is called before the first frame update
    void Start()
    {
        WC = GetComponent<WheelCollider>();

    }

    //                                                         ~~~~~~~ GO
    public void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -1 , 1);                     // noralised w clamp

        float thrustTorque = accel * maxTorque;
        WC.motorTorque = thrustTorque;

        if (canSteer)
        {
            steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;  // in IF for SPEED
            WC.steerAngle = steer;
        }
        //  Brake activation
        else
        {
            brake = Mathf.Clamp(brake, -1, 1) * maxBrakeTorque;
            //  apply brake to back wheels
            WC.brakeTorque = brake;
        }




        // attach and Move the wheel mesh
        Quaternion quat;
        Vector3 position;
        WC.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;                 // rotate wheel mesh
        wheelMesh.transform.rotation = quat;

    }


    //void Update()
    //{
    //    float a = Input.GetAxis("Vertical");
    //    float s = Input.GetAxis("Horizontal");
    //    Go(a, s); 
    //}
}
