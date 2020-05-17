using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Reflection;
using SocketMultplayerGameServer.Servers;

namespace SocketMultplayerGameServer.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controDict = new Dictionary<RequestCode, BaseController>();

        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            BaseController userController = new UserController();
            controDict.Add(userController.GetRequestCode,userController);

            BaseController roomController = new RoomController();
            controDict.Add(roomController.GetRequestCode, roomController);

            BaseController gameController = new GameController();
            controDict.Add(gameController.GetRequestCode, gameController);

            BaseController lobblyController = new LobbyController();
            controDict.Add(lobblyController.GetRequestCode, lobblyController);


        }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="pack"></param>
        public void HandRequest(Mainpack pack,Client client)
        {

            if(controDict.TryGetValue(pack.Requestcode,out BaseController controller))
            {
                string metname = pack.Actioncode.ToString();
                MethodInfo method = controller.GetType().GetMethod(metname);
                if (method == null)
                {
                    Helper.Log("没有找到对应的" + metname + "方法");
                }
                object[] obj = new object[] { server, client,pack};
                object ret = method.Invoke(controller, obj);
                if(ret!=null)
                {
                    client.Send(ret as Mainpack);
                    Helper.Log("发送消息给客户端");
                }

            }
            else
            {
                Helper.Log("没有找到对应的controller处理");
            }
        }

    }
}
