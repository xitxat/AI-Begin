using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : GAction
{
    public override bool PrePerform()
    {
        return true;
    }


    // Communicate to the World State
    public override bool PostPerform()
    {
        // Inject 1 Waiting Patient into the World
        GWorld.Instance.GetWorld().ModifyState("Waiting", 1);

        //  add itself to the queue
        GWorld.Instance.AddPatient(this.gameObject);


        //  Inject a state into the Agent's Belief System
        beliefs.ModifyState("atHospital", 1);           //  Needs to be added to Planner
        return true;
    }
}
