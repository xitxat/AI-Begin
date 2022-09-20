using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
Rigidbody rb;
    float lastTimeChecked;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void RightCar()
    {
        //  move up a bit so ato not be underground
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }
    // Update is called once per frame
    void Update()
    {
        // Y still good or still moving
        if(transform.up.y > 0.5f || rb.velocity.magnitude > 1)
        {
            lastTimeChecked = Time.time;
        }

        if(Time.time > lastTimeChecked + 3)
        {
            RightCar();
        }
    }
}
