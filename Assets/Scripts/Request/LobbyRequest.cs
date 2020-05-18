/*
 * FileName:     LobbyRequest
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-17-10:09:51
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using System.Collections;
using System.Collections.Generic;
using GameFrameWork.UI;
using SocketGameProtocol;
using UnityEngine;

public class LobbyRequest : BaseRequest 
{

    private LobbyPanel lobbyPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Lobby;
        EventManger.AddListener<Mainpack>(ActionCode.Chat, ChatCallBack);

    }

    public override void Start()
    {
        base.Start();
    }
    public override void OnDestroy()
    {
        EventManger.RemoveListener<Mainpack>(ActionCode.Chat, ChatCallBack);
        base.OnDestroy();
    }


    public void SendChat(string str,BasePanel panel)
    {
        Mainpack pack = new Mainpack();
        pack.Requestcode = requestCode;
        pack.Actioncode = ActionCode.Chat;
        pack.Playerpack.Add(face.m_Role);
        pack.Str = str;
        SendRequest(pack);
        if (lobbyPanel==null)
        {
            lobbyPanel = panel as LobbyPanel;
        }
    }
    private void ChatCallBack(Mainpack pack)
    {
        bool isSelf=false;
        if (pack.Playerpack[0].Playername==face.m_Role.Playername)
        {
            switch (pack.Returncode)
            {
                case ReturnCode.Succeed:
                    TipPlanel.Open("消息发送成功");
                    break;
                case ReturnCode.Fail:
                    TipPlanel.Open("消息发送失败");
                    break;
                default:
                    break;
            }
            isSelf = true;
            
        }
        lobbyPanel.GetMessage(pack,isSelf);

    }


}