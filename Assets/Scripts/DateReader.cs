using Newtonsoft.Json;
using System.IO;

public class DateReader 
{
    /// <summary>
    /// Json转对象
    /// </summary>
    /// <typeparam name="T">对象泛型</typeparam>
    /// <param name="path">Json文件目录</param>
    /// <returns></returns>
    public static T JsonToObject<T>(string path)
    {
        string jsonStr = File.ReadAllText(path);
        T Obj = JsonConvert.DeserializeObject<T>(jsonStr);
        return Obj;
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
        StreamWriter sw = File.AppendText(path);
        sw.Close();
    }
}

