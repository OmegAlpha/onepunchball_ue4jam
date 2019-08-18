using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OPB_UI_PlayerRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtUserName;
    
    [SerializeField]
    private TextMeshProUGUI txtPosition;
    
    [SerializeField]
    private TextMeshProUGUI txtPing;

    public void Initialize(OPB_PlayerController player, int pos)
    {
        txtPosition.text = player.state.Score.ToString();
        
        txtUserName.text = player.state.UserName;

        txtPing.text = player.state.ServerPing.ToString("F0");
    }
}
