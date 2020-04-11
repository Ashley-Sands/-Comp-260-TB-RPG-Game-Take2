using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{

	protected int playerId = -1;  // < 0 is unset 
	public int PlayerId => playerId;

	[SerializeField] private ClientAgent clientAgent;
	[SerializeField] private ItemHold itemHold;
	[SerializeField] private ProjectileLauncher projectileLauncher;
	[SerializeField] private Health health;
	[SerializeField] private Lookat_Position lookAtPosition;
	[SerializeField] private Build build;						// this should on be present on the play

	protected ClientAction currentAction;

	public Dictionary<char, Protocol.protocol_event> bindActions;

	protected virtual void Start ()
	{

		bindActions = new Dictionary<char, Protocol.protocol_event>()
		{
			{ 'M', MovePlayer },
			{ 'P', CollectItem },
			{ 'A', Action },
			{ 'D', SetHealth },
			{ 'R', LookAt },
			{ 'B', BuildObject }
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

		print( "Move" + movePlayer.player_id + "==" + playerId + " :: "+ transform.name );

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

	public void SetHealth( Protocol.BaseProtocol proto )
	{

		Protocol.ApplyDamage damage = proto.AsType<Protocol.ApplyDamage>();

		if ( damage.player_id != playerId ) return;

		if ( damage.kill )
		{
			health.Kill();
			GameCtrl.Inst.KillPlayer( playerId );
		}
		else    // let's do some damage boys....
		{
			health.SetHealth( damage.health );
		}
	}

	public void LookAt ( Protocol.BaseProtocol protocol )
	{

		Protocol.LookAtPosition lookAtPos = protocol.AsType<Protocol.LookAtPosition>();

		if ( lookAtPos.player_id != playerId ) return;

		lookAtPosition.LookAtPosition( lookAtPos.Position );

	}

	public void BuildObject( Protocol.BaseProtocol protocol )
	{

		Protocol.BuildObject buildObj = protocol.AsType<Protocol.BuildObject>();

		if ( buildObj.player_id != playerId ) return;

		build.BuildObject( buildObj.obj_id );
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
