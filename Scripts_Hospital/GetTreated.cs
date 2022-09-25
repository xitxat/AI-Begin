using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Put on Patient

public class GetTreated : GAction
{
    public override bool PrePerform()
    {
        // Get target from Inventory
       target = inventory.FindItemWithTag("Cubicle");

        if (target == null)
            return false;
        return true;
    }



    public override bool PostPerform()
    {
        // Communicate to the World State
        //   Add a state บบ Egg numbers
        GWorld.Instance.GetWorld().ModifyState("Treated", 1);

        // Communicate to the Internal Belief State
        beliefs.ModifyState("isCured", 1);


        //  Remove Cubicle fron agents Inventory
        inventory.RemoveItem(target);

        return true;
    }
}
