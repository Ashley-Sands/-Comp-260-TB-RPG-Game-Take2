using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
    public abstract class BaseGameAction : BaseProtocol
    {

        public int player_id;
        /// <summary>
        /// Should the message be sent to self (locally)
        /// For most cases this is true but sometimes 
        /// we need to get some info from the server 
        /// befor running the action.
        /// </summary>
        public virtual bool SendLocal => true;  

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
        public enum Actions { DropItem = 0, LaunchProjectile = 1, EndTurn = 2, EndGame = 3 }
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

        public float health;
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

    public class BuildObject : BaseGameAction
    {

        public override char Identity => 'B';
        public override bool SendLocal => false;

        public ServerObject.ObjectType Type {
            get => (ServerObject.ObjectType)type;
            set => type = (int)value;
        }

        public int obj_id;  // return by the server..
        public int type;

    }

    public class GameLoop : BaseGameAction 
    {
        public override char Identity => '>';

        public enum Actions { Change = 0, Start = 1, End = 2 }

        public Actions Action {
            get => (Actions)action;
            set => action = (int)value;
        }

        public int action;
        public int t;

    }

    public class RelicCount : BaseGameAction
    {
        public override char Identity => '+';

        public int count;

    }
    // TODO: move below into own file

    // Altho this inherits from BaseProtocol its an action that can only happen in the game
    public class ServerObject : BaseProtocol 
    {
        public enum ObjectType { Player = 0, Relic = 1, Block = 2 }
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

        public Vector3 Rotation {
            get => new Vector3( r_x, r_y, r_z );
            set {
                r_x = value.x;
                r_y = value.y;
                r_z = value.z;
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
        public float r_x, r_y, r_z;

        public int type;
        public int action;
        public int object_id = -1;

        public ServerObject( Vector3 _position, Vector3 _rotation, ObjectType _type, int _objId, ObjectAction _action = ObjectAction.Defualt )
        {
            Position = _position;
            Rotation = _rotation;
            Type = _type;
            Action = _action;
            object_id = _objId;
        }

    }
    
}