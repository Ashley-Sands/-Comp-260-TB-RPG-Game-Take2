using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAct_SetGamePlayersText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerListText;
    [SerializeField] private TextMeshProUGUI activePlayerNameText;

    void Start()
    {
        GameCtrl.Inst.gameLoopEvent += UpdateGameInfo;
        ReceivedClients( GameCtrl.Inst.clientData );
    }

    private void UpdateGameInfo( Protocol.GameLoop.Actions action, int ttl )
    {

        ReceivedClients( GameCtrl.Inst.clientData );
        CurrentPlayerChanged();
    }

    private void ReceivedClients( Client[] clients )
    {
        string players = "";
        foreach ( Client c in clients )
        {
            string prefix = "#";

            if ( c.playerId == GameCtrl.Inst.CurrentPlauerId )
                prefix = ">";

            players = string.Format( "{0}{1} ({2}) - {3}\n", players, prefix, c.playerId, c.nickname );

        }

        playerListText.text = players;

    }

    private void CurrentPlayerChanged()
    {
        activePlayerNameText.text = GameCtrl.Inst.CurrentPlayerName;
    }

    private void OnDestroy ()
    {
        GameCtrl.Inst.gameLoopEvent -= UpdateGameInfo;
        GameCtrl.Inst.gameClientsSet -= ReceivedClients;

    }

}
