using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Rigidbody ) )]
public class ApplyForce_onStart : MonoBehaviour
{
    [SerializeField] private float startForce;
    void Start()
    {

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForce( transform.forward * startForce );

    }


}
