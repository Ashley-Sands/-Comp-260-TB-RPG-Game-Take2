using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LobbyButtonGroup : MonoBehaviour
{ 
    [SerializeField] private Button join_button;
    [SerializeField] private TextMeshProUGUI lobbyName;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI players;

    private int buttonLobbyId = -1;

    private bool binded = false;

    public void SetData( int lobbyId, string lobbyNameStr, string levelNameStr, int currentPlayers, int maxPlayers )
    {

        buttonLobbyId = lobbyId;

        if ( !binded )
        {
            join_button.onClick.AddListener( () => ButtonAction() );
            binded = true;
        }

        lobbyName.SetText( lobbyNameStr );
        levelName.SetText( levelNameStr );

        players.SetText( string.Format( "{0} of {1}", currentPlayers, maxPlayers ) );

    }

    private void ButtonAction ()
    {
        // join the server.
        print( "Pressed " + buttonLobbyId );

        Protocol.JoinLobbyRequest lobbyRequest = new Protocol.JoinLobbyRequest() {
            lobby_id = buttonLobbyId
        };

        lobbyRequest.Send();

    }

}