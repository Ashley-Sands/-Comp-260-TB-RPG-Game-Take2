using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAct_UpdateGameRelicCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI relicCountText;
    void Start()
    {
        Protocol.ProtocolHandler.Inst.Bind('+', UpdateRelicCount );
    }

    private void UpdateRelicCount( Protocol.BaseProtocol proto )
    {

        Protocol.RelicCount relicCount = proto.AsType<Protocol.RelicCount>();

        // make sure that the count belongs to the player
        if ( relicCount.player_id != GameCtrl.Inst.playerData.playerId )
            return;

        relicCountText.text = relicCount.count.ToString();

    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( '+', UpdateRelicCount );
    }
}
