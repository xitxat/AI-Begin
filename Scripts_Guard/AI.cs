using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI : MonoBehaviour
{

    NavMeshAgent agent;
    Animator anim;
    public Transform player; //     Add FPS controller/ Core Player Object  to this slot (not top lev nonEmpty GO)
    State currentState;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(gameObject, agent, anim, player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process(); // call to auto set next state from State Process update()
                                               // eg: 10% chance that next state is Patrol

    }
}
