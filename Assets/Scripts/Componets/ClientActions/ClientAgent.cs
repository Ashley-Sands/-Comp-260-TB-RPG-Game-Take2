using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent( typeof( NavMeshAgent ))]
public class ClientAgent : ClientAction
{

    [SerializeField] private ClientManager clientManager;
    private NavMeshAgent agent;
    
    private Vector3 location = Vector3.zero;
    public bool complete = false;

    public bool Naving => !complete && !agent.isStopped && agent.remainingDistance > 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        agent.stoppingDistance = 0; // stop instantly 

    }

    private void Update ()
    {

        if ( complete ) return;

        if ( !agent.pathPending && agent.remainingDistance == 0 )
        {
            agent.isStopped = true;
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
