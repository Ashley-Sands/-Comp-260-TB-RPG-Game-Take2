using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : ClientManager
{

	[SerializeField] private UiActionGroup[] uiActions;
	private int activeUiGroup = -1;
	[SerializeField] private LayerMask layerMask;
	private HitLocation hitLocation;

	private void Update ()
	{

		// this should on be active when this player is the current player
		if ( !GameCtrl.Inst.CurrentClientIsPlayer ) return;

		if ( Input.GetMouseButtonDown( 0 ) )
		{

			// player input.
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

			if ( Physics.Raycast( ray, out hit, 300, layerMask ) )
			{
				hitLocation.Set( hit.point, hit.collider.gameObject );
			}

			SetUiGroup();

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

		if ( activeUiGroup > -1 )	// switch of the current object
		{
			uiActions[ activeUiGroup ].uiHold.SetActive( false );
		}

		activeUiGroup = nextUiGroup;	// change the current object to the next

		if ( activeUiGroup > -1 )		// set the new current object active.
		{
			uiActions[ activeUiGroup ].uiHold.SetActive( true );
		}

	}


}

public struct UiActionGroup
{
	
	public string tag;
	public GameObject uiHold;

}

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
