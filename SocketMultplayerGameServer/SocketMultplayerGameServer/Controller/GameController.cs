using SocketGameProtocol;
using SocketMultplayerGameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMultplayerGameServer.Controller
{
    class GameController:BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        public Mainpack TranslateMessage(Server server,Client client, Mainpack pack)
        {
            pack.Actioncode = ActionCode.TranslateMessage;
            server.TranslateMSG(server.AllClient,pack,client);
            return pack;
        }
        
        /// <summary>
        /// 退出游戏 
        /// </summary>
        /// <param name="sever"></param>
        /// <param name="client"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        public Mainpack ExitGame(Server server, Client client, Mainpack pack)
        {
            pack.Actioncode = ActionCode.ExitGame;
            client.GetRoom.TranslateMSG(server, client, pack);
            return pack;
        }

        /// <summary>
        /// 退出客户端
        /// </summary>
        /// <param name="sever"></param>
        /// <param name="client"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        public Mainpack Close(Server server, Client client, Mainpack pack)
        {
            return pack;
        }

        public void HeartBeat(Server server, Client client, Mainpack pack)
        {
            Helper.Log("心跳收包");
        }
    }
}
