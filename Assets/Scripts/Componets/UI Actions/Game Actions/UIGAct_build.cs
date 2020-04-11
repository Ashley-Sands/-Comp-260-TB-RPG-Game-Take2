using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_build : UIGAct_base
{
    public void Build()
    {

        // move to the location.

        Protocol.MovePlayer movePlayer = new Protocol.MovePlayer()
        {
            Position = playerManager.HitLocation.location
        };

        playerManager.QueueAction( movePlayer );

        // que build object.

        Protocol.BuildObject buildObj = new Protocol.BuildObject()
        {
            Type = Protocol.ServerObject.ObjectType.Block
        };

        playerManager.QueueAction( buildObj );

    }
}
