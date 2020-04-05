using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : ClientManager
{

	[SerializeField] private UiActionGroup[] uiActions;
	[SerializeField] private Transform pressedMarker;

	private int activeUiGroup = -1;		// < 0 is none active.
	[SerializeField] private LayerMask layerMask;
	private HitLocation hitLocation;

	private List<GameObject> actionQueue;

	private void Awake ()
	{
		
		// make sure all the ui is not active
		foreach ( UiActionGroup uia in uiActions )
		{
			uia.uiHold.SetActive( false );
		}

	}

	private void Update ()
	{

		// this should on be active when this player is the current player
		//if ( !GameCtrl.Inst.CurrentClientIsPlayer ) return;

		if ( Input.GetMouseButtonDown( 0 ) )
		{

			// player input.
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );

			if ( Physics.Raycast( ray, out hit, 300, layerMask ) )
			{
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
