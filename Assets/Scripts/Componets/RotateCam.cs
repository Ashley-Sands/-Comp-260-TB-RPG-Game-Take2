using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    [SerializeField] private Transform camHold;
    [SerializeField] private float mouseSpeed = 10;
    [SerializeField] private float rotateSpeed = 45;

    Vector3 lastMousePosition;

    private void Update ()
    {

        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

        if ( Input.GetMouseButton( 2 ) )  // middle button
        {
            print( "fadsfadsfads" );
            Vector3 rot = camHold.eulerAngles;
            rot.y += ( mouseDelta.x / mouseSpeed ) * rotateSpeed * Time.deltaTime;
            rot.x += ( mouseDelta.y / mouseSpeed ) * rotateSpeed * Time.deltaTime;
            camHold.eulerAngles = rot;
        }

        lastMousePosition = Input.mousePosition;
    }
}
