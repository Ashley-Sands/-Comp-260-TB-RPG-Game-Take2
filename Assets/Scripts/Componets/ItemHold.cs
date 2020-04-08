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

    private void Update ()
    {

        if ( currentHoldObject == null ) return;
        
        currentHoldObject.transform.position = transform.position + holdOffset;

    }

    public void CollectItem( int object_id )
    {
        currentHoldObject?.Use( false );    // un use the object if we have one

        ServerObject.InvokeSelectObject( object_id, this );

        currentHoldObject?.Use( true );     // use the object if we found one.

        clientManager.CompleatAction();

    }

    public void DropItem()
    {
        currentHoldObject = null;
    }

}
