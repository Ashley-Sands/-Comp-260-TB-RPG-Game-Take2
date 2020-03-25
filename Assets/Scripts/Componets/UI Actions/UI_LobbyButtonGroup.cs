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

    public void SetData( int lobbyId, string lobbyNameStr, string levelNameStr, int currentPlayers, int maxPlayers )
    {
        
        buttonLobbyId = lobbyId;

        join_button.onClick.AddListener( () => ButtonAction() );

        lobbyName.SetText( lobbyNameStr );
        levelName.SetText( levelNameStr );

        players.SetText( string.Format( "{0} of {1}", currentPlayers, maxPlayers ) );

    }

    private void ButtonAction( )
    {
        // join the server.
        print( "Pressed "+ buttonLobbyId );
    }

}