using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Add to Person
//  Person also needs its own script.
//  Ref.GAction.cs exposed vars.
//  Action Name entered in inspector.
//  Target GO can be dropped into slot.
//  OR
//  Via Tag eg "Door" (1st. del target slot).


public class GoToHospital : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }
}
