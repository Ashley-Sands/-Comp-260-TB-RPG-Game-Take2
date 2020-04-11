using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnServerObject : MonoBehaviour, ISelectServerObject
{

    [SerializeField] private ServerObject blockPrefab;
    public ServerObject SelectedObject { get; set; }

    void Start()
    {
        Protocol.ProtocolHandler.Inst.Bind( '#', SpawnObject );    
    }

    private void SpawnObject( Protocol.BaseProtocol proto )
    {

        Protocol.ServerObject servObj = proto.AsType<Protocol.ServerObject>();

        if ( servObj.Action != Protocol.ServerObject.ObjectAction.Add ) return;

        // check that the object does not not exist.
        SelectedObject = null;
        ServerObject.InvokeSelectObject( servObj.object_id, this );

        if ( SelectedObject != null )
        {
            Debug.LogError( "Unable to spwan object, already exist :( " );
            return;
        }

        // find the object to spawn and spawn it :D
        ServerObject spawnObj = null;

        switch( servObj.Type )
        {
            case Protocol.ServerObject.ObjectType.Block:
                spawnObj = blockPrefab;
                break;
            default:
                Debug.LogErrorFormat( "Unable to spwan server object of type {0}",  servObj.Type );
                return;
        }

        if ( spawnObj != null )
        {
            Quaternion quat = Quaternion.identity;
            quat.eulerAngles = servObj.Rotation;

            ServerObject so = Instantiate( spawnObj, servObj.Position, quat );
            so.serverObjectId = servObj.object_id;

        }
    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( '#', SpawnObject );
    }
}
