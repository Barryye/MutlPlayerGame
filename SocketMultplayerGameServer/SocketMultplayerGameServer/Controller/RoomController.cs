using SocketGameProtocol;
using SocketMultplayerGameServer.Servers;
using System;
using System.Collections.Generic;


namespace SocketMultplayerGameServer.Controller
{
    class RoomController:BaseController
    {

        //所有的房间里
        private List<Room> roomList = new List<Room>();
        public RoomController()
        {
            requestCode = RequestCode.Room;
            Client.act += RemoveRoom;
        }

        public void RemoveRoom(Room room)
        {
            roomList.Remove(room);
        }


        #region 房间操作
        public Mainpack FindRoom(Server server, Client client, Mainpack pack)
        {
            pack.Actioncode = ActionCode.FindRoom;
            try
            {
                if (roomList.Count == 0)
                {
                    pack.Returncode = ReturnCode.NotRoom;
                    return pack;
                }
                foreach (var item in roomList)
                {
                    pack.Actioncode = ActionCode.FindRoom;
                    pack.Roompack.Add(item.GetRoomInfo);
                }
                pack.Returncode = ReturnCode.Succeed;
                return pack;
            }
            catch (Exception)
            {

                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
        public Mainpack JoinRoom(Server server, Client client, Mainpack pack)
        {
            //pack.Actioncode = ActionCode.JoinRoom;
            Room room = roomList.Find((go) => { return go.GetRoomInfo.Roomname == pack.Roompack[0].Roomname; });
            if (room.GetRoomInfo.Maxnum < room.GetRoomInfo.Curnum)
            {
                room.Join(server, client);
                pack.Returncode = ReturnCode.Succeed;

                PlayerPack player = new PlayerPack();
                player.Playername = client.UserName;
                player.PlayerID = room.GetRoomInfo.Curnum.ToString();
                pack.Playerpack.Add(player);
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        public Mainpack CreateRoom(Server server, Client client, Mainpack pack)
        {
            //pack.Actioncode = ActionCode.CreateRoom;
            Room room = roomList.Find((go) => { return go.GetRoomInfo == pack.Roompack[0]; });
            if (room != null)
            {
                pack.Returncode = ReturnCode.Fail;
            }
            else
            {
                room = new Room(client,pack.Roompack[0]);
                roomList.Add(room);
                client.GetRoom = room;
                pack.Returncode = ReturnCode.Succeed;

                PlayerPack player = new PlayerPack();
                player.Playername = client.UserName;
                player.PlayerID = "1";

                pack.Playerpack.Add(player);
            }
            return pack;
        }

        public Mainpack Chat(Server server, Client client, Mainpack pack)
        {
            Room room = roomList.Find((go) => { return go.GetRoomInfo.Roomname == pack.Roompack[0].Roomname; });
            room.TranslateMSG(server, client, pack);
            pack.Returncode = ReturnCode.Succeed;
            return pack;
        }
        public Mainpack Exit(Server server,Client client, Mainpack pack)
        {
            //pack.Actioncode = ActionCode.Exit;
            Room room = roomList.Find((go) => { return go.GetRoomInfo == pack.Roompack[0]; });
            if (room != null)
            {
                room.Exit(this,server,client,pack);
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        #endregion
    }
}
