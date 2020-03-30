using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAct_UpdateLobbyInfo : MonoBehaviour
{

    [SerializeField] private UIAct_UpdateLobbyClientList clientList;

    [SerializeField] private TextMeshProUGUI level_name_text;
    [SerializeField] private TextMeshProUGUI level_players_text;
    [SerializeField] private TextMeshProUGUI level_start_in_text;

    int min_players = 1;
    int max_players = 4;

    Coroutine countdownTimer;

    private void Start ()
    {

        Protocol.ProtocolHandler.Inst.Bind( 'O', UpdateLobbyInfo );

    }

    private void UpdateLobbyInfo( Protocol.BaseProtocol proto )
    {

        Protocol.LobbyInfo lobbyInfo = proto.AsType<Protocol.LobbyInfo>();

        min_players = lobbyInfo.min_players; 
        max_players = lobbyInfo.max_players;

        level_name_text.text = lobbyInfo.level_name;
        level_players_text.text = string.Format( "{0} of {1}", clientList.ClientCount, max_players );

        if ( lobbyInfo.starts_in <= 0 )
        {
            level_start_in_text.text = string.Format( "Requires {0} more players", ( min_players - clientList.ClientCount ) );
            if ( countdownTimer != null )
            {
                StopCoroutine( countdownTimer );
                countdownTimer = null;
            }
        }
        else
        {
            if ( countdownTimer != null )
                StopCoroutine( countdownTimer );

            countdownTimer = StartCoroutine( CountdownTimer(lobbyInfo.starts_in) );

        }

    }

    private IEnumerator CountdownTimer( float ttl )
    {

        while ( ttl > 0 )
        {
            yield return new WaitForSeconds( 1f );
            
            ttl -= 1f;

            level_start_in_text.text = string.Format( "{0} seconds...", ttl );


        }

        level_start_in_text.text = "Starting...";

    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Bind( 'O', UpdateLobbyInfo );

    }

}
