using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using SocketMultplayerGameServer.Servers;

namespace SocketMultplayerGameServer.Controller
{
    class UserController:BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public Mainpack Logon(Server server,Client client,Mainpack pack)
        {
            if (client.GetUserData.Logon(pack,client.GetMysqlConnect))
            {
                client.UserName = pack.Playerpack[0].Playername;
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public Mainpack Login(Server sever, Client client, Mainpack pack)
        {
            if (client.GetUserData.Login(pack,client.GetMysqlConnect))
            {
                PlayerPack player = new PlayerPack();
                player.Playername = "Barry";
                pack.Playerpack.Add(player);
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        public void Close(Server sever, Client client, Mainpack pack)
        {
            Helper.Log("客户端关闭");
            client.Close();
        }

    }
}
