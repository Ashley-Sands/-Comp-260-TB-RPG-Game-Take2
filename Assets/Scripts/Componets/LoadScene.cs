using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public enum LoadEvent{ Start }
    [SerializeField] private string sceneToLoad;
    [SerializeField] private LoadEvent loadOn = LoadEvent.Start;

    void Start ()
    {

        switch ( loadOn )
        {

            case LoadEvent.Start:
                SceneManager.LoadScene( sceneToLoad, LoadSceneMode.Single );
                break;

        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene( sceneToLoad, LoadSceneMode.Single );
    }

    private void OnDestroy ()
    {
        switch ( loadOn )
        {
        }
    }

}
