using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using SocketMultplayerGameServer.Servers;

namespace SocketMultplayerGameServer.Controller
{
    class BaseController
    {
        protected RequestCode requestCode = RequestCode.RequestNone;

        public RequestCode GetRequestCode
        {
            get { return requestCode; }
        }
    }
}
