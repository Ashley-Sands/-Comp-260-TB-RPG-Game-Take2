using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
    public class IdentityRequest : BaseProtocol
    {
        public override char Identity => 'i';

        public int client_id;
        public string nickname;
        public string reg_key;

    }

    public class IdentityStatus : BaseProtocol
    {
        public override char Identity => 'I';

        public int client_id;
        public string reg_key;
        public bool ok;

    }
}