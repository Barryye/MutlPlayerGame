using GameFrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameFrameWork.UI
{
    public class UIManager : SingletonMonoObj<UIManager>
    {
        public Transform m_canvasWorld;
        public Transform m_canvasScreen;
        public Transform m_canvasTop;
        public Transform m_WorldInCamera;

        public struct CanvasConfig
        {
            public string CanvasPath;
            private Transform canvasTransform;
            public Transform CanvasTransform
            {
                get
                {
                    if (canvasTransform == null)
                    {
                        canvasTransform = GameObject.Find(CanvasPath).transform;
                    }
                    return canvasTransform;
                }
            }
        }

        public enum CanvasType
        {
            World,
            Screen,
            WorldInCamera,
            Top
        }

        //借助BasePanel脚本保存所有实例化出来的面板物体（因为BasePanel脚本被所有面板预设物体的自己的脚本所继承，所以需要的时候可以根据BasePanel脚本来实例化每一个面板对象）
        public Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();
        private Stack<BasePanel> panelStack = new Stack<BasePanel>();//这是一个栈，用来保存实例化出来（显示出来）的面板
        private Dictionary<string, BasePanel> m_dicNotInStack = new Dictionary<string, BasePanel>();

        private Dictionary<String, ObjectPool> PoolPanel = new Dictionary<String, ObjectPool>();


        /// <summary>
        /// 从resource中加载UI
        /// </summary>
        /// <param name="panelType"></param>
        /// <param name="isHideLast"></param>
        /// <param name="isAni"></param>
        /// <param name="isLastAcceptMsg"></param>
        /// <param name="uiStayCanvas"></param>
        /// <returns></returns>
        public BasePanel PushPanelFromRes(string panelName, CanvasType uiStayCanvas = CanvasType.Screen, bool isHideLast = true, bool isLastAcceptMsg = false, bool isAni = false)
        {
            if (panelStack == null)//如果栈不存在，就实例化一个空栈
            {
                panelStack = new Stack<BasePanel>();
            }

            if (panelStack.Count > 0)
            {
                //BasePanel topPanel = panelStack.Peek();//取出栈顶元素保存起来，但是不移除
                //if (topPanel != null)
                //{
                //    topPanel.OnPause(isHideLast, isLastAcceptMsg);//使该页面暂停，不可交互
                //}
                if (isHideLast == true)
                {
                    BasePanel topPanel = panelStack.Pop();
                    if (topPanel != null)
                    {
                        topPanel.OnPause(isHideLast, isLastAcceptMsg);
                    }
                }
            }
            BasePanel panelTemp = GetPanelFromRes(panelName, uiStayCanvas);
            panelTemp.transform.SetAsLastSibling();
            panelStack.Push(panelTemp);
            panelTemp.OnEnter(isAni);//页面进入显示，可交互
            return panelTemp;
        }

        //判断我当前是否是顶部ui
        public bool IsTopPanel(BasePanel panel)
        {
            BasePanel topPanel = panelStack.Peek();
            if (panel == topPanel)
            {
                return true;
            }
            return false;
        }


        public bool IsTopPanelByName(string type)
        {
            string top = GetTopPanelType();
            if (top == type)
            {
                return true;
            }
            return false;
        }
        //当前顶部ui类型
        public string GetTopPanelType()
        {
            //string ret = "";

            if (panelStack.Count >= 1)
            {
                BasePanel topPanel = panelStack.Peek();
                return topPanel.m_typeName;
            }
            return "";
        }

        //是否曾经创建过该ui
        public bool IsExist(string panelName)
        {
            BasePanel panel = null;
            panelDict.TryGetValue(panelName, out panel);
            if (panel == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 当前页面退出，并显示上一个界面
        /// </summary>
        public void PopPanelShowPeek()
        {
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0) return;
            //关闭栈顶页面的显示
            BasePanel topPanel1 = panelStack.Pop();
            topPanel1.OnExit();

            if (panelStack.Count <= 0) return;
            BasePanel topPanel2 = panelStack.Peek();
            topPanel2.OnResume();//使第二个栈里的页面显示出来，并且可交互
        }

        /// <summary>
        /// 当前页面退出
        /// </summary>
        /// <param name="isEnableLast"></param>
        public void PopSelf(bool isEnableLast = false)
        {         
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0) return;
            //关闭栈顶页面的显示
            BasePanel topPanel1 = panelStack.Pop();
            topPanel1.OnExit();
            if (isEnableLast == true)
            {
                if (panelStack.Count <= 0) return;
                BasePanel topPanel2 = panelStack.Peek();
                topPanel2.OnEnter();//使第二个栈里的页面显示出来，并且可交互
            }
        }

        public void PopAll()
        {
            while (panelStack.Count > 0)
            {
                BasePanel topPanel1 = panelStack.Pop();
                topPanel1.OnExit();
            }
        }
        // 所有显示的页面隐藏，所有页面出栈
        public void PopAllOld()
        {
            if (panelDict != null)
            {
                foreach (var kvp in panelDict)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.OnExit();
                    }
                }
            }


            if (panelStack != null)
            {
                panelStack.Clear();
            }

        }

        // 所有显示的页面摧毁，所有页面出栈
        public void DestroyAll()
        {
            if (panelDict != null)
            {
                foreach (var kvp in panelDict)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.DestroySelf();
                    }
                }

                panelDict.Clear();
            }

            if (panelStack != null)
            {
                panelStack.Clear();
            }

            if (m_dicNotInStack != null)
            {
                foreach (var item in m_dicNotInStack)
                {
                    if (item.Value != null)
                    {
                        item.Value.DestroySelf();
                    }
                }
                m_dicNotInStack.Clear();
            }
        }

        public BasePanel PushPoolPanel(string panelType, CanvasType uiStayCanvas = CanvasType.Screen)
        {
            BasePanel panel;
            ObjectPool pool;
            PoolPanel.TryGetValue(panelType, out pool);
            if (pool==null)
            {
                Transform par = null;
                GameObject obj = Resources.Load<GameObject>(DataMgr.m_projectName + "/UI/" + panelType);
                if (uiStayCanvas == CanvasType.World)
                {
                    par = m_canvasWorld;
                }
                else if (uiStayCanvas == CanvasType.Screen)
                {
                    par = m_canvasScreen;
                }
                else if (uiStayCanvas == CanvasType.Top)
                {
                    par = m_canvasTop;
                }
                else if (uiStayCanvas == CanvasType.WorldInCamera)
                {
                    par = m_WorldInCamera;

                }
                pool = new ObjectPool(obj,par,1);
                PoolPanel.Add(panelType,pool);
            }
            GameObject go= pool.GetObject();
            panel= go.GetComponent<BasePanel>();
            panel.OnEnter();
            return panel;
        }

        public void PopPoolPanel(GameObject Obj)
        {
            ObjectPool pool;
            PoolPanel.TryGetValue(Obj.name, out pool);
            if (pool==null)
            {
                Debug.LogError("没有该物体");
            }
            else
            {
                pool.Recycle(Obj);
            }
        }

        public Transform GetParent(CanvasType uiStayCanvas)
        {
            Transform tempParent = null;
            if (uiStayCanvas == CanvasType.World)
            {
                tempParent = m_canvasWorld;
            }
            else if (uiStayCanvas == CanvasType.Screen)
            {
                tempParent = m_canvasScreen;
            }
            else if (uiStayCanvas == CanvasType.Top)
            {
                tempParent = m_canvasTop;
            }
            return tempParent;
        }

        /// <summary>
        /// 加载面板显示在屏幕，但不存在栈中
        /// </summary>
        /// <param name="panelType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public BasePanel LoadPanel(string panelType, CanvasType type = CanvasType.Screen)
        {
            BasePanel tempPanel = GetPanel(DataMgr.m_projectName + "/UI/" + panelType, type);
            tempPanel.OnEnter();
            m_dicNotInStack[panelType] = tempPanel;
            return tempPanel;
        }

        /// <summary>
        /// 加载resources文件夹下的预设
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public GameObject GetLoadObject(string path)
        {
            return (Resources.Load(path) as GameObject);
        }


        /// <summary>
        /// 获取场景中存在的面板
        /// </summary>
        /// <param name="panelType"></param>
        /// <param name="uiStayCanvas"></param>
        /// <returns></returns>

        private BasePanel GetPanelFromRes(string panelType, CanvasType uiStayCanvas)
        {
            BasePanel panel = null;
            panelDict.TryGetValue(panelType, out panel);//不为空就根据类型得到Basepanel
            if (panel == null)//如果得到的panel为空，那就去panelPathDict字典里面根据路径path找到，然后加载，接着实例化
            {
                Transform par = null;
                if (uiStayCanvas == CanvasType.World)
                {
                    par = m_canvasWorld;
                }
                else if (uiStayCanvas == CanvasType.Screen)
                {
                    par = m_canvasScreen;
                }
                else if (uiStayCanvas == CanvasType.Top)
                {
                    par = m_canvasTop;
                }
                else if (uiStayCanvas == CanvasType.WorldInCamera)
                {
                    par = m_WorldInCamera;
                    
                }
                //GameObject obj = AssetMgr.Instance.CreateObjSync(panelType, panelType, Vector3.zero, Vector3.zero, Vector3.one, par);


                GameObject obj = PublicFunc.CreateObjFromRes(DataMgr.m_projectName + "/UI/" + panelType, par);
                //obj.transform.localScale = Vector3.one;


                panel = obj.GetComponent<BasePanel>();
                panel.m_typeName = panelType;
                if (panelDict.ContainsKey(panelType))
                {
                    panelDict[panelType] = panel;
                }
                else
                {

                    panelDict.Add(panelType, panel);
                }

            }
            return panel;
        }

        //根据面板类型UIPanelType得到实例化的面板
        private BasePanel GetPanel(string panelType, CanvasType uiStayCanvas)
        {
            BasePanel panel = null;
            panelDict.TryGetValue(panelType, out panel);//不为空就根据类型得到Basepanel
            if (panel == null)//如果得到的panel为空，那就去panelPathDict字典里面根据路径path找到，然后加载，接着实例化
            {
                Transform par = null;
                if (uiStayCanvas == CanvasType.Screen)
                {
                    par = m_canvasWorld;
                }
                else if (uiStayCanvas == CanvasType.Screen)
                {
                    par = m_canvasScreen;
                }
                else if (uiStayCanvas == CanvasType.Top)
                {
                    par = m_canvasTop;
                }
                //GameObject obj = AssetMgr.Instance.CreateObjSync(panelType, panelType, Vector3.zero, Vector3.zero, Vector3.one, par);
                //GameObject obj = AssetMgr.Instance.CreateObjSync(panelType, panelType, par);
                //obj.transform.localScale = Vector3.one;

                GameObject obj = PublicFunc.CreateObjFromRes(panelType, par);
                panel = obj.GetComponent<BasePanel>();
                panel.m_typeName = panelType;
                if (panelDict.ContainsKey(panelType))
                {
                    panelDict[panelType] = panel;
                }
                else
                {
                    panelDict.Add(panelType, panel);
                }

            }
            return panel;
        }

        public BasePanel GetPanelFromCache(string panelType)
        {
            if (panelDict == null)
            {
                return null;
            }
            BasePanel panel = null;
            panelDict.TryGetValue(panelType, out panel);//不为空就根据类型得到Basepanel
            return panel;
        }

        public bool IsExistAnyPanel()
        {
            if (panelStack.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}