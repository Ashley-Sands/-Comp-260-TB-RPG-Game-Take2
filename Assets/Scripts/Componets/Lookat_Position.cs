using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat_Position : MonoBehaviour
{
    [SerializeField] private ClientManager playerManager;
    private bool isSet = false;
    private Vector3 LocationToLookAt;
    [SerializeField] private float rotateSpeed = 45f;   // deg
    [SerializeField] private float compleatRange = 5;   // deg

    void Update()
    {
        if ( !isSet ) return;

        float rotSpeed = ( rotateSpeed * Mathf.Deg2Rad ) * Time.deltaTime;
        Vector3 lookAtPosition = LocationToLookAt;
        lookAtPosition.y = transform.position.y;

        Vector3 targetDir = lookAtPosition - transform.position;
        float angle = Vector3.SignedAngle( targetDir, transform.forward, Vector3.up );

        transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards( transform.forward, ( lookAtPosition - transform.position ), rotSpeed, 0 ) );

        if ( Mathf.Abs( angle ) < compleatRange )
        {
            playerManager.CompleatAction();
            isSet = false;
        }

    }

    public void LookAtPosition( Vector3 pos )
    {
        LocationToLookAt = pos;
        isSet = true;
    }

}
