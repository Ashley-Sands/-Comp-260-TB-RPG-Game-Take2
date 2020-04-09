using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	[SerializeField] GameObject explosionPrefab;
	[SerializeField] bool sendExplosionMessage = true;

	private void OnCollisionEnter ( Collision collision )
	{
		if ( explosionPrefab == null )
			return;

		print( collision.collider.name );

		// exploade.
		Vector3 hitPoint = collision.contacts[ 0 ].point;

		Instantiate( explosionPrefab, hitPoint, Quaternion.identity );

		Destroy( gameObject );

		if ( sendExplosionMessage )
		{
			new Protocol.Explosion()
			{
				Position = transform.position
			}.Send();
		}

	}

}
