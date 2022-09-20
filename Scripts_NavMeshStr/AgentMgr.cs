using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


// AgentMgr script attached to AgentMgr Empty GameObject



public class AgentMgr : MonoBehaviour
{
    //GameObject[] agents;
    List<NavMeshAgent> agents = new List<NavMeshAgent>();


    void Start()
    {
        GameObject[] a = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject go in a)
        {
            agents.Add(go.GetComponent<NavMeshAgent>());        }
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast mouse left click

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                foreach (NavMeshAgent a in agents)
                    a.SetDestination(hit.point);
            }
        }
    }
}
