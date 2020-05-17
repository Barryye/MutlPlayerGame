/*
 * FileName:     BaseManager
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-12-17:19:17
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class BaseManager 
{

    protected GameFaced face;

    public BaseManager(GameFaced faced)
    {
        face = faced;
    }
    public virtual void OnInit()
    {

    }

    public virtual void OnDestory()
    {

    }
}