using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class State                            //~~~~~~~~~~  BASE CLASS
{
    public enum STATE
        {
            IDLE, PATROL, PURSUE,ATTACK, SLEEP, SUPRISE
        };
        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        };

        public STATE name;          //  state enum
        protected EVENT stage;
        protected GameObject npc;
        protected NavMeshAgent agent;
        protected Animator anim;
        protected Transform player;
        protected State nextState;

        float visDist = 10.0f;
        float visAngle = 30.0f;
        float shootDist = 7.0f;
    float nonVisDist = 4.0f;
    float nonVisAngle = 20.0f; // from behind

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        {
            this.npc = _npc;
            agent = _agent;
            this.anim = _anim;
            this.player = _player;
            stage = EVENT.ENTER;
        }
                                                                // ~~~~~   THE BASE REFERENCE
        public virtual void Enter() { stage = EVENT.UPDATE; }
        public virtual void Update() { stage = EVENT.UPDATE;}
        public virtual void Exit() { stage = EVENT.EXIT;}

        public State Process()                                 //  ~~~~~   THE STATE PROCESS
        {
            if (stage == EVENT.ENTER) Enter();
            if (stage == EVENT.UPDATE) Update();
            if (stage == EVENT.EXIT)
            {
                Exit();
                return nextState;
            }
            return this;
        }
    
                                                                //  ~~~~   ACTIONS
    public bool CanSeePlayer()                                 //   ~~~~~  NPC Vision
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        // if distance = dir.magnitude
        if(direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist )
        {
            return true;
        }
        return false;
    }

    public bool CanBeSuprised()                                         //   ~~~~~  NPC Vision Behind
    {
        Vector3 direction =  npc.transform.position - player.position ; // swap order for angle
        float angle = Vector3.Angle(direction, npc.transform.forward);

        // if distance = dir.magnitude
        if (direction.magnitude < nonVisDist && angle < nonVisAngle)
        {
            return true;
        }
        return false;
    }
}


public class Idle : State                                       //  ~~~~~~  IDLE
{
    // Idle references Patrol State
    public Idle (GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base( _npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }


    public override void Update()
    {
        // trigger pursue
        if (CanSeePlayer())
        {
            nextState= new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if(Random.Range(0,100) < 10)
        {
            nextState = new Patrol(npc, agent, anim, player);
             stage = EVENT.EXIT;
        }
        // base.Update(); infinite loop: upSTATE:upBASE:upOVERRIDE. dO NOT CALL
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle"); // clear unused isIdle anim
        base.Exit();
    }
}

public class Patrol : State                                     //  ~~~~~~~~     PATROL
{
    // Patrol does not ref any other STATES
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;                //  only if it has a path to follow
        agent.isStopped = false;
    }

    public override void Enter()
    {
                                        //currentIndex = 0; // WayPoint #
        //   Return NPC to nearest Waypoint after Pursue
        //   Get index of closest WayPoint
        float lastDist = Mathf.Infinity;
        for(int i=0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if(distance < lastDist)
            {
                currentIndex = i - 1;         // entering update will ++i to correct wp
                lastDist = distance;
            }
        }
        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
                                                                    // KEEP AS IS >= -1   !!!
        {
            //  run thru waypoints
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }

        // trigger Pursue
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        // trigger Suprise
        if (CanBeSuprised())
        {
            nextState = new Suprise(npc, agent, anim, player);
            stage = EVENT.EXIT;

        }

    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}

public class Pursue: State
{

    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name= STATE.PURSUE;
        agent.speed = 5; // running
        agent.isStopped = false;
    }
    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        //  NavMesh may take 1 loop to pick up path
        //  use IF to wait/ confirm
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            // previous state was canseeplayer
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(npc, agent, anim, player);
                stage=EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }

}

public class Attack : State
{
    float rotationSpeed = 2.0f;
    AudioSource shoot;
    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        shoot = _npc.GetComponent<AudioSource>();
    }

    public override void Enter()
    {
        anim.SetTrigger("isShooting");
        //  Don't follow and shoot
        agent.isStopped = true;
        shoot.Play();
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        //  restrict tilt in X & Y
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                             Quaternion.LookRotation(direction),
                                             Time.deltaTime * rotationSpeed);

        if (!CanAttackPlayer())
        {
            // All states accessble from Idle > Patrol
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        shoot.Stop();
        base.Exit();
    }
}

public class Suprise : State
{
    GameObject safeCube;

    public Suprise(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.SUPRISE;
        safeCube = GameObject.FindGameObjectWithTag("SAFE");

    }
    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        agent.speed = 8;             // running
        agent.isStopped = false;    //  create path
        agent.SetDestination(safeCube.transform.position);      //  set location here
        base.Enter();
    }

    public override void Update()
    {
        //  Refact. Speed. Stability. 
        //  NavMesh needs one Loop to update.
        //  agent.SetDestination(GameObject.FindGameObjectWithTag("SAFE").transform.position);

        //  Set next state on approch to cube
        if(agent.remainingDistance < 1)
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }

}


