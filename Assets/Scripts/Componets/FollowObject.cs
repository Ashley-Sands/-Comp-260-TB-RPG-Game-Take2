using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;

    void Update()
    {
        if ( objectToFollow != null )
            transform.position = objectToFollow.position;    
    }
}
