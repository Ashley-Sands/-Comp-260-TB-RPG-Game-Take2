using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton GameCtrl
/// </summary>
public class GameCtrl : MonoBehaviour
{

    public event System.Action<Client[]>                            gameClientsSet;
    /// <summary>
    /// the action of the event, 'time to live' for the event
    /// </summary>
    public event System.Action<Protocol.GameLoop.Actions, int>      gameLoopEvent;

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

    public Protocol.GameLoop.Actions currentGameLoopState = Protocol.GameLoop.Actions.End;
    public bool CurrentClientIsPlayerAndActive => playerData.compareClient( CurrentClient ) && currentGameLoopState == Protocol.GameLoop.Actions.Start;
    public int CurrentPlauerId => clientData[ currentClientId ].playerId;
    public string CurrentPlayerName => clientData[ currentClientId ].nickname;

    void Awake()
    {
        Inst = this;
    }

    private void Start ()
    {
        Protocol.ProtocolHandler.Inst.Bind( 'i', IdentityRequest );
        Protocol.ProtocolHandler.Inst.Bind( 'I', IdentityStatus );
        Protocol.ProtocolHandler.Inst.Bind( 'G', UpdateGameClients );
        Protocol.ProtocolHandler.Inst.Bind( '>', UpdateGameLoop );
    }

    public string GetPlayerIdNickname( int playerId )
    {
        foreach ( Client c in clientData )
            if ( c.playerId == playerId )
                return c.nickname;

        return "";

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

    private void UpdateGameLoop( Protocol.BaseProtocol proto )
    {

        Protocol.GameLoop gameLoop = proto.AsType<Protocol.GameLoop>();

        currentGameLoopState = gameLoop.Action;

        if ( gameLoop.Action == Protocol.GameLoop.Actions.Change )
        {
            for ( int i = 0; i < clientData.Length; i++ )
                if ( clientData[ i ].playerId == gameLoop.player_id)
                    currentClientId = i;

        }

        gameLoopEvent?.Invoke( gameLoop.Action, gameLoop.t );

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
        print( Inst.clientData.Length + " :: "+ clientData.Length );
        gameClientsSet?.Invoke( clientData );

        // now that the final list of active clients have arrived
        // we can now notify the server that the scene is ready to play
        // Also to be far, we should make sure that the object list has been received as well
        // but thats a task for later. for now we'll just hope its all setup correctly :)
        // TODO: ^^^
        // TODO: Uncomment... Im going to get the relix to work first. ( it will be easier to test :) )
        Protocol.ClientStatus clientStatus = new Protocol.ClientStatus()
        {
            StatusType = Protocol.ClientStatusType.GameReady,
            ok = true
        };

        clientStatus.Send();

    }

    public void KillPlayer( int player_id )
    {

        for ( int i = 0; i < clientData.Length; i++ )
        {
            if ( clientData[ i ].playerId != player_id ) continue;

            clientData[ i ].alive = false;

            if ( playerData.playerId == player_id )
                playerData.alive = false;

        }

    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( 'i', IdentityRequest );
        Protocol.ProtocolHandler.Inst.Unbind( 'I', IdentityStatus );
        Protocol.ProtocolHandler.Inst.Unbind( 'G', UpdateGameClients );
        Protocol.ProtocolHandler.Inst.Unbind( '>', UpdateGameLoop );

    }
}
