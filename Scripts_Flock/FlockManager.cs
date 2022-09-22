using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  ATTACH TO EMPTY GO.
//  POSITION FM GO
//  DROP PREFAB INTO SLOT.
// GOAL FOOD LOCATION
public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;  // connect flock
    public GameObject fishPrefab;

    public int numFish = 20;
    public GameObject[] allFish;                    
    public Vector3 swimLimits = new Vector3(5,5,5); //  fISHTANK based off FlockSheepManager
    public Vector3 goalPos = Vector3.zero;

    [Header("Fish Settings")]
    [Range(0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;     // ignore those out of range
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;     


    void Start()
    {
        allFish = new GameObject[numFish];          //  Put all the instantiated fish here
        for (int i = 0; i < numFish; i++)          //  Pos of Fish relative to FlockSheepManager
                                                   //   Animals on LAND:  Random.Range(-swimLimits.y, swimLimits.y) replace with 0
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                            Random.Range(-swimLimits.y, swimLimits.y),
                                                            Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
        }
        FM = this;

        goalPos = transform.position;

    }


    // Update is called once per frame
    void Update()
    {
        //  Randomly move goal POS
        if(Random.Range(0,100) < 10)        //  % chance
        {
            goalPos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                        Random.Range(-swimLimits.y, swimLimits.y),
                                                                        Random.Range(-swimLimits.z, swimLimits.z));
        }

    }
}
