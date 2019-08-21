 using Bolt.Matchmaking;
 using ExitGames.Client.Photon;
 using Photon.Chat;
 using TMPro;
 using UnityEngine;
 using UnityEngine.UI;

public class OPB_UI_ChatPanel : MonoSingleton<OPB_UI_ChatPanel>, IChatClientListener
{
    [SerializeField]
    private TMP_InputField txtInput_Chat;
    
    [SerializeField]
    private GameObject txtConnectingToChat;
    
    [SerializeField]
    private Text txtChat;
    
    public ChatClient chatClient;

    public bool isTyping = false;
    
    private void Start()
    {
        txtInput_Chat.onSubmit.AddListener(OnTextSubmitted);
        txtInput_Chat.onDeselect.AddListener((action) =>
        {
            DeactivateDoChat();
        });
        
        txtConnectingToChat.gameObject.SetActive(true);
        txtInput_Chat.gameObject.SetActive(false);

        txtChat.supportRichText = true;
        txtChat.text = "";
    }

    public void Initialize()
    {
        chatClient = new ChatClient(this);
        chatClient.ChatRegion = "US";
        var authValues = new AuthenticationValues(OPB_LocalUserInfo.UserName);

        //TODO: put the id in some other placer or check if Bolt has an access to this
        chatClient.Connect(ChatSettings.Instance.AppId, "1.0", authValues);

        DeactivateDoChat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ActivateDoChat();
        }

        txtInput_Chat.gameObject.SetActive(isTyping);
        
        chatClient?.Service();
    }

    private void ActivateDoChat()
    {
        if (!txtInput_Chat.gameObject.activeSelf)
        {
            isTyping = true;
            txtInput_Chat.gameObject.SetActive(true);
            txtInput_Chat.Select();
            txtInput_Chat.ActivateInputField();
            
            OPB_PlayerController.LocalInstance.ToggleInputEnabled(false);
        }
    }

    private void DeactivateDoChat()
    {
        isTyping = false;
        txtInput_Chat.DeactivateInputField();
        txtInput_Chat.text = "";
        
        OPB_PlayerController.LocalInstance?.ToggleInputEnabled(true);
    }

    private void OnTextSubmitted(string arg0)
    {
        chatClient.PublishMessage("gamechannel", txtInput_Chat.text);

        DeactivateDoChat();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnDisconnected()
    {
        txtConnectingToChat.gameObject.SetActive(true);
        txtInput_Chat.gameObject.SetActive(false);
    }

    public void OnConnected()
    {
        txtConnectingToChat.gameObject.SetActive(false);
        txtInput_Chat.gameObject.SetActive(true);
        
        chatClient.Subscribe("gamechannel");
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for ( int i = 0; i < senders.Length; i++ )
        {
            msgs = string.Format("{0}{1}={2}, ", msgs, senders[i], messages[i]);

            /// txtChat.text = "<color=blue>" + senders[i] + "</color> " + messages[i];
        }
        
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (found)
        {
            txtChat.text = channel.ToStringMessages();
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.LogError(channelName);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.LogError("subscribed " + user);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}
