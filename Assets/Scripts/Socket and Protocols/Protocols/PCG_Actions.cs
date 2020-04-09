using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
    public abstract class BaseGameAction : BaseProtocol
    {

        public int player_id;

    }

    public class MovePlayer : BaseGameAction
    {

        public override char Identity => 'M';

        public Vector3 Position
        {
            get => new Vector3( x, y, z );
            set{
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public float x, y, z;

    }

    public class CollectItem : BaseGameAction
    {

        public override char Identity => 'P';

        public int object_id;
        
    }

    public class GameAction : BaseGameAction
    {
        public enum Actions { DropItem = 0, LaunchProjectile = 1}
        public override char Identity => 'A';

        public Actions Action {
            get => (Actions)action;
            set => action = (int)value;
        }

        public int action;

        public GameAction( Actions act )
        {
            Action = act;
        }

    }

    public class Explosion : BaseProtocol
    {

        public override char Identity => 'E';

        public Vector3 Position {
            get => new Vector3( x, y, z );
            set {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public float x, y, z;

    }

    public class ApplyDamage : BaseGameAction
    {
        public override char Identity => 'D';

        public float damage;
        public bool kill;

    }

    public class LookAtPosition : BaseGameAction
    {
        public override char Identity => 'R';

        public Vector3 Position {
            get => new Vector3( x, y, z );
            set{
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public float x, y, z;

    }


    // TODO: move below into own file

    // Altho this inherits from BaseProtocol its an action that can only happen in the game
    public class ServerObject : BaseProtocol 
    {
        public enum ObjectType { Player = 0, Relic = 1 }
        public enum ObjectAction { Defualt = 0, Add = 1, Destroy = 2 }
        public override char Identity => '#';

        public Vector3 Position {
            get => new Vector3( x, y, z );
            set {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public ObjectType Type{
            get{ return (ObjectType)type; }
            set{ type = (int)value; }
        }

        public ObjectAction Action{
            get { return (ObjectAction)action; }
            set { action = (int)value; }
        }

        public float x, y, z;

        public int type;
        public int action;
        public int object_id = -1;

        public ServerObject( Vector3 _position, ObjectType _type, int _objId, ObjectAction _action = ObjectAction.Defualt )
        {
            Position = _position;
            Type = _type;
            Action = _action;
            object_id = _objId;
        }

    }
    
    /// <summary>
    /// this is a bit of a work around to be able to queue a server object in the player manager.
    /// 
    /// </summary>
    public class QueueServerObject : BaseGameAction
    {
        public override char Identity => serverObject.Identity;

        public ServerObject serverObject;

        public QueueServerObject ( Vector3 _position, ServerObject.ObjectType _type, int _objId, 
                                   ServerObject.ObjectAction _action = ServerObject.ObjectAction.Defualt )
        {
            serverObject = new ServerObject( _position, _type, _objId, _action );
        }

        public override T AsType<T> ()
        {
            return serverObject.AsType<T>();
        }

        public override string GetJson ( out int jsonLength )
        {
            return serverObject.GetJson( out jsonLength );
        }

        public override int GetJsonLength ()
        {
            return serverObject.GetJsonLength();
        }

        public override void Send ()
        {
            serverObject.Send();
        }
        
    }

}