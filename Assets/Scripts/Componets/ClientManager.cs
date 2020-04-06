using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{

	protected int playerId = -1;  // < 0 is unset 

	[SerializeField] private ClientAgent clientAgent;

	protected ClientAction currentAction;

	public Dictionary<char, Protocol.protocol_event> bindActions;

	// ??
	// carry slot
	// attack slot
	// 

	private void Start ()
	{

		bindActions = new Dictionary<char, Protocol.protocol_event>()
		{
			{ 'M', MovePlayer }
		};

		Protocol.ProtocolHandler.Inst.BindDict( bindActions );
	}

	public virtual void Init( int pid )
	{
		playerId = pid;
	}

	public void MovePlayer( Protocol.BaseProtocol proto)
	{
		Protocol.MovePlayer movePlayer = proto.AsType<Protocol.MovePlayer>();

		if ( movePlayer.player_id == playerId )
		{

			clientAgent.MoveAgent( movePlayer.Position );
			currentAction = clientAgent;
		}

	}

	public virtual void CompleatAction()
	{
		currentAction = null;
	}

	private void OnDestroy ()
	{
		Protocol.ProtocolHandler.Inst.UnbindDict( bindActions );
	}

}
