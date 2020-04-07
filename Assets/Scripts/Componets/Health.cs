using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public event System.Action HealthDepleated;    

    [SerializeField] float maxHalth = 100;
    [SerializeField] float currentHealth = 00;

    void AddHealth( float healthToAdd)
    {
        currentHealth += healthToAdd;

        ClampHealth();

    }

    void RemoveHealth( float healthToRemove )
    {
        currentHealth -= healthToRemove;

        ClampHealth();
    }

    void ClampHealth()
    {

        if ( currentHealth > maxHalth )
            currentHealth = maxHalth;
        else if ( currentHealth < 0 )
            HealthDepleated?.Invoke();

    }


}
