/*
 * FileName:     UserRequest
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-14-15:23:28
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

public class UserRequest : BaseRequest 
{
    public override void Awake()
    {
        requestCode = RequestCode.User;
        EventManger.AddListener<Mainpack>(ActionCode.Login, LoginCallback);
        EventManger.AddListener<Mainpack>(ActionCode.Logon, LogonCallback);
    }
    public override void OnDestroy()
    {
        EventManger.RemoveListener<Mainpack>(ActionCode.Login, LoginCallback);
        EventManger.RemoveListener<Mainpack>(ActionCode.Logon, LogonCallback);
        base.OnDestroy();
    }
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="user"></param>
    /// <param name="pass"></param>
    /// <param name="name"></param>
    public void Login(string user, string pass)
    {
        LoginPack loginPack = new LoginPack();
        loginPack.Username = user;
        loginPack.Password = pass;

        Mainpack pack = new Mainpack();
        pack.Requestcode = requestCode;
        pack.Actioncode = ActionCode.Login;

        pack.Loginpack = loginPack;
        
        SendRequest(pack);
    }
    private void LoginCallback(Mainpack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Succeed:
                List<PlayerPack> players = DateReader.StrToObject<List<PlayerPack>>(DataMgr.FilePath + "/Role.txt");
                foreach (var item in players)
                {
                    if (item.Playername== pack.Playerpack[0].Playername)
                    {
                        face.m_Role = item;
                    }
                }
                
                UIManager.Instance.PushPanelFromRes(UIPanelName.LobbyPanel);
                break;
            case ReturnCode.Fail:
                TipPlanel.Open("登录失败");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="user"></param>
    /// <param name="pass"></param>
    public void Logon(string user, string pass, string name)
    {
        LoginPack loginPack = new LoginPack();
        loginPack.Username = user;
        loginPack.Password = pass;

        Mainpack pack = new Mainpack();
        pack.Requestcode = requestCode;
        pack.Actioncode = ActionCode.Logon;

        PlayerPack player = new PlayerPack();
        player.Playername = name;
        pack.Playerpack.Add(player);

        pack.Loginpack = loginPack;
        SendRequest(pack);
    }
    private void LogonCallback(Mainpack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.ReturnNone:
                break;
            case ReturnCode.Succeed:
                UIManager.Instance.PushPanelFromRes(UIPanelName.LoginPanel);
                PlayerPack player = pack.Playerpack[0];
                DateReader.ObjectToJson(player, DataMgr.FilePath+"/Role.txt");
                TipPlanel.Open("注册成功");
                break;
            case ReturnCode.Fail:

                TipPlanel.Open("注册失败");
                break;
            default:
                break;
        }
    }

    public void Close()
    {

    }
}