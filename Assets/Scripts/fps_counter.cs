using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_counter : MonoBehaviour
{

    public int frameRateLimit = -1;
    public float intervals = 2f;
    private float currentInterval = 0f;

    private int frameCount;

    private float lastFPS;
    private float avgDelta;

    private void Start ()
    {
        if ( frameRateLimit > 0 )
            Application.targetFrameRate = frameRateLimit;
    }

    void Update()
    {

        currentInterval += Time.deltaTime;
        ++frameCount;

        if ( currentInterval >= intervals )
        {
            
            avgDelta = currentInterval / frameCount;
            lastFPS = 1f / avgDelta;

            print( frameCount );

            frameCount = 0;
            currentInterval = 0f;
        }

    }

    private void OnGUI ()
    {

        GUI.Box( new Rect( 0, Screen.height - 35, 250, 35 ), string.Format( "{0}fps ({1}ms)", lastFPS, avgDelta ) );

    }
}
