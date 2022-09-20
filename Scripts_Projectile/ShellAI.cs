using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellAI : MonoBehaviour
{
    public GameObject explosion;
    Rigidbody rb;


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "tank")
        {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // SPEED. link to shell's RB
        rb = GetComponent<Rigidbody>();
    }




    // Update is called once per frame
    void Update()
    {
        //   point shell along trajectory
        transform.forward = rb.velocity;
    }
}
