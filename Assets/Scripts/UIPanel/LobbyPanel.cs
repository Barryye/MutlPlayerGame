/*
 * FileName:     LobbyPanel
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-16-12:34:37
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using GameFrameWork.UI;
using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : UGUIPanel 
{

    private Transform chatPanel;
    private Button exitBtn, chatBtn, sendBtn;
    private LobbyRequest lobbyRequest;
    private InputField InputField;

    private void Start()
    {
        chatPanel = transform.Find("ChatPanel");
        chatBtn = transform.GetComponent<Button>("ChatBtn");
        chatBtn.onClick.AddListener(ChatBtnCall);
        transform.GetComponent<Button>("ChatPanel/Scroll View/InputField/SendBtn").onClick.AddListener(SendChatCall);
        InputField = transform.GetComponent<InputField>("ChatPanel/Scroll View/InputField");
        lobbyRequest = GameObject.FindObjectOfType<LobbyRequest>();

    }


    private void ChatBtnCall()
    {
        chatPanel.gameObject.SetActive(true);
        chatBtn.gameObject.SetActive(false);
    }

    private void SendChatCall()
    {
        lobbyRequest.SendChat(InputField.text);
    }

    public void GetMessage(Mainpack pack)
    {

    }
}