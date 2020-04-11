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

    public bool alive = true;

    public Client( int _clientId, string _nickname, int _playerId )
    {
        clientId = _clientId;
        nickname = _nickname;
        playerId = _playerId;
    }

}

/// <summary>
/// Storage for this player
/// </summary>
[System.Serializable]
public class Player : Client
{

    public string reg_key;

    // we dont need the player id in the constructor as player is setup befor the game is set.
    // it gets updated once the game starts :)
    public Player( int _clientId, string _nickname, string _regKey ) :
        base( _clientId, _nickname, 0)  // default player id must be 0 to prevent local testing errors
    {
        reg_key = _regKey;
    }

    public bool compareClient( Client client )
    {
        return client.clientId == clientId;
    }

}