using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health) )]
public class KillPlayer : MonoBehaviour
{

    [SerializeField] private Transform[] unparentObjects;
    [SerializeField] private GameObject destroyObject;
    [SerializeField] private GameObject deadedObject;

    private void Start ()
    {
        GetComponent<Health>().HealthDepleated += Kill;
    }

    private void Kill()
    {

        for ( int i = 0; i < unparentObjects.Length; i++ )
            unparentObjects[i].parent = null;

        Vector3 position = transform.position;
        position.y = 1f;

        Instantiate( deadedObject, position, transform.rotation );

        Destroy( destroyObject );

    }

    private void OnDestroy ()
    {
        GetComponent<Health>().HealthDepleated -= Kill;
    }

}
