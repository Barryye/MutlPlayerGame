/*
 * FileName:     RoomRequest
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-14-15:56:59
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class RoomRequest : BaseRequest 
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        EventManger.AddListener<Mainpack>(ActionCode.FindRoom, FindRoomCallBack);
        EventManger.AddListener<Mainpack>(ActionCode.CreateRoom, CreateRoomCallBack);
        EventManger.AddListener<Mainpack>(ActionCode.JoinRoom, JoinRoomCallBack);
        EventManger.AddListener<Mainpack>(ActionCode.Exit, ExitRoomCallBack);
    }

    public override void OnDestroy()
    {
        EventManger.RemoveListener<Mainpack>(ActionCode.FindRoom, FindRoomCallBack);
        EventManger.RemoveListener<Mainpack>(ActionCode.CreateRoom, CreateRoomCallBack);
        EventManger.RemoveListener<Mainpack>(ActionCode.JoinRoom, JoinRoomCallBack);
        EventManger.RemoveListener<Mainpack>(ActionCode.Exit, ExitRoomCallBack);
        base.OnDestroy();
    }

    public void CreateRoom(string name, int maxNum)
    {
        
    }
    private void CreateRoomCallBack(Mainpack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.ReturnNone:
                break;
            case ReturnCode.Succeed:
                break;
            case ReturnCode.Fail:
                break;
            case ReturnCode.NotRoom:
                break;
            default:
                break;
        }
    }

    public void FindRoom()
    {
        Mainpack pack = new Mainpack();
        pack.Actioncode = ActionCode.FindRoom;
        SendRequest(pack);
    }
    private void FindRoomCallBack(Mainpack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.ReturnNone:
                break;
            case ReturnCode.Succeed:
                break;
            case ReturnCode.Fail:
                break;
            case ReturnCode.NotRoom:
                break;
            default:
                break;
        }
    }

    public void JoinRoom(string name)
    {
        Mainpack pack = new Mainpack();
        RoomPack room = new RoomPack();
        room.Roomname = name;
        pack.Actioncode = ActionCode.JoinRoom;
        pack.Roompack.Add(room);
        SendRequest(pack);
    }
    private void JoinRoomCallBack(Mainpack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.ReturnNone:
                break;
            case ReturnCode.Succeed:
                break;
            case ReturnCode.Fail:
                break;
            case ReturnCode.NotRoom:
                break;
            default:
                break;
        }
    }

    public void ExitRoom()
    {
        Mainpack pack = new Mainpack();
        pack.Actioncode = ActionCode.Exit;
        SendRequest(pack);
    }
    private void ExitRoomCallBack(Mainpack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.ReturnNone:
                break;
            case ReturnCode.Succeed:
                break;
            case ReturnCode.Fail:
                break;
            case ReturnCode.NotRoom:
                break;
            default:
                break;
        }
    }

    public void Chat(string str)
    {

    }
    public void ChatCallBack(Mainpack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.ReturnNone:
                break;
            case ReturnCode.Succeed:
                break;
            case ReturnCode.Fail:
                break;
            case ReturnCode.NotRoom:
                break;
            default:
                break;
        }
    }
}