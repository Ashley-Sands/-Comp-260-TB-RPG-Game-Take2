﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    public static Color[] SpwanColours;
    private Color[] spwanColours = { Color.blue, Color.green, new Color( 1f, 0.4f, 0f ), Color.yellow };

    [Tooltip("the spwan id is where player of player id will be spwaned :) ")]
    [SerializeField] private int spawnId = -1;
    [SerializeField] private ClientManager playerPrefab;
    [SerializeField] private ClientManager clientPrefab;
    
    void Awake()
    {

        GameCtrl.Inst.gameClientsSet += SpwanPlayers;
        SpwanColours = spwanColours;
    }

    private void SpwanPlayers( Client[] clients )
    {

        for (int i = 0; i < clients.Length; i++ )
        {
            Client client = clients[ i ];
            ClientManager spawnedClient = null;
            print( "Helloo World :) :: " + client.playerId + " :: "+ spawnId );

            if ( client.playerId == spawnId )
            {
                print( " :: " );
                // check if its the player or a client
                if ( client.clientId == GameCtrl.Inst.playerData.clientId )
                {
                    spawnedClient = Instantiate( playerPrefab, transform.position, Quaternion.identity );
                }
                else
                {
                    spawnedClient = Instantiate( clientPrefab, transform.position, Quaternion.identity );
                }

                spawnedClient?.Init( spawnId );

                return;

            }

        }

    }
    private void OnDestroy ()
    {

        GameCtrl.Inst.gameClientsSet -= SpwanPlayers;

    }

}
