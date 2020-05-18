using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class DateReader 
{
    /// <summary>
    /// //从文件里面读取json数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetJsonString(string path)     
    {
        //读取Json数据
        StreamReader reader = new StreamReader(path);
        string jsonData = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        Debug.Log(jsonData);
        return jsonData;
    }
    /// <summary>
    /// 读取Json文件并转对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static T ReadJsonToObject<T>(string path)
    {
        string str = GetJsonString(path);
        var ret = JsonToObject<T>(str);
        return ret;
    }

    /// <summary>
    /// Json转对象
    /// </summary>
    /// <typeparam name="T">对象泛型</typeparam>
    /// <param name="path">Json文件目录</param>
    /// <returns></returns>
    public static T JsonToObject<T>(string str)
    {
        var ret = JsonConvert.DeserializeObject<T>(str);
        return ret;
    }
    /// <summary>
    /// 字符串转对象
    /// </summary>
    /// <typeparam name="T">对象泛型</typeparam>
    /// <param name="str">字符流</param>
    /// <returns></returns>
    public static T StrToObject<T>(string str)
    {
        T Obj = JsonConvert.DeserializeObject<T>(str);
        return Obj;
    }
    /// <summary>
    /// 对象转Json文件
    /// </summary>
    /// <param name="obj">对象类型</param>
    /// <returns></returns>
    public static string ObjectToJson(object obj)
    {
        string str = JsonConvert.SerializeObject(obj);
        return str;
    }
    /// <summary>
    /// 对象转Json文件并保存
    /// </summary>
    /// <param name="obj">对象类型</param>
    /// <param name="path">文件路径</param>
    public static void ObjectToJson(object obj,string path)
    {
        if (!File.Exists(path))
        {
            File.CreateText(path);
        }
        string str = JsonConvert.SerializeObject(obj);
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(str);
        }
    }
}

