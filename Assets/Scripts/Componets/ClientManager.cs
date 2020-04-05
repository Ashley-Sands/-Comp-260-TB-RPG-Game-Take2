using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{

	protected int playerId = -1;  // < 0 is unset 

	protected Protocol.BaseGameAction currentAction;

	// ??
	// carry slot
	// attack slot
	// 

	private void Start ()
	{
		
	}

	public virtual void Init( int pid )
	{
		playerId = pid;
	}

	public virtual void CompleatAction()
	{
		currentAction = null;
	}

	private void OnDestroy ()
	{
		
	}

}
