using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GL_GameCountDownTimer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private Transform timerHold;
    private Vector2 timerHoldStartPosition;
    [SerializeField] private float timerHoldYLerpAmount = 50;
    [SerializeField] private float timerHoldLerpTime = 0.5f;

    [SerializeField] private GameObject upNextTextHold;
    [SerializeField] private GameObject timesUpTextHold;

    private Coroutine timerCourt;

    void Start()
    {
        GameCtrl.Inst.gameLoopEvent += UpdateGameInfo;
        timerHoldStartPosition = timerHold.position;
    }

    private void UpdateGameInfo( Protocol.GameLoop.Actions gameAction, int time )
    {
        // update the timer
        if ( timerCourt == null )
        {
            timerCourt = StartCoroutine( Timer( time ) );
        }
        else
        {
            StopCoroutine( timerCourt );
            timerCourt = StartCoroutine( Timer( time ) );
        }

        // Lerp in the change and end turn pannel.
        switch ( gameAction )
        {
            case Protocol.GameLoop.Actions.End:     // lerp in on end
                StartCoroutine( LerpTimerHold( true ) );
                // togle the correct text
                timesUpTextHold.SetActive( true );
                upNextTextHold.SetActive( false );
                break;
            case Protocol.GameLoop.Actions.Start:   // lerp out on start
                StartCoroutine( LerpTimerHold( false ) );
                // togle the correct text
                timesUpTextHold.SetActive( false );
                upNextTextHold.SetActive( true );
                break;
        }

    }

    private IEnumerator Timer( float ttl )
    {

        while ( ttl > -1 )
        {

            timeText.text = string.Format( "{0}", ttl);

            yield return new WaitForSeconds( 1f );
            ttl -= 1f;

        }

        timeText.text = string.Format( "{0}", 0 ); ;

    }

    private IEnumerator LerpTimerHold (bool scrollIn)
    {
        float offset = 0;
        float timeElapsed = 0;
        float updateRate = 1f / 60f;
        YieldInstruction wait = new WaitForSeconds( updateRate );

        while ( timeElapsed > 0f)
        {

            yield return wait;
            timeElapsed += updateRate;

            if ( scrollIn )
                offset = Mathf.Lerp( 0, timerHoldYLerpAmount, timeElapsed / timerHoldLerpTime );
            else
                offset = Mathf.Lerp( timerHoldYLerpAmount, 0, timeElapsed / timerHoldLerpTime );

            timerHold.position = timerHoldStartPosition + new Vector2( 0, offset );

        }

    }

    private void OnDestroy ()
    {
        GameCtrl.Inst.gameLoopEvent -= UpdateGameInfo;
    }


}
