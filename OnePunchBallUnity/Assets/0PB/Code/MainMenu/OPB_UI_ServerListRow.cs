using System.Collections;
using System.Collections.Generic;
using TMPro;
using UdpKit;
using UnityEngine;

public class OPB_UI_ServerListRow : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txt_ServerName;

    private UdpSession session;
    
    public void Initialize(UdpSession sess)
    {
        session = sess;

        txt_ServerName.text = session.HostName;
    }

    public void OnClickJoin()
    {
        OPB_Bolt_GlobalEventsListener.OnSessionsReceived.RemoveAllListeners();
        BoltNetwork.Connect(session);
    }
}
