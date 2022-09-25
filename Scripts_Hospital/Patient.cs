using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Multi Patient Spawner:
//      Patient has complete set of scripts.
//      Update PreFab > Select GO, Insptr Overrides, Apply
//      Drop Patient GO onto Spawner Scr slot

//  All Characters Doc, NUrse, Patient, Chook etc need :  protected override void Start()

// atHospital (getTreated) preCondition is like a standalone action / state / belief
        // injected in GoToWaiting

// BELIEF STATES:       Injected via pre post conditions & belief state in previous script
        //  isCured     added to preCon of GoHome
        //              isCured from GetTreated

public class Patient : GAgent
{

    protected override void Start()
    {
        base.Start();

        // WORLD STATES
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("isTreated", 1, true);         // 5 bc. its more important
        goals.Add(s2, 5);                                   //  have to go thru isWaiting first

        SubGoal s3 = new SubGoal("isHome", 1, true);
        goals.Add(s3, 5);
    }

}
