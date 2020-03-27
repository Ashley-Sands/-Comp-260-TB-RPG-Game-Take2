using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePing : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start ()
    {
        Protocol.ProtocolHandler.Inst.Bind( '&', ProcessesPing );
    }


    void Update ()
    {

        if ( Input.GetKeyDown( KeyCode.P ) )
        {

            InvokeRepeating( "PingGame", 0, 0.5f );
            //pinging_HUD.SetActive( true );
        }

        if ( Input.GetKeyDown( KeyCode.O ) )
        {
            CancelInvoke( "PingGame" );
            //pinging_HUD.SetActive( false );
        }

    }

    void PingGame ()
    {
        System.TimeSpan t = System.DateTime.UtcNow - new System.DateTime( 1970, 1, 1 );
        double millisSinceEpoch = t.TotalMilliseconds;

        ClientSocket.ActiveSocket.SendMsg( new Protocol.PingTime( millisSinceEpoch ) );
    }

    private void ProcessesPing( Protocol.BaseProtocol proto )
    {

        Protocol.PingTime ping = proto.AsType<Protocol.PingTime>();

        System.TimeSpan t = System.DateTime.UtcNow - new System.DateTime( 1970, 1, 1 );
        double millisSinceEpoch = t.TotalMilliseconds;

        double total_time = millisSinceEpoch - ping.client_send_time;
        double time_to_server = ping.server_receive_time - ping.client_send_time;
        double return_time = millisSinceEpoch - ping.server_receive_time;

        Debug.LogFormat( ">>>>>>>>>>>>>PING (0)<<<<<<<<<<<<<<<<<< Client send time: {0}; Server recieve time: {1}; Client receive time: {2}", ping.client_send_time, ping.server_receive_time, millisSinceEpoch );
        Debug.LogFormat( ">>>>>>>>>>>>>PING (1)<<<<<<<<<<<<<<<<<< total time: {0}; Time to server: {1}; return time: {2}", total_time, time_to_server, return_time );


    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( '&', ProcessesPing );

    }
}
