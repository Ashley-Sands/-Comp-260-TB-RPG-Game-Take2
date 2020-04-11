using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health) )]
public class KillPlayer : MonoBehaviour
{

    [SerializeField] private Transform playerCamHold;
    [SerializeField] private GameObject deadedObject;

    private void Start ()
    {
        GetComponent<Health>().HealthDepleated += Kill;
    }

    private void Kill()
    {

        if (playerCamHold)
            playerCamHold.parent = null;

        Vector3 position = transform.position;
        position.y = 1f;

        Instantiate( deadedObject, position, transform.rotation );

        Destroy( gameObject );

    }

    private void OnDestroy ()
    {
        GetComponent<Health>().HealthDepleated -= Kill;
    }

}
