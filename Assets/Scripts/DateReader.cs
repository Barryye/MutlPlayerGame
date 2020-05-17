using Newtonsoft.Json;
using System.IO;

public class DateReader 
{
    /// <summary>
    /// Jsonת����
    /// </summary>
    /// <typeparam name="T">������</typeparam>
    /// <param name="path">Json�ļ�Ŀ¼</param>
    /// <returns></returns>
    public static T JsonToObject<T>(string path)
    {
        string jsonStr = File.ReadAllText(path);
        T Obj = JsonConvert.DeserializeObject<T>(jsonStr);
        return Obj;
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
        StreamWriter sw = File.AppendText(path);
        sw.Close();
    }
}

