using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHold : MonoBehaviour, ISelectServerObject
{

    [SerializeField] private ClientManager clientManager;

    public ServerObject SelectedObject {
        get => currentHoldObject;
        set => currentHoldObject = value;
    }

    private ServerObject currentHoldObject;
    [SerializeField] private Vector3 holdOffset;
    [Tooltip("ui to display when i player has an item.")]
    [SerializeField] private GameObject[] toggleOnUi; 
    [SerializeField] private GameObject[] toggleOffUi;

    private void Awake ()
    {
        GameCtrl.Inst.gameLoopEvent += TurnEnded;
    }

    private void Update ()
    {

        if ( currentHoldObject == null ) return;

        Vector3 offset = (transform.right * holdOffset.x) + (transform.up * holdOffset.y) + (transform.forward * holdOffset.z);
        
        currentHoldObject.transform.position = transform.position + offset;

    }

    public void CollectItem( int object_id )
    {
        currentHoldObject?.Use( false );    // un use the object if we have one

        ServerObject.InvokeSelectObject( object_id, this );

        currentHoldObject?.Use( true );     // use the new object if we found one.

        clientManager.CompleatAction();

        ToggleUI( toggleOnUi, currentHoldObject != null );
        ToggleUI( toggleOffUi, !(currentHoldObject != null) );
    }

    public void DropItem(bool compleatAction = true)
    {

        Vector3 offset = new Vector3( 0, holdOffset.y, 0 );

        if ( currentHoldObject != null)
            currentHoldObject.transform.position = currentHoldObject.transform.position - offset;

        currentHoldObject?.Use( false );    // un use the object if we have one
        currentHoldObject = null;

        if (compleatAction)
            clientManager.CompleatAction();

        ToggleUI( toggleOnUi, !(currentHoldObject == null) );
        ToggleUI( toggleOffUi, currentHoldObject == null );

    }

    private void TurnEnded( Protocol.GameLoop.Actions action, int ttl )
    {
        // Now that the players turn is over we will have to directly drop the item
        // and send the message to the sever bypassing the queue as is now not active.
        if ( currentHoldObject != null ) // dorp it! :D
        {
            DropItem(false);
            Protocol.GameAction gameAct = new Protocol.GameAction( Protocol.GameAction.Actions.DropItem );
        }

    }

    private void ToggleUI( GameObject[] gos, bool active)
    {
        print( active + " :: " + toggleOnUi.Length );
        if ( gos.Length > 0 )
            print( active + "'''''@@@@@@@@@@@@@" );
            foreach ( GameObject go in gos )
                go.SetActive( active );
    }

    private void OnDestroy ()
    {
        GameCtrl.Inst.gameLoopEvent -= TurnEnded;
    }
}
