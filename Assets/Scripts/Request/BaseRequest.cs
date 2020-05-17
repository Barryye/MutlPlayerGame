/*
 * FileName:     BaseRequest
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-12-18:25:57
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class BaseRequest :MonoBehaviour
{
    protected RequestCode requestCode;
    public RequestCode GetRequestCode
    {
        get { return requestCode; }
    }

    protected ActionCode actionCode;
    public ActionCode GetActionCode
    {
        get { return actionCode; }
    }

    protected GameFaced face;
    public virtual void Awake()
    {
    
    }
    public virtual void Start()
    {
        face = GameFaced.Face;
        face.AddRequest(this);
        Debug.Log("添加:" + actionCode.ToString());
    }

    public virtual void OnDestroy()
    {
        face.RemoveRequest(requestCode);
    }
    /// <summary>
    /// 客户端接收回调
    /// </summary>
    /// <param name="pack"></param>
    public virtual void OnResponse(Mainpack pack)
    {
        EventManger.Broadcast<Mainpack>(pack.Actioncode, pack);
    }
    /// <summary>
    /// 客户端发送消息
    /// </summary>
    /// <param name="pack"></param>
    public virtual void SendRequest(Mainpack pack)
    {
        face.Send(pack);
        Debug.Log("发送数据");
    }
}