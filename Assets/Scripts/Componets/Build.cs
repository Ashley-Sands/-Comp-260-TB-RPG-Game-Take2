using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Build : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ServerObject objPrefab;
    [SerializeField] private int buildItems = 3;
    [SerializeField] private TextMeshProUGUI buildText;
    [SerializeField] private float spawnOffset = 2;
    [SerializeField] private float yPosition = 1.5f;

    private void Start ()
    {
        UpdateUi();
    }

    public void BuildObject( int objId )
    {

        Vector3 spwanPosition = transform.position + ( transform.forward * spawnOffset );
        spwanPosition.y = yPosition;

        ServerObject so = Instantiate( objPrefab, spwanPosition, Quaternion.identity );

        so.serverObjectId = objId;
        so.Send(true);

        playerManager.CompleatAction();

    }

    private void UpdateUi()
    {
        if ( buildText != null )
            buildText.text = buildItems.ToString();
    }
}
