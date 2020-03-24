using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton GameCtrl
/// </summary>
public class GameCtrl : MonoBehaviour
{

    private static GameCtrl inst;
    public static GameCtrl Inst {
        get { return inst; }
        set{
            if ( inst == null )
                inst = value;
        }
    }

    public Player playerData;   

    public Client[] clients;    // all the clients including the player.
    public int currentClientId = 0;

    void Awake()
    {
        Inst = this;
    }

    
}
