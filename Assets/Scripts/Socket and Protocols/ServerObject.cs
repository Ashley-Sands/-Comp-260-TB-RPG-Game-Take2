using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObject : MonoBehaviour
{

    private delegate void SelectServerObject ( int objectId, ISelectServerObject selectedServerObjectInterface );
    private static SelectServerObject selectServerObject;

    private bool inUse = false;
    [SerializeField] private Protocol.ServerObject.ObjectType serverObjectType;

    public int serverObjectId;
    private Vector3 lastPosition;

    public static void InvokeSelectObject( int objectId, ISelectServerObject selectedServerObjectInterface )
    {
        selectServerObject?.Invoke( objectId, selectedServerObjectInterface );
    }

    private void Awake ()
    {
        selectServerObject += SelectObject;
    }

    private void Start ()
    {
        Protocol.ProtocolHandler.Inst.Bind('#', UpdateServerObject);
    }

    public virtual void Use ( bool use )
    {
        inUse = use;

        if ( !inUse )   // ie the object has been droped.
        {
            Send();
        }

    }

    public virtual void SelectObject( int objectId, ISelectServerObject selectedServerObjectInterface )
    {
        if ( objectId == serverObjectId )
            selectedServerObjectInterface.SelectedObject = this;
    }

    /// <summary>
    /// Updates the objects position when the server says so. :D
    /// </summary>
    /// <param name="proto"></param>
    public void UpdateServerObject( Protocol.BaseProtocol proto )
    {

        Protocol.ServerObject obj = proto.AsType<Protocol.ServerObject>();

        if ( obj.Type != serverObjectType || obj.object_id != serverObjectId )
            return;

        // update the position of the object.
        transform.position = lastPosition = obj.Position;


    }

    /// <summary>
    /// Sends the ServerObject Protocol for this object to the server :)
    /// </summary>
    public virtual void Send()
    {
        if ( transform.position == lastPosition ) return;

        Protocol.ServerObject obj = new Protocol.ServerObject( transform.position, serverObjectType, serverObjectId );
        obj.Send();

        lastPosition = transform.position;

    }

    private void OnDestroy ()
    {
        selectServerObject -= SelectObject;
        Protocol.ProtocolHandler.Inst.Unbind('#', UpdateServerObject);
    }
}
