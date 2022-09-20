using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FollowWpNavMesh : MonoBehaviour
{
    // RED TANK UNITY NAV MESH
    // Place Go at start() node AND adjust for array # element (WP-1)
    // set WP destinations via WP objects or co ords
    // Connect button functions to obj

    public GameObject wpManager;
    GameObject[] wps;       // wp system
    GameObject currentNode;
    NavMeshAgent agent;




    void Start()
    {

        wps = wpManager.GetComponent<WPManager>().waypoints;

        //   OBJECT START POSITION must natch this node location!!!
        currentNode = wps[0];
        Time.timeScale = 5;

        agent = this.GetComponent<NavMeshAgent>();
    }


    //  Exposed in Button onClick
    public void GoToHeli()
    {
        // 
        //g.AStar(currentNode, wps[1]); 
        agent.SetDestination(wps[1].transform.position);

    }

    public void GoToRuin()
    {
        // add in element WP #
        //   g.AStar(currentNode, wps[4]);
        agent.SetDestination(wps[4].transform.position);

    }

    public void GoToHide()
    {
        // add in element WP #
        // g.AStar(currentNode, wps[6]);
        agent.SetDestination(wps[6].transform.position);

    }

    public void GoToCamp()
    {
        // add in element WP #
        //      g.AStar(currentNode, wps[3]);
        agent.SetDestination(wps[3].transform.position);

    }


    void LateUpdate()
    {      

    }

}
