using System;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using SocketGameProtocol;
using SocketMultplayerGameServer.DAO;
using SocketMultplayerGameServer.Tool;


namespace SocketMultplayerGameServer.Servers
{
    class Client
    {
        private string connstr = "database=sys;data source=127.0.0.1;user=root;password=123456;pooling=false;charset=utf8;port=3306";
        private Socket socket;
        private Message message;
        private UserData userData;
        public static Action<Room> act;
        public UserData GetUserData
        {
            get { return userData; }
        }
        private Server server;

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public Room GetRoom
        {
            get;
            set;
        }

        /// <summary>
        /// 最后一次心跳时间
        /// </summary>
        public long lastHeartTime;

        /// <summary>
        /// 是否活着
        /// </summary>
        public bool alive;

        private MySqlConnection sqlConnection;
        public MySqlConnection GetMysqlConnect
        {
            get { return sqlConnection; }
        }
        public Client(Socket socket,Server server)
        {
            this.server = server;
            this.socket = socket;

            userData = new UserData();
            message = new Message();
            lastHeartTime = server.GetTimeSpan();
            alive = true;
            ConnectMySql();

            StartReceive();

        }

        private void StartReceive()
        {
            socket.BeginReceive(message.Buffer,message.StartIndex,message.RemSize,SocketFlags.None, ReceiveCallBack, null);
        }
        private void ReceiveCallBack(IAsyncResult iar)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                int len = socket.EndReceive(iar);
                if (len == 0)
                {
                    return;
                }

                message.ReadBuffer(len, HandleRequest);
                StartReceive();
            }
            catch (Exception)
            {

                Close();
            }
        }

        /// <summary>
        /// 发送消息给客户端
        /// </summary>
        /// <param name="pack"></param>
        public void Send(Mainpack pack)
        {
            socket.Send(Message.PackData(pack));
        }

        /// <summary>
        /// 服务端接收回调
        /// </summary>
        /// <param name="pack"></param>
        private void HandleRequest(Mainpack pack)
        {
            server.HandleRequest(pack, this);
        }

        public void Close()
        {
            if (GetRoom!=null)
            {
                act(GetRoom);
               //GetRoom.ExitHost(server, this);

            }
            server.RemoveClient(this);
            socket.Shutdown(SocketShutdown.Both);
            //socket.Close();
            sqlConnection.Close();
        }

        public void ConnectMySql()
        {
            try
            {
                sqlConnection = new MySqlConnection(connstr);
                sqlConnection.Open();
                Helper.Log("数据库打开成功");
            }
            catch (Exception e)
            {

                Helper.Log("数据库打开失败"+e); 
            }

        }
    }
}
