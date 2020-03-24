using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
    public class IdentityRequest : BaseProtocol
    {
        public override char Identity => 'i';

        int client_id;
        string nickname;
        string reg_key;

    }

    public class IdentityStatus : BaseProtocol
    {
        public override char Identity => 'I';

        int client_id;
        string reg_key;
        bool ok;

    }
}