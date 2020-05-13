using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AMSHelpers;
using TMPro;

public class HandlePing : MonoBehaviour
{

    [SerializeField] private GameObject testing_hud;
    [SerializeField] private TextMeshProUGUI pingTimeText;
    CSV csvFile;
    public float pingRate = 0.1f;

    private void Awake ()
    {

        int fileCount = SaveLoadFile.GetFileCount( Application.dataPath + "/testData/" );
        csvFile = new CSV( Application.dataPath + "/testData/", "pingData." + fileCount );

        csvFile.AddRow( new string[] {
                "Client send time",
                "ServerRecevie time",
                "client receive time",
                "total time",
                "time to server",
                "return time"
                } );

        csvFile.SaveCSV();

        SceneManager.activeSceneChanged += SceneChanged;

    }
    // Start is called before the first frame update
    private void Start ()
    {
        Protocol.ProtocolHandler.Inst.Bind( '&', ProcessesPing );
    }


    void Update ()
    {

        if ( Input.GetKeyDown( KeyCode.Alpha0 ) )
        {

            InvokeRepeating( "PingGame", 0, pingRate );
            testing_hud.SetActive( true );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha9 ) )
        {
            CancelInvoke( "PingGame" );
            testing_hud.SetActive( false );
        }

    }

    void PingGame ()
    {
        System.TimeSpan t = System.DateTime.UtcNow - new System.DateTime( 1970, 1, 1 );
        double millisSinceEpoch = t.TotalMilliseconds;

        ClientSocket.ActiveSocket.SendMsg( new Protocol.PingTime( millisSinceEpoch ) );
    }

    private void ProcessesPing( Protocol.BaseProtocol proto )
    {

        Protocol.PingTime ping = proto.AsType<Protocol.PingTime>();

        System.TimeSpan t = System.DateTime.UtcNow - new System.DateTime( 1970, 1, 1 );
        double millisSinceEpoch = t.TotalMilliseconds;

        double total_time = millisSinceEpoch - ping.client_send_time;
        double time_to_server = ping.server_receive_time - ping.client_send_time;
        double return_time = millisSinceEpoch - ping.server_receive_time;

        pingTimeText.text = string.Format( "ping: {0:f3}ms", total_time);

        /* CSV HEADERS
saveLoadFile.AddRow( new string[] {
        "Client send time",
        "ServerRecevie time",
        "client receive time",
        "total time",
        "time to server",
        "return time"
        } );
*/
        csvFile.AddRow( new string[] {
                ping.client_send_time.ToString(),
                ping.server_receive_time.ToString(),
                millisSinceEpoch.ToString(),
                total_time.ToString(),
                time_to_server.ToString(),
                return_time.ToString()
                } );

        csvFile.SaveCSV();

    }

    private void SceneChanged( Scene from, Scene to)
    {
        // Log the scene in the CSV :)
        csvFile.AddRow( new string[] {
                "From:",
                from.name,
                "",
                "To:",
                to.name,
                ""
                } );

        csvFile.SaveCSV();

    }

    private void OnDestroy ()
    {
        Protocol.ProtocolHandler.Inst.Unbind( '&', ProcessesPing );
        SceneManager.activeSceneChanged -= SceneChanged;

    }
}
