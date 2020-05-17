/*
 * FileName:     ObjectPool
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-03-03-10:43:00
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:对象池，提供简单的获取和回收功能
 * 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool
{
    private Queue<GameObject> pool;
    private List<GameObject> objects;
    private GameObject prefab;
    private Transform prefabParent;
    //使用构造函数构造对象池  
    public ObjectPool(GameObject obj, Transform parent, int count)
    {
        prefab = obj;
        pool = new Queue<GameObject>(count);
        prefabParent = parent;
        objects = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject objClone = GameObject.Instantiate(prefab) as GameObject;
            //为克隆出来的子弹指定父物体 
            objClone.transform.SetParent(prefabParent, false);
            objClone.name = prefab.name;
            objClone.SetActive(false);
            pool.Enqueue(objClone);
        }
    }


    public GameObject GetObject()
    {
        GameObject obj = null;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();  //Dequeue()方法 移除并返回位于 Queue 开始处的对象 
        }
        else
        {
            obj = GameObject.Instantiate(prefab) as GameObject;
            obj.name = prefab.name;
            obj.transform.SetParent(prefabParent, false);
        }

        obj.SetActive(true);
        objects.Add(obj);
        return obj;
    }

    //回收对象  
    public void Recycle(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);//加入队列  
    }

    public void RecycleAllObj()
    {
        foreach (var item in objects)
        {
            Recycle(item);
        }
    }
}