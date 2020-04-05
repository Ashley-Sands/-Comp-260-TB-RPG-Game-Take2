using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_base : MonoBehaviour
{

    [SerializeField] protected PlayerManager playerManager;

    public void MovePlayer()
    {
        Protocol.MovePlayer movePlayer = new Protocol.MovePlayer()
        {
            Position = playerManager.HitLocation.location
        };

        playerManager.QueueAction( movePlayer );

    }

}
