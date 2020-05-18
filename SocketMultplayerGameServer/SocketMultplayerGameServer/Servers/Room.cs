using Google.Protobuf.Collections;
using SocketGameProtocol;
using SocketMultplayerGameServer.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMultplayerGameServer.Servers
{
    class Room
    {
        private RoomPack roomInfo;
        private List<Client> clientList = new List<Client>();

        public RoomPack GetRoomInfo
        {
            get
            {
                roomInfo.Curnum = clientList.Count;
                return roomInfo;
            }
        }
        public Room(Client client,RoomPack pack)
        {
            this.roomInfo = pack;
            clientList.Add(client);
        }

        public RepeatedField<PlayerPack> GetPlayerInfo()
        {
            RepeatedField<PlayerPack> pack = new RepeatedField<PlayerPack>();
            foreach (var item in clientList)
            {
                PlayerPack player = new PlayerPack();
                player.Playername = item.UserName;
                pack.Add(player);
            }
            Helper.Log("玩家人数：" + pack.Count);
            return pack;
        }

        public void TranslateMSG(Server server,Client client,Mainpack pack)
        {
            server.TranslateMSG(clientList, pack, client);
        }

        public void TranslateUDPMSG(Client client,Mainpack pack)
        {
            foreach (Client item in clientList)
            {
                if (item.Equals(client))
                {
                    continue;
                }
                item.SendUDP(pack);
            }
        }

        public void Join(Server server,Client client)
        {
            clientList.Add(client);
            client.GetRoom = this;
            Mainpack pack = new Mainpack();
            pack.Actioncode = ActionCode.PlayerList;
            foreach (var item in GetPlayerInfo())
            {
                pack.Playerpack.Add(item);
            }
            Helper.Log("广播加入房间");
            TranslateMSG(server,client,pack);
        }

        /// <summary>
        /// 退出是否为房主返回值true退出为房主false客户
        /// </summary>
        /// <param name="server"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public void Exit(RoomController controller, Server server,Client client, Mainpack pack)
        {
            
            if (client==clientList[0])
            {
                Helper.Log("房主离开");
                client.GetRoom = null;
                pack.Actioncode = ActionCode.Exit;
                controller.RemoveRoom(client.GetRoom);
                TranslateMSG(server, client, pack);
                return;
            }
            Helper.Log("客户端离开");
            clientList.Remove(client);
            client.GetRoom = null;
            pack.Actioncode = ActionCode.PlayerList;
            foreach (var item in GetPlayerInfo())
            {
                pack.Playerpack.Add(item);
            }
            TranslateMSG(server, client, pack);
            return;
        }


    }
}
