using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UdpKit;
using UnityEngine;

public class OPB_MainMenu : MonoSingleton<OPB_MainMenu>
{
    [SerializeField]
    private GameObject panel_Main;
    
    [SerializeField]
    private GameObject panel_Join;
    
    [SerializeField]
    private Transform container_ServersList;
    
    [SerializeField]
    private OPB_UI_ServerListRow prefab_sessionRow;
    
    [SerializeField]
    private TextMeshProUGUI txtUsername;
    
    [SerializeField]
    private TextMeshProUGUI txtUsername_Error;
    
    private void Start()
    {
        txtUsername.text = PlayerPrefs.GetString("userName", "");
        txtUsername_Error.text = "";
        
        if (BoltNetwork.IsRunning)
        {
            try
            {
                foreach (var connection in BoltNetwork.Connections)
                {
                    connection.Disconnect();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
        OPB_Bolt_GlobalEventsListener.OnSessionsReceived.AddListener(OnSessionsReceived);
        
        panel_Main.SetActive(true);
        panel_Join.SetActive(false);
        
    }

    private void OnSessionsReceived(List<UdpSession> sessionsList)
    {
        while(container_ServersList.childCount > 0)
        { 
            Destroy(container_ServersList.GetChild(0).gameObject);  
        }

        foreach (var session in sessionsList)
        {
            OPB_UI_ServerListRow newRow = Instantiate(prefab_sessionRow, container_ServersList);
            newRow.Initialize(session);
        }
    }

    public void OnClick_Host()
    {
        if(!CheckNameRules())
            return;
        
        BoltLauncher.StartServer();
        panel_Main.SetActive(false);
    }
    
    public void OnClick_GoServersList()
    {
        if(!CheckNameRules())
            return;
        
        panel_Main.SetActive(false);
        panel_Join.SetActive(true);
        OnClick_RefreshList();
    }
    
    public void OnClick_RefreshList()
    {
        BoltLauncher.StartClient();
    }

    public void OnClick_CancelJoinList()
    {
        panel_Main.SetActive(true);
        panel_Join.SetActive(false);
    }

    public bool CheckNameRules()
    {
        bool lenghtOk = ToolBox.IntBetween(4, 13, txtUsername.text.Length);

        if (!lenghtOk)
        {
            txtUsername_Error.text = "UserName be 4 to 13 characters long";
            return false;
        }

        txtUsername_Error.text = "";

        OPB_LocalUserInfo.UserName = txtUsername.text;
        
        PlayerPrefs.SetString("userName", txtUsername.text);

        return true;
    }
}
