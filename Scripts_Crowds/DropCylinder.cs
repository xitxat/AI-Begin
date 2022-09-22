using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//      Tag all agents to Run away
//      MUST attach to an object in the scene. ie Camera

public class DropCylinder : MonoBehaviour
{
    public GameObject obstacle;
    GameObject[] agents;


    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation); // Instantiate needs Rotation
               foreach(GameObject a in agents)
                {
                    // Send position of Cylinder obj to the agents.
                    a.GetComponent<AIControl>().DetectNewObstacle(hitInfo.point);
                }

            }
        }
    }
}
