using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendSceneLoaded_OnAwake : MonoBehaviour
{
	private void Awake ()
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
