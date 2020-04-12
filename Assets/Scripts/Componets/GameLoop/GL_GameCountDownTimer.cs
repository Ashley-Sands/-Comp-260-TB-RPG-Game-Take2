using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GL_GameCountDownTimer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeText;

    private Coroutine court;

    void Start()
    {
        GameCtrl.Inst.gameLoopEvent += UpdateGameInfo;
    }

    private void UpdateGameInfo( Protocol.GameLoop.Actions gameAction, int time )
    {
        if ( court == null )
        {
            court = StartCoroutine( Timer( gameAction.ToString(), time ) );
        }
        else
        {
            StopCoroutine( court );
            court = StartCoroutine( Timer( gameAction.ToString(), time ) );
        }
    }

    private IEnumerator Timer( string lable, float ttl )
    {
        lable = lable + " - ({0}) ";

        while ( ttl > -1 )
        {

            timeText.text = string.Format(lable, ttl);

            yield return new WaitForSeconds( 1f );
            ttl -= 1f;

        }

        timeText.text = timeText.text = string.Format( lable, 0 ); ;

    }

    private void OnDestroy ()
    {
        GameCtrl.Inst.gameLoopEvent -= UpdateGameInfo;
    }


}
