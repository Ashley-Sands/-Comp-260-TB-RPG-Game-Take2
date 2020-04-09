using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_launchProjectile : UIGAct_base
{
    


    public void LaunchProjectile()
    {

        // rotate to look at the object to attack.
        Protocol.LookAtPosition lookAtPos = new Protocol.LookAtPosition()
        {
            Position = playerManager.pressedMarker.position // SelectedObject.position
        };

        playerManager.QueueAction( lookAtPos );

        // attack
        Protocol.GameAction gameAct = new Protocol.GameAction( Protocol.GameAction.Actions.LaunchProjectile );

        playerManager.QueueAction( gameAct );

    }

}
