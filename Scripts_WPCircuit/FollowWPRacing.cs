using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWPRacing : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;

    public float speed = 10f;
    public float rotSpeed = 2f;
    public float turningCircle = 13f;

    GameObject tracker;
    public float trackerSpeed = 0.1f;
    public float lookAhead = 10f;


    void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;    
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    void ProgressTracker()
    {
        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead) return;

        // Tanks follow this obj. Will notget stuck at WPs
        if (Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 3)
            currentWP++;

        // reset WP if at last WP
        if (currentWP >= waypoints.Length)
            currentWP = 0;

        tracker.transform.transform.LookAt(waypoints[currentWP].transform);
        tracker.transform.Translate(0, 0, (speed + 2) * Time.deltaTime);        // trackerSpeed

    }


    // Update is called once per frame
    void Update()
    {
        ProgressTracker();

        //  Slerp rotate
        //  setup vector

        Quaternion lookatWP = Quaternion.LookRotation(tracker.transform.position - this.transform.position);
        // rotate a small amt of angle
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

        //  move
        this.transform.Translate(0, 0, speed * Time.deltaTime);

    }
}
