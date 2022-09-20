using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public sealed class GameEnvironment 
{
private static GameEnvironment instance;
    private List<GameObject> checkpoints = new List<GameObject> ();
    public List<GameObject> Checkpoints { get { return checkpoints; } }




    // SINGLETON
    // Grab Checkpoints. Call directly from states.
    public static GameEnvironment Singleton
    {
        get
        {
            if(instance == null)
            {
                instance = new GameEnvironment ();

                //WayPoints by Tag.Random order
                 instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));


                // WayPoints by Set Order (Linq)
                // GameObj waypoint
                // alphabetical names
                 //instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList ();
            }
            return instance;
        }
    }

}
