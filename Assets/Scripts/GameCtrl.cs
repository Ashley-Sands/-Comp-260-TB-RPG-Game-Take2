using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton GameCtrl
/// </summary>
public class GameCtrl : MonoBehaviour
{

    public event System.Action<Client[]> gameClientsSet;

    private static GameCtrl inst;
    public static GameCtrl Inst {
        get { return inst; }
        set {
            if ( inst == null )
                inst = value;
        }
    }

    public Player playerData;

    public Client[] clientData;    // all the clients including the player.
    public int currentClientId = 0;

    public Client CurrentClient {
        get{
            return clientData.Length == 0 ? null : clientData[ currentClientId ];
        }
    }

    public bool CurrentClientIsPlayer => playerData.compareClient( CurrentClient );

    void Awake()
    {
        Inst = this;
    }

    private void Start ()
    {
        Protocol.ProtocolHandler.Inst.Bind( 'i', IdentityRequest );
        Protocol.ProtocolHandler.Inst.Bind( 'I', IdentityStatus );
        Protocol.ProtocolHandler.Inst.Bind( 'G', UpdateGameClients );
    }

    private void IdentityRequest( Protocol.BaseProtocol prto )
    {
        Protocol.IdentityRequest request = prto.AsType<Protocol.IdentityRequest>();

        if ( string.IsNullOrWhiteSpace( playerData.nickname ) )  // only set a nickname if we dont have one already.
            playerData.nickname = request.nickname;
        else                                                     // otherwise return our current nickname 
            request.nickname = playerData.nickname;

        request.client_id = playerData.clientId;
        request.reg_key = playerData.reg_key;

        ClientSocket.ActiveSocket.SendMsg( request );

    }

    private void IdentityStatus( Protocol.BaseProtocol prto )
    {
        Protocol.IdentityStatus status = prto.AsType<Protocol.IdentityStatus>();

        if ( !status.ok )
        {
            Debug.LogError( "Error Bad Identity status" );
            return;
        }

        playerData.clientId = status.client_id;
        playerData.reg_key = status.reg_key;

        UIAct_MessageList.MessageList.AddMessage( string.Format( "Wellcom, {0} ({1} :: {2})", playerData.nickname, playerData.clientId, playerData.reg_key ), 20 );

    }

    private void UpdateGameClients( Protocol.BaseProtocol proto )
    {

        // TODO: prevent game clients from being set again.

        Protocol.GameClientList clientList = proto.AsType<Protocol.GameClientList>();
        clientData = new Client[ clientList.client_ids.Length ];

        for ( int i = 0; i < clientList.client_ids.Length; i++ )
        {
            if ( clientList.client_ids[ i ] == playerData.clientId )
                playerData.playerId = clientList.client_player_ids[ i ];

            clientData[ i ] = new Client( clientList.client_ids[ i ], clientList.client_nicknames[ i ], clientList.client_player_ids[ i ] );

        }

        gameClientsSet?.Invoke( clientData );

    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( 'i', IdentityRequest );
        Protocol.ProtocolHandler.Inst.Unbind( 'I', IdentityStatus );
        Protocol.ProtocolHandler.Inst.Unbind( 'G', UpdateGameClients );

    }
}
