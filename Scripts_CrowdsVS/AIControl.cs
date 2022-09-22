using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{

    public GameObject goal;
    NavMeshAgent agent;


    void Start()
    {
    agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(goal.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
