using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Protocol
{

	class Message : BaseProtocol
	{

		public override char Identity => 'm';

		public int[] to_client_ids;
		public string message;

	}

}