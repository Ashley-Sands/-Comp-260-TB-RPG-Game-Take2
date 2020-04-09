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

        // up date the position on the server.
        Protocol.QueueServerObject serverObj = new Protocol.QueueServerObject( transform.position, Protocol.ServerObject.ObjectType.Player, GameCtrl.Inst.playerData.playerId );
        playerManager.QueueAction( serverObj );

    }

}
