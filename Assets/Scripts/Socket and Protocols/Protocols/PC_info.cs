﻿using System.Collections;
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

	class LobbyClientList : BaseProtocol
	{

		public override char Identity => 'C';

		public int[] client_ids;
		public string[] client_nicknames;

	}

	class LobbyInfo : BaseProtocol
	{

		public override char Identity => 'O';

		public string level_name;
		public string max_players;
		public float starts_in;

	}

}
