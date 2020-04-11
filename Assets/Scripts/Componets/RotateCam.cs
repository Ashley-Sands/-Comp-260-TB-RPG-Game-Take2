using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    [SerializeField] private Transform camHold;
    [SerializeField] private float mouseSpeed = 10;
    [SerializeField] private float rotateSpeed = 45;

    Vector3 lastMousePosition;

    Vector3 rotation = Vector3.zero;

    private void Start ()
    {
        rotation = camHold.eulerAngles;
    }

    private void Update ()
    {

        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

        if ( Input.GetMouseButtonDown( 2 ) )
            rotation = camHold.eulerAngles;

        if ( Input.GetMouseButton( 2 ) )  // middle button
        {
            rotation.y += ( mouseDelta.x / mouseSpeed ) * rotateSpeed * Time.deltaTime;
            rotation.x -= ( mouseDelta.y / mouseSpeed ) * rotateSpeed * Time.deltaTime;
            camHold.eulerAngles = rotation;
        }

        lastMousePosition = Input.mousePosition;
    }
}
