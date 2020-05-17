/*
 * FileName:     CreateRoomRequest
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-13-15:33:12
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class CreateRoomRequest : BaseRequest 
{


    private Mainpack pack = null;
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public override void OnResponse(Mainpack pack)
    {
        this.pack=pack;
    }
    public void SendRequest(string roomName,int maxNum)
    {
        Mainpack pack = new Mainpack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.Roomname = roomName;
        roomPack.Maxnum = maxNum;
        pack.Roompack.Add(roomPack);
        pack.Str = "r";
        
        base.SendRequest(pack);
    }
}