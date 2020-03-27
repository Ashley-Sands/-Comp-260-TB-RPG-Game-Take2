using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Protocol
{
	class PingTime : BaseProtocol
	{
		public override char Identity => '&';

		public double client_send_time;
		public double server_receive_time;

		public PingTime( double clientSendTime )
		{
			client_send_time = clientSendTime;
		}

	}

}