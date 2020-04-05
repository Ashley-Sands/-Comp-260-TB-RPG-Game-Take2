using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent( typeof( NavMeshAgent ), typeof(ClientManager) )]
public class ClientAgent : ClientAction
{

    private ClientManager clientManager;
    private NavMeshAgent agent;

    [SerializeField] private float moveSpeed = 5;
    
    private Vector3 location = Vector3.zero;
    public bool complete = false;

    [SerializeField] private float completeRadius;

    void Awake()
    {
        clientManager = GetComponent<ClientManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update ()
    {

        if ( complete ) return;

        // ignore the distacne of the y axis
        Vector3 playerPos = transform.position;
        Vector3 targetPos = location;
        targetPos.y = playerPos.y;

        if ( Vector3.Distance( playerPos, targetPos) < completeRadius )
        {
            agent.isStopped = true;
            transform.position = targetPos;
            complete = true;
            // compleat the task.
            clientManager.CompleatAction();
        }

    }

    public void MoveAgent( Vector3 loc )
    {

        location = loc;
        complete = false;

    }

    public override void CancelAction ()
    {
        agent.isStopped = true;
    }

}
