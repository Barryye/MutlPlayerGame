using GameFrameWork.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PublicFunc : MonoBehaviour 
{
    static PublicFunc m_instance = null;
    static GameObject m_publicObj = null;
    static Coroutine m_corCreateFromRes = null;//异步创建obj

    public static PublicFunc Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_publicObj = new GameObject();
                m_publicObj.name = "PublicFunc";
                m_instance = m_publicObj.AddComponent<PublicFunc>();
            }
            return m_instance;
        }
    }

    static public void SaveJsonString(string JsonString, string path)    //保存Json格式字符串
    {//写入Json数据
        if (File.Exists(path) == true)
        {
            File.Delete(path);
        }

        string onlyPath = GetOnlyPath(path);
        if (!Directory.Exists(onlyPath))
        {
            Directory.CreateDirectory(onlyPath);
        }

        FileInfo file = new FileInfo(path);
        StreamWriter writer = file.CreateText();
        writer.Write(JsonString);
        writer.Close();
        writer.Dispose();
    }

    static public string GetJsonString(string path)     //从文件里面读取json数据
    {//读取Json数据
        StreamReader reader = new StreamReader(path);
        string jsonData = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        return jsonData;
    }

    public static T GetJsonData<T>(string path) where T : new()
    {
        T ret = new T();
        string str = GetJsonString(GetStreamingAssetsPath() + "/" + path);
        //ret = JsonMapper.ToObject<T>(str);
        return ret;
        
    }

    public static T StringToEnum<T>(string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }

    static public GameObject CreateTmp(GameObject tmp)
    {
        GameObject obj = Instantiate(tmp);
        obj.SetActive(true);
        return obj;
    }

    static public GameObject CreateObjFromRes(string sName, Transform par = null)
    {
        GameObject tmp = Resources.Load<GameObject>(sName);
        if (tmp != null)
        {
            GameObject obj = Instantiate(tmp);
            //obj.SetActive(true);
            if (par != null)
            {
                obj.transform.SetParent(par, false);
                obj.name = tmp.name;
            }
            return obj;
        }
        else
        {
            UnityEngine.Debug.Log("CreateFromResFailure:" + sName);
            return null;
        }
    }
    //是否复位
    static public GameObject CreateObjFromResAndRest(string sName, Transform par = null,bool IsRestPosAndRos=true)
    {
        GameObject tmp = Resources.Load<GameObject>(sName);
        if (tmp != null)
        {
            GameObject obj = Instantiate(tmp);
            obj.SetActive(true);
            if (par != null)
            {
                obj.transform.SetParent(par, false);
                if (IsRestPosAndRos)
                {
                  
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localRotation = Quaternion.identity;
                    obj.transform.localScale = Vector3.one;
                }
            }
            return obj;
        }
        else
        {
            UnityEngine.Debug.Log("CreateFromResFailure:" + sName);
            return null;
        }
    }
    static public GameObject CreateObjFromRes(string sName, GameObject par, Vector3 pos, Vector3 angles, Vector3 scale)
    {
        GameObject tmp = Resources.Load<GameObject>(sName);
        if (tmp != null)
        {
            GameObject obj = Instantiate(tmp);
            obj.SetActive(true);
            obj.transform.parent = par.transform;
            obj.transform.localPosition = pos;
            obj.transform.localEulerAngles = angles;
            obj.transform.localScale = scale;
            return obj;
        }
        else
        {
            UnityEngine.Debug.LogError("CreateFromResFailure:" + sName);
            return null;
        }
    }


    public static void DeleObj(GameObject obj)
    {
        if (obj != null)
        {
            Destroy(obj);
            obj = null;
        }
    }


    /// <summary>
    /// 开启协程
    /// </summary>
    /// <param name="sName"></param>
    /// <param name="par"></param>
    /// <param name="pos"></param>
    /// <param name="angle"></param>
    /// <param name="scale"></param>
    /// <param name="isShowLoad"></param>
    /// <param name="isAutoDestory"></param>
    /// <param name="name"></param>
    /// <param name="isAtuoHide"></param>
    /// <param name="isPreCnt"></param>
    static public void CreateObjFromResAsync(string sName, GameObject par, Vector3 pos, Vector3 angle, Vector3 scale, bool isShowLoad = false, bool isAutoDestory = false, string name = "", bool isAtuoHide = false, bool isPreCnt = false)
    {
        //if (m_corCreateFromRes != null)
        //{
        //    Instance.StopCoroutine(m_corCreateFromRes);
        //}
        m_corCreateFromRes = Instance.StartCoroutine(Instance.YieldCreateFromRes(sName, par, pos, angle, scale, isShowLoad, isAutoDestory, name, isAtuoHide, isPreCnt));
    }

    IEnumerator YieldCreateFromRes(string sName, GameObject par, Vector3 pos, Vector3 angle, Vector3 scale, bool isShowLoad, bool isAutoDestory = false, string name = "", bool isAutoHide = false, bool isPreCnt = false)
    {
        //ResourceRequest resourceRequest = Resources.LoadAsync<Texture2D>("Characters/Textures/CostumePartyCharacters" + (i < 2 ? "" : "" + i));
        //while (!resourceRequest.isDone)
        //{
        //    yield return 0;
        //}
        //material.mainTexture = resourceRequest.asset as Texture2D;
        GameObject load = null;
        if (isShowLoad == true)
        {
            load = PublicFunc.CreateObjFromRes("Lockie/PreLoadMgr");
        }
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(sName);
        while (!resourceRequest.isDone)
        {
            yield return 0;
        }


        GameObject tmp = resourceRequest.asset as GameObject;
        //GameObject tmp = (GameObject)Resources.LoadAsync<GameObject>(sName);
        if (tmp != null)
        {
            GameObject obj = Instantiate(tmp);
            obj.SetActive(true);
            if (obj != null && par != null)
            {
                obj.transform.parent = par.transform;
                obj.transform.localPosition = pos;
                obj.transform.localEulerAngles = angle;
                obj.transform.localScale = scale;
            }

            if (name != "")
            {
                obj.name = name;
            }

            if (isAutoHide == true)
            {
                obj.SetActive(false);
            }

            if (isPreCnt == true)
            {
                // NotificationCenter.Get().ObjDispatchEvent(KEventKey.m_evPreLoadCnt);
            }
            if (isAutoDestory == true)
            {
                Destroy(obj);
            }
            //var rotation = Quaternion.identity;
            //rotation.eulerAngles = angle;
            //GameObject obj = ObjPoolMgr.SpawnPrefab(tmp, pos, rotation, scale, par.transform, transform);
        }
        else
        {
            UnityEngine.Debug.LogError("CreateFromResFailure:" + sName);
        }

        if (isShowLoad == true)
        {
            if (load != null)
            {
                Destroy(load);
            }
        }
        yield return null;
    }


    public static string GetOnlyPath(string path)
    {
        string[] bufPath = path.Split('/');
        string name = bufPath[bufPath.Length - 1];
        string onlyPath = path.Replace(name, "");
        //string abPath = info.m_prefabName.Replace("/" + abName, "");
        //string[] bufAbName = abName.Split('.');
        return onlyPath;
    }


    public static string GetStreamingAssetsPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "jar:file://" + Application.dataPath + "!/assets/";
            case RuntimePlatform.IPhonePlayer:
                return Application.dataPath + "/Raw/";
            default:
                return Application.streamingAssetsPath;
        }
    }
   

    static public void MyStartCoroutine(IEnumerator act)
    {
        Instance.StartCoroutine(act);
    }

   
    public static List<float> GetPos2Net(Vector3 pos)
    {
        List<float> list = new List<float>(new float[] { pos.x, pos.y, pos.z });
        return list;
    }

    public static List<float> GetRot2Net(Quaternion rot)
    {
        List<float> list = new List<float>(new float[] { rot.x, rot.y, rot.z, rot.w });
        return list;
    }

    public static Vector3 GetNet2Pos(List<float> list)
    {
        Vector3 pos = new Vector3(list[0], list[1], list[2]);
        return pos;
    }

    public static Quaternion GetNet2Rot(List<float> list)
    {
        Quaternion pos = new Quaternion(list[0], list[1], list[2], list[3]);
        return pos;
    }

    public static void LoadScene(string sceneName, Action act = null)
    {
        UIManager.Instance.PopAll();
        UIManager.Instance = null;
        
        //SceneManager.LoadScene(sceneName);
        //Loading.LoadingScene(sceneName, act);

    }

}


