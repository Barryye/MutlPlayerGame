using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class DateReader 
{
    /// <summary>
    /// //���ļ������ȡjson����
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetJsonString(string path)     
    {
        //��ȡJson����
        StreamReader reader = new StreamReader(path);
        string jsonData = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        Debug.Log(jsonData);
        return jsonData;
    }
    /// <summary>
    /// ��ȡJson�ļ���ת����
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
    /// Jsonת����
    /// </summary>
    /// <typeparam name="T">������</typeparam>
    /// <param name="path">Json�ļ�Ŀ¼</param>
    /// <returns></returns>
    public static T JsonToObject<T>(string str)
    {
        var ret = JsonConvert.DeserializeObject<T>(str);
        return ret;
    }
    /// <summary>
    /// �ַ���ת����
    /// </summary>
    /// <typeparam name="T">������</typeparam>
    /// <param name="str">�ַ���</param>
    /// <returns></returns>
    public static T StrToObject<T>(string str)
    {
        T Obj = JsonConvert.DeserializeObject<T>(str);
        return Obj;
    }
    /// <summary>
    /// ����תJson�ļ�
    /// </summary>
    /// <param name="obj">��������</param>
    /// <returns></returns>
    public static string ObjectToJson(object obj)
    {
        string str = JsonConvert.SerializeObject(obj);
        return str;
    }
    /// <summary>
    /// ����תJson�ļ�������
    /// </summary>
    /// <param name="obj">��������</param>
    /// <param name="path">�ļ�·��</param>
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

