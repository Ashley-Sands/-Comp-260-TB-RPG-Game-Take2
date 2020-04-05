using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{

	protected int playerId = -1;  // < 0 is unset 

	public virtual void Init( int pid )
	{
		playerId = pid;
	}

}
