using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketMultplayerGameServer.Servers;

namespace SocketMultplayerGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(6666);
            server.Start();
            Helper.Log("服务器启动");
            Console.Read();
        }
    }
}
