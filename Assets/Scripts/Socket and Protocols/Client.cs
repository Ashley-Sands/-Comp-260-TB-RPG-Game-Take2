using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stoage for other players
/// </summary>
[System.Serializable]
public class Client
{

    public int clientId;        // this is there id in the database
    public string nickname;

    public int playerId;        // In Game

}

/// <summary>
/// Storage for this player
/// </summary>
[System.Serializable]
public class Player : Client
{

    public string reg_key;

    public bool compareClient( Client client )
    {
        return client.clientId == clientId;
    }

}