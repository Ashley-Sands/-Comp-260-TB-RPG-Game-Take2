using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GL_CountDown : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeText;
    private Coroutine court;

    void Start()
    {
        GameCtrl.Inst.gameLoopEvent += SetTimer;
    }

    private void SetTimer( Protocol.GameLoop.Actions gameAction, int time )
    {
        if (court == null)
            court = StartCoroutine( Timer( Time.time + time ) );
    }

    private IEnumerator Timer( float time )
    {
        
        while ( Time.time < time )
        {

            timeText.text = Mathf.FloorToInt( time - Time.time ) + "Seconds";

            yield return new WaitForSeconds( 1 );

        }

        timeText.text = " 0 Seconds";
        court = null;

    }

    private void OnDestroy ()
    {
        GameCtrl.Inst.gameLoopEvent -= SetTimer;
    }


}
