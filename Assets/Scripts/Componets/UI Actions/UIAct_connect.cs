using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAct_connect : MonoBehaviour
{

    [SerializeField] private Button[] disableOnConnect;
    [SerializeField] private TMP_InputField IpInput;

    private bool _updateButtons = false;
    private bool UpdateButtons {
        get{
            lock( this)
            {
                return _updateButtons;
            }
        }
        set{
            lock(this)
            {
                _updateButtons = value;
            }
        }
    }
    private bool _active = false;
    private bool Active {
        get {
            lock ( this )
            {
                return _active;
            }
        }
        set {
            lock ( this )
            {
                _active = value;
            }
        }
    }

    public void Awake ()
    {
        ClientSocket.ActiveSocket.connectionStatusChanged += StatusChange;
    }

    public void Start ()
    {
        if ( PlayerPrefs.HasKey( "server-ip" ) )
            IpInput.text = PlayerPrefs.GetString( "server-ip" );

    }

    public void LateUpdate ()
    {

        if ( UpdateButtons )
        {
            foreach ( Button b in disableOnConnect )
                b.enabled = Active;

            UpdateButtons = false;

        }
    }

    public void Connect()
    {
        ClientSocket.ActiveSocket.hostIp = IpInput.text;
        ClientSocket.ActiveSocket.InitializeSocket();

        PlayerPrefs.SetString( "server-ip", IpInput.text );

    }

    public void StatusChange( ClientSocket.ConnectionStatus connStatus )
    {

        Active = !( connStatus == ClientSocket.ConnectionStatus.Conntecting );

        UpdateButtons = true;
            
    }

    private void OnDestroy ()
    {
        ClientSocket.ActiveSocket.connectionStatusChanged -= StatusChange;
    }

}
