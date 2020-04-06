using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
    public enum ServerStatusType { Server = 0, LobbyRequest = 1}
    public class ServerStatus : BaseProtocol
    {

        public override char Identity => '!';

        public ServerStatusType StatusType{
            get{
                return (ServerStatusType)status_type;
            }
            set {
                status_type = (int)value;
            }

        }

        public int status_type;
        public bool ok;
        public string message;


    }

    public enum ClientStatusType { Client = 0, SceneLoaded = 1, GameReady = 2 }

    public class ClientStatus : BaseProtocol
    {

        public override char Identity => '?';

        public ClientStatusType StatusType {
            get {
                return (ClientStatusType)status_type;
            }
            set {
                status_type = (int)value;
            }

        }

        public int status_type;
        public bool ok;
        public string message;


    }

}