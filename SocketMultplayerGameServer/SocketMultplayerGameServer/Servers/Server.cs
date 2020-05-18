using SocketMultplayerGameServer.Controller;
using System;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using System.Net.Sockets;
using SocketGameProtocol;
using System.Threading;

namespace SocketMultplayerGameServer.Servers
{
    class Server
    {
        private Socket socket;
        private System.Timers.Timer timer;
        private float heartBeat = 30;

        private UDPServer us;
        private Thread aucThread;
        //所有客户端
        private List<Client> clientList = new List<Client>();
        public List<Client> AllClient
        {
            get { return clientList; }
        }

        private ControllerManager controllerManager;
        public Server(int port)
        {
            controllerManager = new ControllerManager(this);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any,port));
            socket.Listen(0);

            us = new UDPServer(6667, this, controllerManager);
        }

        ~Server()
        {
            if (aucThread!=null)
            {
                aucThread.Abort();
            }
        }

        #region 心跳
        private void HeartTimerStartUp()
        {
            timer = new System.Timers.Timer(1000);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += HeartBeat;

        }
        private void HeartBeat(object sender,ElapsedEventArgs e)
        {
            long nowTime =GetTimeSpan();
            Client client;
            for (int i = 0; i < clientList.Count; i++)
            {
                client = clientList[i];
                if (nowTime-client.lastHeartTime>heartBeat)
                {
                    if (client.alive)
                    {
                        Helper.Log("心跳发包");
                        Mainpack pack = new Mainpack();
                        pack.Requestcode = RequestCode.Game;
                        pack.Actioncode = ActionCode.HeartBeat;
                        client.Send(pack);
                        client.alive = false;
                        client.lastHeartTime = nowTime;
                    }
                    else
                    {
                        Helper.Log("心跳引起关闭");
                        client.Close();
                    }

                }
               
            }
        }
        private void UpdateHeart(Client client)
        {
            client.lastHeartTime =GetTimeSpan();
            client.alive = true;
        }
        public long GetTimeSpan()
        {
            TimeSpan time = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return Convert.ToInt64(time.TotalSeconds);
        }
        #endregion

        #region 循环监听连接客户端
        private void StartAccept()
        {
            socket.BeginAccept(AcceptCallBack,null);
        }
        private void AcceptCallBack(IAsyncResult iar)
        {
            Socket client = socket.EndAccept(iar);
            if (client!=null)
            {
                clientList.Add(new Client(client, this));
                Helper.Log("有一个客户端加入");
                Helper.Log("当前客户端数量:" + clientList.Count);
            }
           
            StartAccept();
        }
        #endregion

        /// <summary>
        /// 获取接收UDP消息是哪个客户端
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Client ClientFromUserName(string user)
        {
            foreach (Client item in clientList)
            {
                if (item.UserName==user)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 服务器启动
        /// </summary>
        public void Start()
        {
            StartAccept();
            //心跳启动
            HeartTimerStartUp();
        }
        public void HandleRequest(Mainpack pack,Client client)
        {
            //更新客户端心跳时间
            UpdateHeart(client);
            controllerManager.HandRequest(pack, client);
        }
        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        public void RemoveClient(Client client)
        {
            clientList.Remove(client);
        }
        /// <summary>
        /// TCP转发消息
        /// </summary>
        /// <param name="clientLst">客户端列表</param>
        /// <param name="pack">消息包</param>
        /// <param name="client">当前客户端</param>
        /// <param name="Unself">是否不给当前客户端转发</param>
        public void TranslateMSG(List<Client> clientLst,Mainpack pack,Client client,bool Unself =true)
        {
            foreach (var item in clientLst)
            {
                if (!Unself)
                {
                    item.Send(pack);
                }
                else
                {
                    if (item!=client)
                    {
                        item.Send(pack);
                    }
                }
                
            }
        }


    }
}
