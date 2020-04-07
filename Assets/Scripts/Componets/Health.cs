using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{

    public event System.Action HealthDepleated;    

    [SerializeField] float maxHalth = 100;
    [SerializeField] float currentHealth = 00;

    [SerializeField] TextMeshProUGUI healthText;

    void AddHealth( float healthToAdd)
    {
        currentHealth += healthToAdd;

        ClampHealth();
        UpdateUi();

    }

    void RemoveHealth( float healthToRemove )
    {
        currentHealth -= healthToRemove;

        ClampHealth();
        UpdateUi();

    }

    void ClampHealth()
    {

        if ( currentHealth > maxHalth )
            currentHealth = maxHalth;
        else if ( currentHealth < 0 )
            HealthDepleated?.Invoke();

    }

    private void UpdateUi()
    {
        if (healthText != null)
            healthText.text = string.Format( "{0}/{1}", currentHealth, maxHalth );

    }

}
