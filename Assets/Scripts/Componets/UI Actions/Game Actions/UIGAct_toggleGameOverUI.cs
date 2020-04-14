using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGAct_toggleGameOverUI : MonoBehaviour
{

    [SerializeField] private GameObject mainGameCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    [SerializeField] private TextMeshProUGUI winnerLosserText;
    [SerializeField] private TextMeshProUGUI winnerNicknameText;

    private void Awake ()
    {
        mainGameCanvas.SetActive( true );
        gameOverCanvas.SetActive( false );
    }

    void Start()
    {
        Protocol.ProtocolHandler.Inst.Bind( 'A', ToggleUI );
    }

    private void ToggleUI( Protocol.BaseProtocol proto )
    {

        Protocol.GameAction action = proto.AsType<Protocol.GameAction>();

        if ( action.Action != Protocol.GameAction.Actions.EndGame ) 
            return;

        string winnerNickname = GameCtrl.Inst.GetPlayerIdNickname( action.player_id );
        string wlText = GameCtrl.Inst.playerData.playerId == action.player_id ? "Winner!" : "Losser :(";
        wlText = string.Format( "You {0}", wlText );

        winnerLosserText.text = wlText;
        winnerNicknameText.text = winnerNickname;
        
        mainGameCanvas.SetActive( false );
        gameOverCanvas.SetActive( true );

    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( 'A', ToggleUI );
    }
}
