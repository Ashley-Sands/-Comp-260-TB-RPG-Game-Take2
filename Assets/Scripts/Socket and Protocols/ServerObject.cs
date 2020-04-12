using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerObject : MonoBehaviour
{

    private delegate void SelectServerObject ( int objectId, ISelectServerObject selectedServerObjectInterface );
    private static SelectServerObject selectServerObject;

    private bool inUse = false;
    [SerializeField] private Protocol.ServerObject.ObjectType serverObjectType;

    public int serverObjectId = -1;     // all ids should be >= 0
    private Vector3 lastPosition;
    private Vector3 lastRotation;

    public static void InvokeSelectObject( int objectId, ISelectServerObject selectedServerObjectInterface )
    {
        selectServerObject?.Invoke( objectId, selectedServerObjectInterface );
    }

    private void Awake ()
    {
        selectServerObject += SelectObject;

        Protocol.ProtocolHandler.Inst.Bind('#', UpdateServerObject);

    }

    public void PlayerInit( int playerId )
    {

        // if we are of player type, then the object id needs to be the player id
        if ( serverObjectType == Protocol.ServerObject.ObjectType.Player )
            serverObjectId = playerId;

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
        if ( serverObjectType != Protocol.ServerObject.ObjectType.Player && objectId == serverObjectId )
            selectedServerObjectInterface.SelectedObject = this;
    }

    /// <summary>
    /// Updates the objects position when the server says so. :D
    /// </summary>
    /// <param name="proto"></param>
    public void UpdateServerObject( Protocol.BaseProtocol proto )
    {


        Protocol.ServerObject obj = proto.AsType<Protocol.ServerObject>();

        if ( obj.Type != serverObjectType || obj.object_id != serverObjectId ) return;

        print( obj.Action +"=="+ Protocol.ServerObject.ObjectAction.Destroy + " && " + obj.Type + "==" + serverObjectType + " && " + obj.object_id + "==" + serverObjectId );
        if ( obj.Action == Protocol.ServerObject.ObjectAction.Destroy )
        {
            Destroy( gameObject );
        }
        
        if ( obj.Action != Protocol.ServerObject.ObjectAction.Defualt )
        {
            return;
        }

        // stop the nav agent if present.
        ClientAgent agent = GetComponent<ClientAgent>();

        Debug.LogError( name + " :: "+ (agent != null) +" && "+ !agent.FindingPath +" && "+ agent.Naving );

        if ( agent != null && !agent.FindingPath && agent.Naving )
        {
            print( "Stop Agent." );
            agent?.CancelAction();
        }

        print( "TAG " + gameObject.tag + " transform.position " + transform.position );

        // update the position of the object.
        transform.position = lastPosition = obj.Position;
        transform.eulerAngles = lastRotation = obj.Rotation;

    }

    /// <summary>
    /// Sends the ServerObject Protocol for this object to the server :)
    /// </summary>
    public virtual void Send( bool newObject = false)
    {

        if ( !newObject && transform.position == lastPosition && transform.eulerAngles == lastRotation) return;

        Protocol.ServerObject.ObjectAction soACtion = newObject ? Protocol.ServerObject.ObjectAction.Add : Protocol.ServerObject.ObjectAction.Defualt;

        Protocol.ServerObject obj = new Protocol.ServerObject( transform.position, transform.eulerAngles, serverObjectType, serverObjectId, soACtion );
        obj.Send();

        lastPosition = transform.position;
        lastRotation = transform.eulerAngles;

    }

    private void OnDestroy ()
    {
        selectServerObject -= SelectObject;
        Protocol.ProtocolHandler.Inst.Unbind('#', UpdateServerObject);
    }
}
