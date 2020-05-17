/*
 * FileName:     ChatRequest
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-14-11:15:58
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class ChatRequest : BaseRequest 
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void SendRequest(Mainpack pack)
    {
        base.SendRequest(pack);
    }

    public override void OnResponse(Mainpack pack)
    {
        base.OnResponse(pack);
    }

}