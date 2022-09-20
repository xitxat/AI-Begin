using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiContBasic : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject target;   // Jane

    Animator anim;              // Animate while walking




    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
        // check AI progress along move path
        // will slide to stop if dist is too far
        if(agent.remainingDistance < 2)
        {
            // Set Idle animation
            // bool set in Character's Animator Component Controller slot.
                //  Open Ani window, in Idle to Fwd arrow : Condition
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }


    }
}
