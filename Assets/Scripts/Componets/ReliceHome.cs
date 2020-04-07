using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReliceHome : MonoBehaviour
{
    
    [SerializeField] private int compleatRelices = 4;
    private int currentRelices = 0;

    [SerializeField] private TextMeshProUGUI relicText;

    private void Start ()
    {
        UpdateUi();
    }

    private void OnTriggerEnter ( Collider other )
    {
        if ( other.CompareTag( "Relic" ) )
        {
            ++currentRelices;
            UpdateUi();

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
            UpdateUi();
        }
    }

    private void UpdateUi()
    {

        if ( relicText != null )
            relicText.text = string.Format("{0} of {1}", currentRelices, compleatRelices);

    }
}
