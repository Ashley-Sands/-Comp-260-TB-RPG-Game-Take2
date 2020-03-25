using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
	class LobbyList : BaseProtocol
	{
		public override char Identity => 'l';

		string[] lobby_names;
		int[]    lobby_ids;
		int[]    current_clients;
		int[]    max_clients;

	}
}
