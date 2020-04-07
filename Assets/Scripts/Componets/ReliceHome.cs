using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReliceHome : MonoBehaviour
{
    
    [SerializeField] private int compleatRelices = 4;
    private int currentRelices = 0;

    [SerializeField] private TextMeshProUGUI relicText;

    private void OnTriggerEnter ( Collider other )
    {
        if ( other.CompareTag( "Relic" ) )
        {
            ++currentRelices;

            if ( currentRelices == compleatRelices )
            {
                // compleat game.
            }

        }
    }

    private void OnTriggerExit ( Collider other )
    {
        if ( other.CompareTag( "Relic" ) )
        {
            --currentRelices;
        }
    }

    private void UpdateUi()
    {
        if ( relicText != null )
            relicText.text = currentRelices.ToString();
    }
}
