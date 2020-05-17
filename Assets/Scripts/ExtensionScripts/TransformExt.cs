/*
 * FileName:     TransformExt
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-03-05-16:27:07
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using System;
public static class TransformExt 
{
	public static Transform SetScale(this Transform transform,float X,float Y,float Z)
    {
        transform.localScale = new Vector3(X,Y,Z);
        return transform;
    }

    public static Transform SetSameScale(this Transform transform,float Scale)
    {
        transform.SetScale(Scale, Scale, Scale);
        return transform;
    }

    public static T GetComponent<T>(this Transform transform, string path)
    {
        Transform tran = transform.Find(path);
        T t;
        if (tran != null)
        {
            t = tran.GetComponent<T>();
            if (t != null)
            {
                return t;
            }
            else
                Debug.LogError("没有" + t + "组件");
        }
        else
        {
            Debug.LogError("路径错误");
        }
        return default(T);
    }




}