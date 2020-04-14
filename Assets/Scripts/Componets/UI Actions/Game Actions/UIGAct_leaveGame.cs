using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_leaveGame : MonoBehaviour
{
    public void LeaveGame()
    {
        Protocol.ClientStatus status = new Protocol.ClientStatus()
        {
            StatusType = Protocol.ClientStatusType.LeaveGame,
            ok = true
        };

        status.Send();

    }
}
