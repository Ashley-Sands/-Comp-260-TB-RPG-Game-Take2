using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{

    [Tooltip("the spwan id is where player of player id will be spwaned :) ")]
    [SerializeField] private int spawnId = -1;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject clientPrefab;

    void Awake()
    {

        GameCtrl.Inst.gameClientsSet += SpwanPlayers;

    }

    private void SpwanPlayers( Client[] clients )
    {

        

        for (int i = 0; i < clients.Length; i++ )
        {
            Client client = clients[ i ];
            GameObject spawnedClient;
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

                return;

            }

        }

    }
    private void OnDestroy ()
    {

        GameCtrl.Inst.gameClientsSet -= SpwanPlayers;

    }

}
