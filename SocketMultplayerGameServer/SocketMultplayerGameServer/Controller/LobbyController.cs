using SocketGameProtocol;
using SocketMultplayerGameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMultplayerGameServer.Controller
{
    class LobbyController:BaseController
    {
        public LobbyController()
        {
            requestCode = RequestCode.Lobby;
        }

        public Mainpack Chat(Server server, Client client, Mainpack pack)
        {

           // pack.Actioncode = ActionCode.TranslateMessage;
            server.TranslateMSG(server.AllClient, pack, client,false);
            return pack;
        }
    }
}
