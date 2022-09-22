using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;

    float speedMult;
    float detectionRadius = 20;
    float fleeRadius = 10;


    void Start() {

        agent = GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        int i = Random.Range(0, goalLocations.Length);
        agent.SetDestination(goalLocations[i].transform.position);
        anim = this.GetComponent<Animator>();

        // Adjust walking anim
        // Hier. Char. Animator Controler. Walking Box. Parameters - add Float wOffset. Inspector. Cycle Offset + param.
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));

        ResetAgent();

    }

    void ResetAgent(){
        // Speed Multiplyer
        speedMult = Random.Range(0.1f, 1.4f);
        anim.SetFloat("speedMult", speedMult);
        agent.angularSpeed = 120;                 //    turn
        agent.speed *= speedMult;

        anim.SetTrigger("isWalking");
        agent.ResetPath();                       //   NavMesh Command. Stop and reset.
    }

    public void DetectNewObstacle(Vector3 position)                             //  ~~~~    FLEE
    {
        //  Connected to the DropCylinder scr + on mouse click


        if(Vector3.Distance(position, transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (transform.position - position).normalized;
            Vector3 newGoal = transform.position + fleeDirection * fleeRadius;  // fleeRadius: dist to run

            NavMeshPath path = new NavMeshPath();                               // auto get Wayponts & put in array "corners"
            agent.CalculatePath(newGoal, path);

            // Check if a path can be constructed a 2 b
            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);    // -1 goal WP location
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }

    void Update() {

        if (agent.remainingDistance < 1.0f) {

            ResetAgent();
            int i = Random.Range(0, goalLocations.Length);
            agent.SetDestination(goalLocations[i].transform.position);



        }
    }
}