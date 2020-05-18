using SocketGameProtocol;
using SocketMultplayerGameServer.Controller;
using SocketMultplayerGameServer.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketMultplayerGameServer.Servers
{
    class UDPServer
    {
        Socket udpServer;//udpsocket
        IPEndPoint bindEP;//本地监听ip
        EndPoint remoteEP;//远程ip

        Server server;

        ControllerManager controllerManager;

        Byte[] buffer = new Byte[1024];//消息缓存

        Thread receiveThread;//接收线程

        public UDPServer(int port,Server server,ControllerManager controllerManager)
        {
            this.server = server;
            this.controllerManager = controllerManager;
            udpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            bindEP = new IPEndPoint(IPAddress.Any, port);
            remoteEP = (EndPoint)bindEP;
            udpServer.Bind(bindEP);
            receiveThread = new Thread(ReceiveMsg);
            receiveThread.Start();
            Helper.Log("UDP服务已启动...");
        }

        ~UDPServer()
        {
            if (receiveThread!=null)
            {
                receiveThread.Abort();
                receiveThread = null;
            }
        }
        /// <summary>
        /// UDP接收消息
        /// </summary>
        private void ReceiveMsg()
        {
            while (true)
            {
                int len = udpServer.ReceiveFrom(buffer, ref remoteEP);
                Mainpack pack = (Mainpack)Mainpack.Descriptor.Parser.ParseFrom(buffer, 0, len);
                Handlerequest(pack,remoteEP);
            }
        }

        public void Handlerequest(Mainpack pack,EndPoint ipEndPoint)
        {
            Client client = server.ClientFromUserName(pack.User);
            if (client.IEP==null)
            {
                client.IEP = ipEndPoint;
            }
            controllerManager.HandRequest(pack, client, true);
        }
        /// <summary>
        /// UDP发送消息
        /// </summary>
        /// <param name="pack"></param>
        /// <param name="point"></param>
        public void SendUDP(Mainpack pack,EndPoint point)
        {
            byte[] buff = Message.PackDataUDP(pack);
            udpServer.SendTo(buff, buff.Length, SocketFlags.None, point);
        }
    }
}
