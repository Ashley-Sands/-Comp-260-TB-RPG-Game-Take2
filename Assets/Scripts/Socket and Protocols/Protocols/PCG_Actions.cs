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
}