using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientSocket : MonoBehaviour
{

    public enum ConnectionStatus { None, Conntecting, Connected, Reset}

    private const int MESSAGE_LEN_PACKAGE_SIZE = 2;
    private const int MESSAGE_TYPE_PACKAGE_SIZE = 1;
    private const int MESSAGE_MAX_LENGTH = 1024;

    public static ClientSocket ActiveSocket { get; private set; }

    private ConnectionStatus connStatus = ConnectionStatus.None;
    public ConnectionStatus ConnStatus {
        get{
            lock(this)
            {
                return connStatus;
            }
        }
        set {
            lock ( this )
            {
                connStatus = value;
            }
        }
    }

    private Socket socket = null;
    private ASCIIEncoding encoder = new ASCIIEncoding();

    private readonly string hostIp = "127.0.0.1";
    private readonly int port = 8222;

    private Thread connectThread;
    private Thread receiveThread;
    private Thread sendThread;

    private Queue inboundQueue;
    private Queue outboundQueue;

    private void Awake ()
    {
        ActiveSocket = this;

        DontDestroyOnLoad( this );
    }


    private void InitializeSocket()
    {

        // creates a new socket and starts the connecting process
        if ( socket == null )
        {
            socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
            ConnStatus = ConnectionStatus.Conntecting;

            connectThread = new Thread( ConnectThread );
            connectThread.Start();
        }

    }

    private void Start ()
    {

        // start the synchronized queues
        inboundQueue = Queue.Synchronized( new Queue() );
        outboundQueue = Queue.Synchronized( new Queue() );

    }

    private void ConnectThread()
    {

        while ( ConnStatus == ConnectionStatus.Conntecting )
        {
            try
            {
                socket.Connect( new IPEndPoint( IPAddress.Parse( hostIp ), port ) );
                ConnStatus = ConnectionStatus.Connected;
            }
            catch ( System.Exception e )
            {
                Debug.LogErrorFormat( " Error: {0} ", e );
                Debug.LogFormat( " Retring connection " );
            }
        }

        // start the receive thread. the send thread will only run if theres items to be sent.
        receiveThread = new Thread( ReceiveThread );
        receiveThread.Start();

    }

    /// <summary>
    /// Retreves message from cue if any are available
    /// </summary>
    private Object ReceiveMessage()
    {
        if ( inboundQueue.Count > 0 )
            return inboundQueue.Dequeue() as Object;
        else
            return null;
    }

    private void ReceiveThread ()
    {
        
        while (ConnStatus == ConnectionStatus.Connected)
        {

        }

    }

    /// <summary>
    /// Add a message to the cue and starts the send thread if not already running
    /// </summary>
    /// <param name="message"></param>
    private void SendMessage( Object message )
    {

        outboundQueue.Enqueue( message );

        if ( sendThread == null || !sendThread.IsAlive )
            sendThread = new Thread( SendThread );

    }

    private void SendThread()
    {

        while ( ConnStatus == ConnectionStatus.Connected && outboundQueue.Count > 0)
        {
            
        }

    }

}
