using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAct_lobbieList_Select : MonoBehaviour
{

    [SerializeField] private RectTransform parent;
    [SerializeField] private UI_LobbyButtonGroup baseButtonGroup;

    private List<UI_LobbyButtonGroup> lobbyListGroups = new List<UI_LobbyButtonGroup>();

    [SerializeField] private Vector2 startPosition = Vector2.zero;
    [SerializeField] private Vector2 buttonOffset = new Vector2(0, 75);

    private void Awake ()
    {

        Protocol.ProtocolHandler.Inst.Bind( 'l', UpdateLobbyList );

    }

    private void UpdateLobbyList( Protocol.BaseProtocol proto )
    {

        print( "Identity l processed................" );

        Protocol.LobbyList lobbyList = proto.AsType<Protocol.LobbyList>();

        // go thorough every button (that we need or pre existing)
        // if there currently more items than needed will just switch them off
        // otherwise we add new items if neeed
        for ( int i = 0; i < Mathf.Max( lobbyList.lobby_ids.Length, lobbyListGroups.Count ); i++ )
        {
            
            if ( i >= lobbyListGroups.Count )
            {   // Add new buttons
                UI_LobbyButtonGroup button = Instantiate<UI_LobbyButtonGroup>( baseButtonGroup, parent );
                lobbyListGroups.Add( button );
            }
            else if ( i >= lobbyList.lobby_ids.Length )
            {   // switch off existing buttons
                lobbyListGroups[ i ].gameObject.SetActive( false );
            }
            else
            {   // update info
                lobbyListGroups[ i ].gameObject.SetActive( true );
            } 

            // update position and info
            if ( i < lobbyList.lobby_ids.Length )
            {
                ( lobbyListGroups[ i ].transform as RectTransform ).anchoredPosition = startPosition - (buttonOffset * i);
                lobbyListGroups[ i ].SetData( lobbyList.lobby_ids[ i ], lobbyList.lobby_names[ i ], "Level Name", lobbyList.current_clients[ i ], lobbyList.max_clients[ i ] );
            }  
        }

    }

    private void OnDestroy ()
    {

        Protocol.ProtocolHandler.Inst.Bind( 'l', UpdateLobbyList );

    }
}
