using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ammo : MonoBehaviour
{

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int currentAmmo = 15;
    [SerializeField] private TextMeshProUGUI ammoText;

    private void UpdateUi()
    {
        if ( ammoText != null )
            ammoText.text = currentAmmo.ToString();
    }
    
}
