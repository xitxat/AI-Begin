
using UnityEngine;


//  1. Nurse finds a Cubicle.
//  2. Nurse finds Patient.
//  3. Nurse gives Patient access to Cube.
//      Both have same Cube in Inventory.
//      Allowing both to move to the same Cube.


public class GetPatient : GAction
{
    // Get Static GameObj. ie. Cubicle, to put into Inventory
    GameObject resource;


    public override bool PrePerform()
    {
        //  Pick up patients
        target = GWorld.Instance.RemovePatient();

        //  Recycle plan on fail
        if(target == null)
            return false;

        // First get Target then Cube
        // Setup Pointer to First Cube in Queue
        resource = GWorld.Instance.RemoveCubicle();
        if(resource != null)
            //  Add to Nurses inventory
            inventory.AddItem(resource);
            else
            {   //  if No Cube then release Patient
                GWorld.Instance.AddPatient(target);
                target = null;
                return false;
            }

        //  We have a Cube
        // Now reduce the availability of Cubes by value -1. 
        //  Update world

        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", -1);
        return true;
    }
                                                //  ~~~~ #  ~~~  \\
    public override bool PostPerform()
    {
        // Take Away a waiting Patient
        // Add Cubicle to Patients inventory.
        GWorld.Instance.GetWorld().ModifyState("Waiting", -1);
        if(target)
            target.GetComponent<GAgent>().inventory.AddItem(resource);
        return true;
    }
}
