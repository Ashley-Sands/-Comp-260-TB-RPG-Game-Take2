using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{

	class JoinLobbyRequest : BaseProtocol
	{

		public override char Identity => 'L';

		public int lobby_id;

	}

}
