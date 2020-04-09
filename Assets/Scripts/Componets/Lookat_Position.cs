using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat_Position : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    private bool isSet = false;
    private Vector3 LocationToLookAt;
    [SerializeField] private float rotateSpeed = 45f;         // deg
    [SerializeField] private float compleatRange = 5;   // deg

    void Update()
    {
        if ( !isSet ) return;

        Vector3 lookAtPosition = LocationToLookAt;
        lookAtPosition.y = transform.position.y;

        float rotSpeed = ( rotateSpeed * Mathf.Deg2Rad ) * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards( transform.forward, ( lookAtPosition - transform.position ), rotSpeed, 0 ) );

        float angle = Mathf.Atan2( lookAtPosition.x, lookAtPosition.z ) * Mathf.Rad2Deg;

        if ( angle < compleatRange )
        {
            playerManager.CompleatAction();
            isSet = false;
        }
    }

    public void LookAtObject( Vector3 pos )
    {
        LocationToLookAt = pos;
        isSet = true;
    }

}
