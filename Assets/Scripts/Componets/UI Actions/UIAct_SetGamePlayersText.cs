using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAct_SetGamePlayersText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    void Start()
    {

        ReceivedClients( GameCtrl.Inst.clientData );
    }

    private void ReceivedClients( Client[] clients )
    {
        string players = "";

        foreach ( Client c in clients )
            players = string.Format( "{0}{1} - {2}\n", players, c.nickname, c.playerId );

        text.text = players;

    }

    private void CurrentPlayerChanged()
    {

    }

    private void UpdateUI ()
    {
       
    }

    private void OnDestroy ()
    {
        GameCtrl.Inst.gameClientsSet -= ReceivedClients;

    }

}
