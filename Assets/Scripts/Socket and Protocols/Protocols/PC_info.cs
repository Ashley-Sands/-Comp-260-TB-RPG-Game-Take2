using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{
	class LobbyList : BaseProtocol
	{
		public override char Identity => 'l';

		public string[] lobby_names;
		public int[]    lobby_ids;
		public int[]    current_clients;
		public int[]    max_clients;

	}
}
