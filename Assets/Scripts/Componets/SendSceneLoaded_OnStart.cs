using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendSceneLoaded_OnStart : MonoBehaviour
{
	private void Start ()
	{

		Protocol.ClientStatus status = new Protocol.ClientStatus()
		{
			StatusType = Protocol.ClientStatusType.SceneLoaded,
			ok = true,
			message = gameObject.scene.name
		};

		status.Send();

	}
}
