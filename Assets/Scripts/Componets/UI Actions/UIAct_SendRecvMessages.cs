using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAct_SendRecvMessages : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TextMeshProUGUI output;

    // Start is called before the first frame update
    void Start()
    {
        Protocol.ProtocolHandler.Inst.Bind('m', UpdateMessages);
    }

    private void UpdateMessages( Protocol.BaseProtocol proto)
    {
        Protocol.Message message = proto.AsType<Protocol.Message>();

        string receivedTime = System.DateTime.Now.ToShortTimeString();

        string outStr = string.Format( "{0}\n{1} | {2}: {3}", output.text, receivedTime, message.from_client_name, message.message );

        output.SetText( outStr );

    }

    public void SendMsg()
    {

        if ( string.IsNullOrWhiteSpace( input.text ) )
        {
            Protocol.Message msg = new Protocol.Message() { message = input.text };
            msg.Send();
        }
    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind('m', UpdateMessages);

    }

    struct Message{
        string recivedAt;
        string from;
        string message;

        public string ToStr()
        {
            return string.Format( "{0} | {1}: {2}", recivedAt, from, message );
        }
    }
}
