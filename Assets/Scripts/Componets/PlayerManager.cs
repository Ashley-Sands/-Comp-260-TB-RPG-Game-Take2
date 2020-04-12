using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : ClientManager
{

	[SerializeField] private ServerObject serverObject;
	[SerializeField] private UiActionGroup[] uiActions;
	public Transform pressedMarker;

	private int activeUiGroup = -1;		// < 0 is none active.
	[SerializeField] private LayerMask layerMask;
	[Tooltip("the amount of pixals from the left of the scene in witch raycast will be ignored.")]
	[SerializeField] private int x_rayBlock = 150;
	private int XRayBlock => (int)( x_rayBlock * (float)(Screen.height / 1080f) );
	private HitLocation hitLocation;
	public HitLocation HitLocation { get => hitLocation; }

	private Queue<Protocol.BaseGameAction> actionQueue = new Queue<Protocol.BaseGameAction>();
	private Protocol.BaseGameAction nextAction;

	private ServerObject selectedServerObject;
	public ServerObject SelectedServerObject => selectedServerObject;

	private Transform selectedObject;
	public Transform SelectedObject => selectedObject;

	private void Awake ()
	{
		
		// make sure all the ui is not active
		foreach ( UiActionGroup uia in uiActions )
		{
			uia.uiHold.SetActive( false );
		}

		GameCtrl.Inst.gameLoopEvent += GameLoopUpdate;

	}

	protected override void Start ()
	{
		base.Start();

		// first things first update our position on the server.
		serverObject.Send();
	}

	private void Update ()
	{

		if ( pressedMarker == null || !GameCtrl.Inst.CurrentClientIsPlayerAndActive ) return;

		// this should on be active when this player is the current player
		//if ( !GameCtrl.Inst.CurrentClientIsPlayer ) return;

		if ( Input.GetMouseButtonDown( 0 ) )
		{

			Vector2 mousePosition = Input.mousePosition;

			if ( mousePosition.x < XRayBlock ) return;

			// player input.
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( mousePosition );

			if ( Physics.Raycast( ray, out hit, 300, layerMask ) )
			{
				if ( hit.collider.gameObject.CompareTag( "RayBlocker" ) ) return;

				selectedServerObject = hit.collider.GetComponent<ServerObject>();
				selectedObject = hit.collider.transform;


				hitLocation.Set( hit.point, hit.collider.gameObject );

				SetUiGroup();

				Vector3 pressedLocation = hit.point;
				pressedLocation.y = 0.5f;
				pressedMarker.position = pressedLocation;

				print( hit.point + " :: " + hit.collider.gameObject.name + ", Active group: "+activeUiGroup );
			}

			
		}

	}

	private void SetUiGroup()
	{

		// find the tag in the Ui Actions.
		// if theres not switch of the current group
		// other wise set the current ui group.

		int nextUiGroup = -1;

		for ( int i = 0; i < uiActions.Length; i++ )
		{
			if ( hitLocation.obj.CompareTag( uiActions[i].tag ) )
			{
				nextUiGroup = i;
				break;
			}

		}

		if ( activeUiGroup == nextUiGroup ) return;	// no change.

		if ( activeUiGroup > -1 )	// switch off the current ui object
		{
			uiActions[ activeUiGroup ].uiHold.SetActive( false );
		}

		activeUiGroup = nextUiGroup;	// change the current object to the next

		if ( activeUiGroup > -1 )		// set the new current object active.
		{
			uiActions[ activeUiGroup ].uiHold.SetActive( true );
		}

	}

	private void GameLoopUpdate( Protocol.GameLoop.Actions action, int ttl )
	{
		// if its the end of our go, clear the queue, cancel the current action and update out position on the server :D
		if ( action == Protocol.GameLoop.Actions.End && GameCtrl.Inst.CurrentPlauerId == playerId )
		{
			print( "Clear remaining Actions" );
			ClearActions();

			currentAction?.CancelAction();
			currentAction = null;
			nextAction = null;

			serverObject.Send();	// finally update out final position on the server

		}
	}

	public override void CompleatAction ()
	{

		serverObject.Send();

		base.CompleatAction();
		nextAction = null;

		NextAction();
		print("Compleated");
	}

	/// <summary>
	/// Queues a player action. Triggering it there is no action currently
	/// </summary>
	/// <param name="gameAction"></param>
	public void QueueAction( Protocol.BaseGameAction gameAction )
	{
		print( "Action Queued (current: "+ (nextAction != null) +" :: "+ nextAction?.GetType().ToString() +")" );
		actionQueue.Enqueue( gameAction );

		if ( nextAction == null )
			NextAction();

	}

	/// <summary>
	/// triggers the plauers next action in the queue
	/// </summary>
	private void NextAction()
	{

		if ( nextAction == null && actionQueue.Count > 0 )
		{
			nextAction = actionQueue.Dequeue();
			nextAction.player_id = playerId;

			ClientSocket.ActiveSocket.SendMsg( nextAction );

			if ( nextAction.SendLocal )
				ClientSocket.ActiveSocket.LocalSendMsg( nextAction );

			Debug.LogFormat( "Sending {0} action from playerMannager :D", nextAction.Identity );

		}


	}

	public void ClearActions()
	{
		actionQueue.Clear();
	}

	private void OnDestroy ()
	{
		GameCtrl.Inst.gameLoopEvent += GameLoopUpdate;
	}

}

[System.Serializable]
public struct UiActionGroup
{
	
	public string tag;
	public GameObject uiHold;

}

[System.Serializable]
public struct HitLocation
{
	
	public Vector3 location;
	public GameObject obj;

	public void Set( Vector3 loc, GameObject go )
	{
		location = loc;
		obj = go;

	}

}
