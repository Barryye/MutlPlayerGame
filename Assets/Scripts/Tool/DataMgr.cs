/*
 * FileName:     DataMgr
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-15-09:35:17
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class DataMgr  
{
    public const string m_projectName = "Prefab";
    public const string Item= "Prefab/UI/";



    public static string FilePath = Application.streamingAssetsPath+"/Config";


    #region NetWork
    public const string IP = "127.0.0.1";
    #endregion
}