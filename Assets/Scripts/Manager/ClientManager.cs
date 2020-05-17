/*
 * FileName:     ClientManager
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-12-17:34:32
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using SocketGameProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
 
public class ClientManager : BaseManager 
{

    private Socket socket;
    private Message message;

    public bool connect;
    public ClientManager(GameFaced faced) : base(faced)
    {
        message = new Message();
    }
    

    public override void OnInit()
    {
        InitSocket();
        base.OnInit();
    }

    public override void OnDestory()
    {
        
        //message = new Message();
        Mainpack pack = new Mainpack();
        pack.Requestcode = RequestCode.User;
        pack.Actioncode = ActionCode.Close;
        face.Send(pack);
        CloseSocket();
        base.OnDestory();
        
    }

    /// <summary>
    /// 初始化socket
    /// </summary>
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            //链接成功
            socket.Connect("127.0.0.1", 6666);
            connect = socket.Connected;

            if (connect)
            {
                StartReceive();
            }
           
        }
        catch (System.Exception e)
        {
            //链接出错
            Debug.LogWarning(e);
            TipPlanel.Open("服务器链接失败");
        }
    }
    /// <summary>
    /// 关闭socket
    /// </summary>
    private void CloseSocket()
    {
        if (socket.Connected&&socket!=null)
        {
            socket.Close();
        }
    }

    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer, message.StartIndex, message.RemSize, SocketFlags.None, ReceiveCallBack, null);
    }

    private void ReceiveCallBack(IAsyncResult iar)
    {
        try
        {
            if (socket == null || socket.Connected == false) return;
            int len = socket.EndReceive(iar);
            if (len==0)
            {
                CloseSocket();
                return;
            }
            message.ReadBuffer(len, HandleResponse);
            StartReceive();
        }
        catch (Exception e)
        {

            Debug.LogWarning(e);
        }
    }

    /// <summary>
    /// socket回調
    /// </summary>
    /// <param name="pack"></param>
    private void HandleResponse(Mainpack pack)
    {
        face.HandleResponse(pack);
        Debug.Log("client处理");
    }

    public void Send(Mainpack pack)
    {
        try
        {
          int s= socket.Send(Message.PackData(pack));
            Debug.Log(s);
        }
        catch (Exception e)
        {
            connect = false;
            Debug.LogWarning(e);
        }
        
    }


}