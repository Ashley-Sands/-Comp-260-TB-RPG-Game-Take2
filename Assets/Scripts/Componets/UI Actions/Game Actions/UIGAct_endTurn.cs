using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGAct_endTurn : MonoBehaviour
{
    public void EndTurn()
    {
        Protocol.GameAction action = new Protocol.GameAction( Protocol.GameAction.Actions.EndTurn );
        action.Send();
    }
}
