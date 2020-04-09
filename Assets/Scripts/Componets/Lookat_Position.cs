using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat_Position : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    private bool isSet = false;
    private Vector3 LocationToLookAt;
    [SerializeField] private float rotateSpeed = 45f;   // deg
    [SerializeField] private float compleatRange = 5;   // deg

    void Update()
    {
        if ( !isSet ) return;

        Vector3 lookAtPosition = LocationToLookAt;
        lookAtPosition.y = transform.position.y;

        float rotSpeed = ( rotateSpeed * Mathf.Deg2Rad ) * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards( transform.forward, ( lookAtPosition - transform.position ), rotSpeed, 0 ) );

        float angle = Mathf.Atan2( -lookAtPosition.x, lookAtPosition.z ) * Mathf.Rad2Deg;

        float angleDif = Mathf.Abs( Mathf.Abs(transform.localEulerAngles.y) - Mathf.Abs(angle) );
        float angleDif2 = transform.localEulerAngles.y - angle;

        float angleDif_inv = ( angle + transform.localEulerAngles.y - 180f );

        if ( ( angleDif2 - transform.eulerAngles.y >= 0 && angleDif < compleatRange ) || ( angleDif2 - transform.eulerAngles.y < 0 && Mathf.Abs(180f - ( angleDif_inv )) < compleatRange ) )
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
