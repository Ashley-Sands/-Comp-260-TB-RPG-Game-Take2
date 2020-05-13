using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientSocket : MonoBehaviour
{
    public event System.Action<ConnectionStatus> connectionStatusChanged;
    public enum ConnectionStatus { None, Conntecting, Connected, Error}

    private const int MESSAGE_LEN_PACKAGE_SIZE = 2;
    private const int MESSAGE_TYPE_PACKAGE_SIZE = 1;
    private const int MESSAGE_MAX_LENGTH = 1024;

    public static ClientSocket ActiveSocket { get; private set; }

    private ConnectionStatus _connStatus = ConnectionStatus.None;
    public ConnectionStatus ConnStatus {
        get{
            lock(this)
            {
                return _connStatus;
            }
        }
        set {
            lock ( this )
            {
                if ( value != _connStatus )
                {
                    _connStatus = value;
                    connectionStatusChanged?.Invoke( value );
                }
            }
        }
    }

    private Socket socket = null;
    private ASCIIEncoding encoder = new ASCIIEncoding();

    public bool localhost = true;
    private readonly string localhostIp = "127.0.0.1";
    [SerializeField] private string hostIp = "159.65.80.187";
    private string HostIp => localhost ? localhostIp : hostIp;
    private readonly int port = 8222;

    private bool _canStart = false;      // Call InitializeSocket to start.
    public bool _autoReconnect = true;
    private bool _clean = false;
    private bool _cleaning = false;

    private Thread connectThread;
    private Thread receiveThread;
    private Thread sendThread;

    private Queue inboundQueue;
    private Queue outboundQueue;

    // make it all thread safe :)
    private bool CanStart {
        get { lock ( this ) return _canStart;  }
        set { lock ( this ) _canStart = value; }
    }

    private bool AutoReconnect {
        get { lock ( this ) return _autoReconnect; }
        set { lock ( this ) _autoReconnect = value; }
    }

    private bool Clean {
        get { lock ( this ) return _clean; }
        set { lock ( this ) _clean = value; }
    }

    private bool Cleaning {
        get { lock ( this ) return _cleaning; }
        set { lock ( this ) _cleaning = value; }
    }

    private void Awake ()
    {
        ActiveSocket = this;

        DontDestroyOnLoad( this );
    }


    public void InitializeSocket()
    {

        // creates a new socket and starts the connecting process
        if ( socket == null )
        {
            CanStart = true;
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

    private void Update ()
    {

        if ( Clean && !Cleaning )
            CleanUpConnection();
        else if ( Clean && Cleaning )
            Clean = false;

        if ( ConnStatus == ConnectionStatus.None && CanStart && AutoReconnect )
            InitializeSocket();

        while ( inboundQueue.Count > 0 )
        {
            Protocol.BaseProtocol proto = inboundQueue.Dequeue() as Protocol.BaseProtocol;
            Protocol.ProtocolHandler.Inst.InvokeProtocol(proto);
            print( "Recived inbound :: "+ proto.Identity );
        }

    }

    private void ConnectThread()
    {

        while ( ConnStatus == ConnectionStatus.Conntecting )
        {
            try
            {
                socket.Connect( new IPEndPoint( IPAddress.Parse( HostIp ), port ) );
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

    private void ReceiveThread ()
    {
        // altho we only recive 2 bytes for length and 1 byte for identity 
        // bitConverter converts in chunks of 4 :(
        byte[] mesLenBuffer = new byte[ 4 ]; // MESSAGE_LEN_PACKAGE_SIZE ];
        byte[] mesTypeBuffer = new byte[ 4 ]; // MESSAGE_TYPE_PACKAGE_SIZE ];
        byte[] mesBuffer = new byte[ MESSAGE_MAX_LENGTH ];              // Define the message buffer out of the while loop so we dont have to realocate unless we start a new thread:)

        while (ConnStatus == ConnectionStatus.Connected)
        {

            int bytes = 0;
            // recive first bytes to see how long the message is
            try
            {
                bytes = socket.Receive( mesLenBuffer, 0, MESSAGE_LEN_PACKAGE_SIZE, SocketFlags.None );
                // Get the next byte to see what data the message contatines
                bytes = socket.Receive( mesTypeBuffer, 0, MESSAGE_TYPE_PACKAGE_SIZE, SocketFlags.None );
            }
            catch ( System.Exception e )
            {
                Debug.LogError( e );
                ConnStatus = ConnectionStatus.Error;
                Clean = true;
                break;
            }
            
            if ( bytes == 0)
            {
                Debug.LogError( "Disconected..." );
                ConnStatus = ConnectionStatus.None;
                Clean = true;
                return;
            }

            if ( System.BitConverter.IsLittleEndian )
            {   // use first two bytes reversed for little endian
                byte tempByte = mesLenBuffer[ 0 ];
                mesLenBuffer[ 0 ] = mesLenBuffer[ 1 ];
                mesLenBuffer[ 1 ] = tempByte;
            }

            int messageLen = System.BitConverter.ToInt32( mesLenBuffer, 0 );
            char messageIdenity = System.BitConverter.ToChar( mesTypeBuffer, 0 );

            if ( messageLen > MESSAGE_MAX_LENGTH )
            {
                // TODO: send the message back to server so it can be loged as a fatal error
                // Or should i just realocate?
                Debug.LogErrorFormat( "FATAL ERROR: Message has exceded the max message size. The message has been loged, and discarded as a result! (Max message size: {0} Received message size: {1})",
                                      MESSAGE_MAX_LENGTH, messageLen );
                continue;

            }

            Debug.LogWarningFormat( "Recived message Len {0}; Identity {1}; ", messageLen, messageIdenity );

            int result = 0;

            // receive the message
            try
            {
                result = socket.Receive( mesBuffer, 0, messageLen, SocketFlags.None );
            }
            catch ( System.Exception e )
            {
                Debug.LogError( e );
                Clean = true;
                break;
            }

            string message = encoder.GetString( mesBuffer, 0, result );

            Protocol.BaseProtocol protocol = Protocol.ProtocolHandler.ConvertJson( messageIdenity, message );

            inboundQueue.Enqueue( protocol );
            Debug.Log( "Inbound Message: " + message );

        }

    }

    public void LocalSendMsg( Protocol.BaseProtocol message )
    {
        message.from_client_name = GameCtrl.Inst.playerData.nickname;
        inboundQueue.Enqueue( message );
        print( "Local message queued" );
    }

    /// <summary>
    /// Add a message to the que and starts the send thread if not already running
    /// </summary>
    /// <param name="message"></param>
    public void SendMsg( Protocol.BaseProtocol message )
    {

        outboundQueue.Enqueue( message );

        if ( sendThread == null || !sendThread.IsAlive )
        {
            sendThread = new Thread( SendThread );
            sendThread.Start();
        }

        print( "outbound message cued" );


    }

    private void SendThread()
    {
        print( ConnStatus + " :: "+ outboundQueue.Count );
        while ( ConnStatus == ConnectionStatus.Connected && outboundQueue.Count > 0)
        {

            Protocol.BaseProtocol message = outboundQueue.Dequeue() as Protocol.BaseProtocol;
            string data = message.GetJson( out int messageLength );

            // the byte converter, converts in chunks of 4, we only need the 2 for the len and 1 for the char
            byte[] _dataLenBytes = System.BitConverter.GetBytes( messageLength );
            byte[] _dataIdenityBytes = System.BitConverter.GetBytes( message.Identity );

            byte[] dataLenBytes = new byte[ MESSAGE_LEN_PACKAGE_SIZE ];
            byte[] dataIdenityBytes = new byte[ MESSAGE_TYPE_PACKAGE_SIZE ];
            byte[] dataBytes = encoder.GetBytes( data );

            // TODO: Make this work for different packet sizes
            // Get the bytes that we need
            // We are working with Big endian on the server :)
            if ( System.BitConverter.IsLittleEndian )
            {   // use first two bytes reversed for little endian
                byte temp = _dataLenBytes[ 0 ];
                dataLenBytes[ 0 ] = _dataLenBytes[ 1 ];
                dataLenBytes[ 1 ] = temp;
                dataIdenityBytes[ 0 ] = _dataIdenityBytes[ 0 ];

            }
            else
            {   // use last two bytes for big endian
                dataLenBytes[ 0 ] = _dataLenBytes[ 3 ];
                dataLenBytes[ 1 ] = _dataLenBytes[ 4 ];
                dataIdenityBytes[ 0 ] = _dataIdenityBytes[ 4 ];
            }

            Debug.LogWarningFormat( "Sending mesage Length: {0}; Idenity: {1}", messageLength, message.Identity );
            Debug.Log( "Outbound Message: " + data );

            try
            {
                socket.Send( dataLenBytes );                                    // send the length of the message
                socket.Send( dataIdenityBytes );                                // send the idenity of the message
                socket.Send( dataBytes );                                       // send the message

            }
            catch ( System.Exception e )
            {
                Debug.LogError( e );
                ConnStatus = ConnectionStatus.Error;
                Clean = true;
                break;
            }

        }

    }

    public void Disconnect()
    {
        CanStart = false;
        CleanUpConnection();
    }

    /// <summary>
    /// Cleans up the socket and threads affter disconnecting.
    /// safe for expected and unexpected disconnections
    /// 
    /// </summary>
    private void CleanUpConnection()
    {

        if ( Cleaning ) return;

        Cleaning = true;
        Clean = false;

        if ( ConnStatus == ConnectionStatus.Connected || ConnStatus == ConnectionStatus.Conntecting )
            ConnStatus = ConnectionStatus.None;

        if ( socket != null )
        {
            try
            {   // this will error only if we have disconnected (most likey unexpectly)
                socket.Shutdown( SocketShutdown.Both );
            }
            catch { }   // so theres no need to worry about it :)

            try
            {
                socket.Close();
            }
            catch( System.Exception e)
            {
                Debug.LogErrorFormat( "Failed to close socket: {0]", e );
            }

        }

        // check all the threads are dead.
        if ( connectThread != null && connectThread.IsAlive )
            connectThread.Join();

        if ( receiveThread != null && receiveThread.IsAlive )
            receiveThread.Join();

        if ( sendThread != null && sendThread.IsAlive )
            sendThread.Join();

        // clean it all up ready to try again
        socket        = null;
        connectThread = null;
        receiveThread = null;
        sendThread    = null;

        ConnStatus = ConnectionStatus.None;

        Cleaning = false;
        Clean = false;
        print( "Deaded Sockets" );
    }

    private void OnDestroy ()
    {
        CanStart = false;
        CleanUpConnection();
    }

}
