using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{

	protected int playerId = -1;  // < 0 is unset 

	[SerializeField] private ClientAgent clientAgent;
	[SerializeField] private ItemHold itemHold;
	[SerializeField] private ProjectileLauncher projectileLauncher;
	[SerializeField] private Health health;

	protected ClientAction currentAction;

	public Dictionary<char, Protocol.protocol_event> bindActions;

	private void Start ()
	{

		bindActions = new Dictionary<char, Protocol.protocol_event>()
		{
			{ 'M', MovePlayer },
			{ 'P', CollectItem },
			{ 'A', Action },
			{ 'D', TakeDamage }
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

	private void CollectItem ( Protocol.BaseProtocol proto )
	{
		Protocol.CollectItem collectItem = proto.AsType<Protocol.CollectItem>();

		if ( collectItem.player_id == playerId )
		{
			itemHold.CollectItem( collectItem.object_id );
		}

	}

	private void Action( Protocol.BaseProtocol proto )
	{
		Protocol.GameAction gameAct = proto.AsType<Protocol.GameAction>();

		if ( gameAct.player_id != playerId ) return;

		switch( gameAct.Action )
		{
			case Protocol.GameAction.Actions.DropItem:
				itemHold.DropItem();
				break;
			case Protocol.GameAction.Actions.LaunchProjectile:
				projectileLauncher.LaunchProjectile();
				break;
		}

	}

	public void TakeDamage( Protocol.BaseProtocol proto )
	{

		Protocol.ApplyDamage damage = proto.AsType<Protocol.ApplyDamage>();

		if ( damage.player_id != playerId ) return;

		health.RemoveHealth( damage.damage_amount );

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
