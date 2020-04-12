using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{

	public delegate void protocol_event ( BaseProtocol protocol );

    /// <summary>
    /// Sigleton class for handlering all protocols.
    /// use Bind/Unbind to connect functions for each protocol
    /// </summary>
	public class ProtocolHandler
	{
        /// <summary>
        /// Event system for each protocol
        /// </summary>
        private class ProtocolEvent
        {
            public event protocol_event callback;

            public void Invoke ( BaseProtocol protocol )
            {
                callback?.Invoke( protocol );
            }

        }

        Dictionary<char, ProtocolEvent> protocolEvents;

        // singleton instance
        private static ProtocolHandler inst;
        public static ProtocolHandler Inst {
            get {
                if ( inst == null )
                    inst = new ProtocolHandler();

                return inst;
            }

        }

        private ProtocolHandler ()
        {
            // see protocol list in server project for a full description of each protocol.
            protocolEvents = new Dictionary<char, ProtocolEvent>
            {
                // Genral Commands
                { '!', new ProtocolEvent() },    // server status
                { '?', new ProtocolEvent() },    // client status
                { '&', new ProtocolEvent() },    // ping
                { 'i', new ProtocolEvent() },    // identity request
                { 'I', new ProtocolEvent() },    // identity status
                { 's', new ProtocolEvent() },    // Scene request
                { 'l', new ProtocolEvent() },    // Lobby List
                { 'O', new ProtocolEvent() },    // Lobby Info
                { 'C', new ProtocolEvent() },    // Lobby Client List
                { 'G', new ProtocolEvent() },    // Game Client List
                { 'm', new ProtocolEvent() },    // message

                // Game actions
                { '>', new ProtocolEvent() },     // Game Loop
                { 'M', new ProtocolEvent() },     // Move Player
                { 'A', new ProtocolEvent() },     // Game Action
                { 'P', new ProtocolEvent() },     // Collect Item
                { 'D', new ProtocolEvent() },     // Apply Damage
                { 'R', new ProtocolEvent() },     // Look At Position
                { 'B', new ProtocolEvent() },     // Build Object

                { '#', new ProtocolEvent() }      // Server Object
                // Dont forget to add it to Convert json as well :)
            };

        }

        /// <summary>
        /// Binds function to protocol callback
        /// </summary>
        /// <param name="idenity">the idenity to bind to</param>
        /// <param name="protocolFunc">function to bind</param>
        public void Bind ( char idenity, protocol_event protocolFunc )
        {

            if ( !protocolEvents.ContainsKey( idenity ) )
            {
                Debug.LogErrorFormat( "Unable to bind, Failed to identify protocol {0}", idenity );
                return;
            }

            protocolEvents[ idenity ].callback += protocolFunc;

        }

        public void BindDict( Dictionary<char, protocol_event> functs )
        {

            foreach ( KeyValuePair<char, protocol_event> pe in functs )
                Bind( pe.Key, pe.Value );

        }

        /// <summary>
        /// Unbinds function to protocol callback
        /// </summary>
        /// <param name="idenity">the idenity to bind to</param>
        /// <param name="protocolFunc">function to bind</param>
        public void Unbind ( char idenity, protocol_event protocolFunc )
        {

            if ( !protocolEvents.ContainsKey( idenity ) )
            {
                Debug.LogErrorFormat( "Unable to unbind, Failed to identify protocol {0}", idenity );
                return;
            }

            protocolEvents[ idenity ].callback -= protocolFunc;

        }

        public void UnbindDict ( Dictionary<char, protocol_event> functs )
        {

            foreach ( KeyValuePair<char, protocol_event> pe in functs )
                Unbind( pe.Key, pe.Value );

        }

        public void InvokeProtocol ( BaseProtocol proto )
        {

            if ( protocolEvents.ContainsKey( proto.Identity ) )
                protocolEvents[ proto.Identity ]?.Invoke( proto );
            else
                Debug.LogErrorFormat( "Unable to invoke protocol {0}", proto.Identity );

        }

        /// <summary>
        /// Handles json string as idenity.
        /// </summary>
        /// <param name="idenity">idenity of the json string</param>
        /// <param name="json">json string of idenity</param>
        /// <returns> protocol. null if protocol does not exist</returns>
        public static BaseProtocol ConvertJson ( char idenity, string json )
        {

            BaseProtocol newProto;

            switch ( idenity )
            {
                case '!':
                    newProto = JsonUtility.FromJson<ServerStatus>( json );
                    break;      
                case '?':
                    newProto = JsonUtility.FromJson<ClientStatus>( json );
                    break;
                case '&':
                    newProto = JsonUtility.FromJson<PingTime>( json );
                    break;
                case 'i':
                    newProto = JsonUtility.FromJson<IdentityRequest>( json );
                    break;
                case 'I':
                    newProto = JsonUtility.FromJson<IdentityStatus>( json );
                    break;
                case 's':
                    newProto = JsonUtility.FromJson<SceneRequest>( json );
                    break;
                case 'l':
                    newProto = JsonUtility.FromJson<LobbyList>( json );
                    break;
                case 'C':
                    newProto = JsonUtility.FromJson<LobbyClientList>( json );
                    break;
                case 'G':
                    newProto = JsonUtility.FromJson<GameClientList>( json );
                    break;
                case 'O':
                    newProto = JsonUtility.FromJson<LobbyInfo>( json );
                    break;
                case 'm':
                    newProto = JsonUtility.FromJson<Message>( json );
                    break;
                case '>':
                    newProto = JsonUtility.FromJson<GameLoop>( json );
                    break;
                case 'M':
                    newProto = JsonUtility.FromJson<MovePlayer>( json );
                    break;
                case 'A':
                    newProto = JsonUtility.FromJson<GameAction>( json );
                    break;
                case 'P':
                    newProto = JsonUtility.FromJson<CollectItem>( json );
                    break;
                case 'D':
                    newProto = JsonUtility.FromJson<ApplyDamage>( json );
                    break;
                case 'R':
                    newProto = JsonUtility.FromJson<LookAtPosition>( json );
                    break;
                case 'B':
                    newProto = JsonUtility.FromJson<BuildObject>( json );
                    break;
                case '#':
                    newProto = JsonUtility.FromJson<ServerObject>( json );
                    break;
                default:    // Not found
                    Debug.LogErrorFormat( "Unable to handle json, Failed to identify protocol {0}", idenity );
                    return null;
            }

            return newProto;

        }

    }
}