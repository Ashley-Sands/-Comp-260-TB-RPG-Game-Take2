using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObject : MonoBehaviour
{

    private bool inUse = false;
    [SerializeField] private Protocol.ServerObject.ObjectType serverObjectType;
    private int serverObjectId = -1;
    private Vector3 lastPosition;

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
        Protocol.ProtocolHandler.Inst.Unbind('#', UpdateServerObject);
    }
}
