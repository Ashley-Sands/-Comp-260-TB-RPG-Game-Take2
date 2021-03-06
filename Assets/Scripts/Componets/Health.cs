﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{

    public event System.Action HealthDepleated;    

    [SerializeField] float maxHalth = 100;
    [SerializeField] float currentHealth = 00;

    [SerializeField] TextMeshProUGUI healthText;

    private void Start ()
    {
        UpdateUi();
    }

    public void SetHealth( float health )
    {
 
        currentHealth = health;
        ClampHealth();
        UpdateUi();
    }

    public void AddHealth( float healthToAdd)
    {
        currentHealth += healthToAdd;

        ClampHealth();
        UpdateUi();

    }

    public void RemoveHealth( float healthToRemove )
    {
        currentHealth -= healthToRemove;

        ClampHealth();
        UpdateUi();

    }

    public void Kill( )
    {
        currentHealth = -1;
        UpdateUi();
        HealthDepleated?.Invoke();
    }

    void ClampHealth()
    {

        if ( currentHealth > maxHalth )
        {
            currentHealth = maxHalth;
        }
        else if ( currentHealth <= 0 )
        {
            Debug.Log( "Hmmm, it appears the server is brock :( " );
            currentHealth = 1;
        }
    }

    private void UpdateUi()
    {
        if ( healthText != null )
            if ( currentHealth < 0 )
                healthText.text = "Dead!";
            else
                healthText.text = string.Format( "{0}/{1}", currentHealth, maxHalth );

    }

}
