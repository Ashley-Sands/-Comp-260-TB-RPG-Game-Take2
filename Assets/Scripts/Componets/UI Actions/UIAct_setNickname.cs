using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAct_setNickname : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start ()
    {

        text.SetText( GameCtrl.Inst.playerData.nickname );

    }
}
