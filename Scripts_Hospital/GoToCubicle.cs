
using UnityEngine;


// For anyone who needs to go to the Cube
// Target the Cubicle
// THIS person Updates the Shared booth. Not both. Just this.Cube boss

public class GoToCubicle : GAction
{
    public override bool PrePerform()
    {
        // Get target from Inventory
        target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;
        return true;
    }


    // Communicate to the World State
    public override bool PostPerform()
    {
        //   Add a state - "LayingEggs"บบ Egg numbers
        GWorld.Instance.GetWorld().ModifyState("TreatingPatient", 1);

        //  Cube boss
        //  Give back a Cubicle to the World. 
        GWorld.Instance.AddCubicle(target);

        //  Remove Cubicle fron agents Inventory
        inventory.RemoveItem(target);

        //  Cube boss
        //  Set state of the world.
        //  Free Cubicle Status. Update Cube Count: Add 1. 
        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", 1);
        return true;
    }
}
