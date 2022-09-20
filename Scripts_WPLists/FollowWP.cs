using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    Transform goal;
    float speed = 10.0f;
    float accuracy = 2.0f;
    float rotSpeed = 2.0f;

    public GameObject wpManager;
    GameObject[] wps;       // wp system
    GameObject currentNode;
    int currentWP = 0;
    Graph g;


    void Start()
    {

        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;

        //   OBJECT START POSITION must natch this node location!!!
        currentNode = wps[0];

        // delay 2 secs then go
        // Invoke("GoToRuin", 2);

        // Speed run testing
        //Time.timeScale = 5;
    }

    //  Exposed in Button onClick
    public void GoToHeli()
    {
        // 
        g.AStar(currentNode, wps[1]);
        //  not counting indexes Start() array
        currentWP = 0;
    }


    public void GoToRuin()
    {
        // add in element WP #
        g.AStar(currentNode, wps[4]);
        currentWP = 0;
    }

    public void GoToHide()
    {
        // add in element WP #
        g.AStar(currentNode, wps[6]);
        currentWP = 0;
    }

    public void GoToCamp()
    {
        // add in element WP #
        g.AStar(currentNode, wps[3]);
        currentWP = 0;
    }


    void LateUpdate()
    {       // or at the end of path
        if (g.pathList.Count == 0 || currentWP == g.pathList.Count)
            return;

        // turn the NOde into  GAME  OBJ TO GET THE POSITION ,
        // the tanks position and test for accuracy
        if (Vector3.Distance(g.pathList[currentWP].getId().transform.position,
                                transform.position)< accuracy)
        {
            // when arrive @ WP update this Node that was just found
            currentNode = g.pathList[currentWP].getId();
            currentWP++;
        }

        if(currentWP < g.pathList.Count)
        {
            goal = g.pathList[currentWP].getId().transform;
            // use Y axis so tank doesnt tilt looking for WP
            // Slerp angle
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            // rotate an GO!
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
                                                        Quaternion.LookRotation(direction),
                                                        Time.deltaTime * rotSpeed);
            transform.Translate(0, 0, speed * Time.deltaTime);



        }
    }

}
