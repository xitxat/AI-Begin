using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ALL inheriting from GAgent need:
//          protected override void Start()

public class Nurse : GAgent
{

    protected override void Start()

    {
        base.Start();

        SubGoal s1 = new SubGoal("treatPatient", 1, false);
        goals.Add(s1, 3);
    }

}
