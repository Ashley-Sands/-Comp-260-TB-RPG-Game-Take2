using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_collectItem : UIGAct_base
{

    public void CollectObject ()
    {
        // make sure the player has selected an object that we can collect
        if ( playerManager.SelectedServerObject == null )
            return;

        // move to the object
        // then collect it :)

        Protocol.MovePlayer movePlayer = new Protocol.MovePlayer()
        {
            Position = playerManager.HitLocation.location
        };

        playerManager.QueueAction( movePlayer );


        Protocol.CollectItem collectObject = new Protocol.CollectItem()
        {
            object_id = playerManager.SelectedServerObject.serverObjectId
        };

        playerManager.QueueAction( collectObject );

    }

    public void DropObject()
    {

        if ( playerManager.SelectedServerObject != null )
            return;

        Protocol.GameAction gameAct = new Protocol.GameAction( Protocol.GameAction.Actions.DropItem );

        playerManager.QueueAction( gameAct );

    }

    public void MoveToAndDropObject ()
    {

        if ( playerManager.SelectedServerObject != null )
            return;

        Protocol.MovePlayer movePlayer = new Protocol.MovePlayer()
        {
            Position = playerManager.HitLocation.location
        };

        playerManager.QueueAction( movePlayer );


        Protocol.GameAction gameAct = new Protocol.GameAction( Protocol.GameAction.Actions.DropItem );

        playerManager.QueueAction( gameAct );

    }

}
