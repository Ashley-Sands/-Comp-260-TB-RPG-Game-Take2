using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_launchProjectile : UIGAct_base
{
    
    public void LaunchProjectile()
    {

        // TODO: rotation towards the client we ant to fire at :D

        Protocol.GameAction gameAct = new Protocol.GameAction( Protocol.GameAction.Actions.LaunchProjectile );

        playerManager.QueueAction( gameAct );

    }

}
