using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a singletion for connection status ect...
/// </summary>
public class UIAct_MessageList : MonoBehaviour
{

    public static UIAct_MessageList MessageList { get; private set; }

    private List<string> messages = new List<string>();
    [SerializeField] private GameObject holdObject;
    [SerializeField] private TMPro.TextMeshProUGUI textArea;

    Queue coroutineQue = new Queue();

    private void Awake ()
    {
        MessageList = this;
    }

    private void Start ()
    {

        Protocol.ProtocolHandler.Inst.Bind('!', PrintServerStatus);
        ClientSocket.ActiveSocket.connectionStatusChanged += ConnectionStatusChanged;
        InvokeRepeating("UpdateUI", 1f, 1f);

    }

    private void LateUpdate ()
    {

        while ( coroutineQue.Count > 0 )
            StartCoroutine( coroutineQue.Dequeue() as IEnumerator );

    }

    public void AddMessage( string message, int ttl )
    {
        messages.Add( message );

        // make sure it all happens on the main thread.
        if ( ttl > 0 )
            coroutineQue.Enqueue( RemoveMessageInTime( message, ttl ) );

    }

    public void RemoveMessage( string message )
    {
        // make sure it all happens on the main thread.
        coroutineQue.Enqueue( RemoveMessageInTime( message, 0 ) );

    }

    IEnumerator RemoveMessageInTime(string message, int ttl)
    {

        yield return new WaitForSeconds( ttl );

        if ( messages.Contains( message ) )
            messages.Remove( message );

    }

    public void Clear()
    {
        messages.Clear();
    }

    void ConnectionStatusChanged( ClientSocket.ConnectionStatus connStatus )
    {

        Dictionary<ClientSocket.ConnectionStatus, string> connStatusMsg = new Dictionary<ClientSocket.ConnectionStatus, string>()
        {
            { ClientSocket.ConnectionStatus.Conntecting, "Connecting..." },
            { ClientSocket.ConnectionStatus.Connected, "Connected" },
            { ClientSocket.ConnectionStatus.Error, "Error." }
        };

        if ( connStatusMsg.ContainsKey( connStatus ) )
        {
            AddMessage( connStatusMsg[ connStatus ], ( connStatus == ClientSocket.ConnectionStatus.Connected ? 30 : -1 ) );
        //    connStatusMsg.Remove( connStatus ); // make sure we dont remove the newly added message
        }

        // make sure the other messages are not still in the log.
        //foreach ( KeyValuePair<ClientSocket.ConnectionStatus, string> kv in connStatusMsg )
        //    RemoveMessage( kv.Value );
        
    }

    private void UpdateUI()
    {

        holdObject.SetActive( messages.Count > 0 );  

        string text = string.Join( "\n", messages );
        textArea.SetText( text );
    }

    private void PrintServerStatus( Protocol.BaseProtocol proto )
    {

        Protocol.ServerStatus serverStatus = proto.AsType<Protocol.ServerStatus>();
        
        if ( !serverStatus.ok )
        {
            AddMessage( "Error: "+serverStatus.message, 30 );
        }

    }

    private void OnDestroy ()
    {
        ClientSocket.ActiveSocket.connectionStatusChanged -= ConnectionStatusChanged;
        Protocol.ProtocolHandler.Inst.Unbind( '!', PrintServerStatus );

    }

}
