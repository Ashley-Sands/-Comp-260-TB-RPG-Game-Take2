﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	[SerializeField] GameObject explosionPrefab;

	private void OnCollisionEnter ( Collision collision )
	{
		if ( explosionPrefab == null )
			return;

		print( collision.collider.name );

		// exploade.
		Vector3 hitPoint = collision.contacts[ 0 ].point;

		Instantiate( explosionPrefab, hitPoint, Quaternion.identity );

		Destroy( gameObject );

		// Tell the server iv exploded.
		// TODO: ^^
		// or should the explosion tell the server :?

	}

}
