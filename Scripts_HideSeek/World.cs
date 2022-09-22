using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World 
{
    private static readonly World instance = new World();
    private static GameObject[] hidingSpots;



    static World() 
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    private World() { }

    public static World Instance { get { return instance; } }

    // This GO gets the  hiding spots.
    // Agents ask  the World for them, from the world singleton.
    // Don't have to keep calling them.
    public  GameObject[] GetHidingSpots ()
    { 
        return hidingSpots; 
    }

}
