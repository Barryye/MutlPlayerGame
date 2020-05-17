/*
 * FileName:     FindRoomRequest
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-13-15:50:46
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class FindRoomRequest : BaseRequest 
{
    private Mainpack pack = null;
    
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.FindRoom;
        base.Awake();
    }

    public override void OnResponse(Mainpack pack)
    {
        this.pack = pack;
    }
    public void SendRequest()
    {
        Mainpack pack = new Mainpack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        pack.Str = "r";

        base.SendRequest(pack);
    }
}