using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add box colider to car as trigger
public class AvoidDetector : MonoBehaviour
{
    public float avoidPath = 0;
    public float avoidTime = 0;
    public float wanderDistance = 4;
    public float avoidLength = 1;
    public bool reverse = false;
    Rigidbody rb;

 void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerExit(Collider col)
    {
        reverse = false;
        if (col.gameObject.tag != "car") return;
        avoidTime = 0;
    }

    private void OnTriggerStay(Collider col)
    {
        // REVERSE
        // Determine if Car just Hit is infront / behind
        // +z & z = fwd
        Vector3 collisionDir = transform.InverseTransformPoint(col.gameObject.transform.position);

        if(collisionDir.x > 0 &&  collisionDir.z > 0)
        {
            if(rb.velocity.magnitude < 1 ) reverse = true;


            else if (col.gameObject.tag != "car")
                {
                    Rigidbody otherCar = col.GetComponent<Rigidbody>();
                    avoidTime = Time.time + avoidLength;

                    Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
                    float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
                    // turn away
                    avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle);
                }
        }


    }
}
