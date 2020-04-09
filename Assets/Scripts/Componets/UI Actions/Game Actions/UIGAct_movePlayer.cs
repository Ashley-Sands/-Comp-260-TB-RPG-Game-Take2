using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_movePlayer : UIGAct_base
{
    public void MovePlayer ()
    {
        Protocol.MovePlayer movePlayer = new Protocol.MovePlayer()
        {
            Position = playerManager.HitLocation.location
        };

        playerManager.QueueAction( movePlayer );

    }

}
