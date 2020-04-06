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
                return (ServerStatusType)serverStatusType;
            }
            set {
                serverStatusType = (int)value;
            }

        }

        public int serverStatusType;
        public bool ok;
        public string message;


    }

    public enum ClientStatusType { Client = 0, SceneLoaded = 1, GameReady = 2 }

    public class ClientStatus : BaseProtocol
    {

        public override char Identity => '?';

        public ClientStatusType StatusType {
            get {
                return (ClientStatusType)serverStatusType;
            }
            set {
                serverStatusType = (int)value;
            }

        }

        public int serverStatusType;
        public bool ok;
        public string message;


    }

}