using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAct_UpdateLobbyClientList : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI name_text;

    public int ClientCount { get; private set; }

    void Start()
    {
        Protocol.ProtocolHandler.Inst.Bind( 'C', Update_Clients );
    }

    private void Update_Clients( Protocol.BaseProtocol proto )
    {
        
        Protocol.LobbyClientList clientList = proto.AsType<Protocol.LobbyClientList>();

        ClientCount = clientList.client_ids.Length;
        name_text.SetText( string.Join( "\n", clientList.client_nicknames ) );

    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( 'C', Update_Clients );
    }

}
