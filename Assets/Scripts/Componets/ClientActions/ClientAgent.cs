using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent( typeof( NavMeshAgent ))]
public class ClientAgent : ClientAction
{

    [SerializeField] private ClientManager clientManager;
    private NavMeshAgent agent;

    [SerializeField] private float moveSpeed = 5;
    
    private Vector3 location = Vector3.zero;
    public bool complete = false;

    [SerializeField] private float completeRadius;

    void Awake()
    {
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
        agent.isStopped = false;
        agent.SetDestination( loc );
        complete = false;

    }

    public override void CancelAction ()
    {
        agent.isStopped = true;
    }

}
