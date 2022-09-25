using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    //  Add to Spawner GObj
    public GameObject patientPrefab;
    public int numPatients;

    void Start()
    {
       for ( int i = 0; i < numPatients; i++)
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
        }

        Invoke("SpawnPatient", 15);
    }

    void SpawnPatient()
    {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke("SpawnPatient", Random.Range(2,10));


    }


    void Update()
    {
        
    }
}
