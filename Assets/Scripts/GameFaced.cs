/*
 * FileName:     GameFaced
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-12-17:19:07
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using GameFrameWork.UI;
using SocketGameProtocol;
using System.Collections;
using UnityEngine;
 
public class GameFaced : MonoBehaviour
{
    private ClientManager clientManager;
    private RequestManager requestManager;
    private RoleManager roleManager;
    private Mainpack pack;
    private PlayerPack role;
    public PlayerPack m_Role
    {
        get { return role; }
        set { role = value; }
    }
    private bool connect;

    private GameRequest gameRequest;
    public GameRequest m_GameRequest
    {
        get { return gameRequest; }
    }
    public string UserName
    {
        get;
        set;
    }

    private static GameFaced instance;
    public static GameFaced Face
    {
        get { return instance; }
    }


    private void Awake()
    {
        instance = this;
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        roleManager = new RoleManager(this);
        
        //clientManager.OnInit();
        requestManager.OnInit();
        DontDestroyOnLoad(this);

        StartCoroutine(ConnectServer());
    }

    private void Start()
    {
        gameRequest = GameObject.FindObjectOfType<GameRequest>();
        DontDestroyOnLoad(gameRequest.gameObject);
        UIManager.Instance.PushPanelFromRes(UIPanelName.LoginPanel);
    }
    private void Update()
    {
        if (pack!=null)
        {
            requestManager.HandleResponse(pack);
            pack = null;
        }

    }

    /// <summary>
    /// 服务器链接
    /// </summary>
    /// <returns></returns>
    private IEnumerator ConnectServer()
    {
        while (true)
        {
            if (!connect)
            {
                clientManager.OnInit();
                connect = clientManager.connect;
            }
            yield return new WaitForSeconds(5);
        }
    }
    private void OnDestroy()
    {
        clientManager.OnDestory();
        requestManager.OnDestory();
    }

    public void Send(Mainpack pack)
    {
        clientManager.Send(pack);
    }

    public void SendUDP(Mainpack pack)
    {
        pack.User = UserName;
        clientManager.SendUDP(pack);
            
    }

    //public void HandleResponse(Mainpack pack)
    //{
    //    //处理
    //    requestManager.HandleResponse(pack);
    //}

    public Mainpack HandleResponse(Mainpack pack)
    {
        //处理
        this.pack = pack;
        return pack;
    }
    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }
    public void RemoveRequest(RequestCode request)
    {
        requestManager.RemoveRequest(request);
    }

    public void AddRole(Mainpack pack)
    {
        foreach (var item in pack.Playerpack)
        {
            string name= item.Playername;
            roleManager.AddRole(name,item);
        };
        
    }

    public void RemoveRole(Mainpack pack)
    {
       string name= pack.Playerpack[0].Playername;
        roleManager.RemoveRole(name);
    }
}