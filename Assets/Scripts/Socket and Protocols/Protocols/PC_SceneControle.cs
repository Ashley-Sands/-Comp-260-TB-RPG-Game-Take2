using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
    public class SceneRequest : BaseProtocol
    {

        public override char Identity => 's';

        public string scene_name;

    }

}