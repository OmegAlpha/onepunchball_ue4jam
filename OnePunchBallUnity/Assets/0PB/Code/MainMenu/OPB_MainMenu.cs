using System;
using System.Collections;
using System.Collections.Generic;
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
    
    
    private void Start()
    {
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

    private void Update()
    {
        GlobalEvents.USERNAME = txtUsername.text;
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
        BoltLauncher.StartServer();
        panel_Main.SetActive(false);
    }
    
    public void OnClick_GoServersList()
    {
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
}
