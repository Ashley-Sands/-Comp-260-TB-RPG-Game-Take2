using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent( typeof( NavMeshAgent ) )]
public class ClientAgent : MonoBehaviour
{

    private NavMeshAgent agent;

    [SerializeField] private float moveSpeed = 5;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start ()
    {
        
    }

    private void Update ()
    {
        
    }

    private void GetHitLocation()
    {

    }

    private void MoveAgent( Protocol.BaseProtocol proto )
    {

    }

    private void OnDestroy ()
    {
        
    }

}
