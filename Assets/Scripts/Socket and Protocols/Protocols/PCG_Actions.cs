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
}