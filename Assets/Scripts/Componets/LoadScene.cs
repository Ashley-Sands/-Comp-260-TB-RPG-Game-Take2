using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public enum LoadEvent{ Start, SceneRequestProtocol }
    [SerializeField] private string sceneToLoad;
    [SerializeField] private LoadEvent loadOn = LoadEvent.Start;

    void Start ()
    {

        switch ( loadOn )
        {

            case LoadEvent.Start:
                SceneManager.LoadScene( sceneToLoad, LoadSceneMode.Single );
                break;
            case LoadEvent.SceneRequestProtocol:
                Protocol.ProtocolHandler.Inst.Bind( 's', ChangeSceneRequest );
                break;

        }
    }

    private void ChangeSceneRequest( Protocol.BaseProtocol proto)
    {
        Protocol.SceneRequest sceneRequest = proto.AsType<Protocol.SceneRequest>();

        SceneManager.LoadScene( sceneRequest.scene_name, LoadSceneMode.Single );
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene( sceneToLoad, LoadSceneMode.Single );
    }

    private void OnDestroy ()
    {
        switch ( loadOn )
        {
            case LoadEvent.SceneRequestProtocol:
                Protocol.ProtocolHandler.Inst.Unbind( 's', ChangeSceneRequest );
                break;
        }
    }

}
