using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMultplayerGameServer.Controller
{
    class LogicController:BaseController
    {
        public LogicController()
        {
            requestCode = RequestCode.Logic;
        }
    }
}
