using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Build : MonoBehaviour
{
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private int buildItems = 3;
    [SerializeField] private TextMeshProUGUI buildText;

    private void Start ()
    {
        UpdateUi();
    }

    private void UpdateUi()
    {
        if ( buildText != null )
            buildText.text = buildItems.ToString();
    }
}
