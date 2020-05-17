/*
 * FileName:     RequestManager
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-12-18:27:16
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class RequestManager : BaseManager 
{
    public RequestManager(GameFaced faced) : base(faced) { }

    private Dictionary<RequestCode, BaseRequest> requestDict = new Dictionary<RequestCode, BaseRequest>();

    public override void OnInit()
    {
        base.OnInit();
    }
    public override void OnDestory()
    {
        base.OnDestory();
    }
    public void AddRequest(BaseRequest request)
    {
        requestDict.Add(request.GetRequestCode,request);
        Debug.Log(requestDict.Count);
    }
    public void RemoveRequest(RequestCode request)
    {
        requestDict.Remove(request);
    }

    /// <summary>
    /// 客户端回调
    /// </summary>
    /// <param name="pack"></param>
    public void HandleResponse(Mainpack pack)
    {
        if (requestDict.TryGetValue(pack.Requestcode,out BaseRequest request))
        {
            request.OnResponse(pack);
        }
        else
        {
            Debug.LogWarning("找不到对应处理");
        }
    }
}