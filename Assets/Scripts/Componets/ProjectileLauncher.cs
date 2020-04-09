using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private ClientManager clientManager;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int currentAmmo = 15;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private bool ignoreCollider = true;

    private void Start ()
    {
        UpdateUi();
    }

    public void LaunchProjectile()
    {

        Vector3 spawnLocation = transform.position + transform.forward;     // spawn 1 unit infront of the player.
        Vector3 spawnRot = transform.eulerAngles;
        spawnRot.x -= 25;    // make it point up a lil

        Quaternion quater = Quaternion.Euler( spawnRot );

        GameObject go = Instantiate( projectilePrefab, spawnLocation, quater );

        if ( ignoreCollider )
            Physics.IgnoreCollision( go.GetComponent<Collider>(), GetComponent<Collider>(), true );

        --currentAmmo;

        UpdateUi();

        clientManager.CompleatAction(); //TODO: i think it shold compleat once the projectile exploads.

    }

    private void UpdateUi()
    {
        if ( ammoText != null )
            ammoText.text = currentAmmo.ToString();
    }
    
}
