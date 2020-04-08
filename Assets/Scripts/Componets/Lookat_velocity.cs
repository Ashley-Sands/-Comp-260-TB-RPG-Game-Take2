using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody) )]
public class Lookat_velocity : MonoBehaviour
{

    private Rigidbody rb;

    private void Awake ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.LookAt( transform.position + rb.velocity );
    }

}
