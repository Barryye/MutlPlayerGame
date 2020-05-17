/*
 * FileName:     RoleManager
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-14-10:26:32
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class RoleManager : BaseManager 
{

    private Dictionary<string, PlayerPack> RoleDic;

    public int RoleCount
    {
        get { return RoleDic.Count; }
    }
    public RoleManager(GameFaced faced) : base(faced)
    {
        RoleDic = new Dictionary<string, PlayerPack>();
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }

    public void AddRole(string playername, PlayerPack player)
    {
        RoleDic.Add(playername, player);
    }

    public void RemoveRole(string playername)
    {
        RoleDic.Remove(playername);
    }


}