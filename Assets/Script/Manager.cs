using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using TMPro;

public class Manager : NetworkManager
{
    public string myNick;
    Transform textArea;
    NetworkClient myClient;
    private void Start()
    {
      
    }

    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }

    public void EnterChat(string a)
    {
        if (textArea == null)
        {
            textArea = GameObject.Find("PlayerUI").transform.Find("ScrollArea").Find("TextContainer").Find("Text");
        }
        textArea.GetComponent<TextMeshProUGUI>().text = textArea.GetComponent<TextMeshProUGUI>().text + "\n" + a;
        textArea.GetComponent<RectTransform>().sizeDelta = new Vector2(232.5f, textArea.GetComponent<TextMeshProUGUI>().preferredHeight);
    }

 


    //채팅 받는 기능
    public class MyMsgType
    {
        public static short Chat = MsgType.Highest + 1; // 메세지 타입이 47개가 있는데 유저 커스텀 타입은 Highest보다 높아야함
    }
    
    private class ChatMessage : MessageBase
    {
        public string nickname;
        public string message;
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        myClient = client;
        myClient.RegisterHandler(MyMsgType.Chat, OnChatMessage);
    }

    public void OnChatMessage(NetworkMessage netMsg)
    {
        ChatMessage msg = netMsg.ReadMessage<ChatMessage>();
        EnterChat(msg.nickname + ": " + msg.message);  
    }
    //

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    //서버 시작,접속에 관한 함수
    public void ServerHost()
    {
        var nick_inputField = GameObject.Find("Canvas").transform.Find("Nick_InputField").GetComponent<TMP_InputField>();
        if (nick_inputField.text != "")
        {
            myNick = nick_inputField.text;
            StartHost();
        }
    }
    public void ConnectToServer()
    {
        var ip_inputField = GameObject.Find("Canvas").transform.Find("IP_InputField").GetComponent<TMP_InputField>();
        var nick_inputField = GameObject.Find("Canvas").transform.Find("Nick_InputField").GetComponent<TMP_InputField>();
        if (nick_inputField.text != "")
        {
            myNick = nick_inputField.text;
            if (ip_inputField.text == "")
            {
                print("접속합니다");
                networkAddress = "45.112.159.4";
            }
            else
            {
                networkAddress = ip_inputField.text;
            }
        }
        networkPort = 7777;
        StartClient();
    }
}
