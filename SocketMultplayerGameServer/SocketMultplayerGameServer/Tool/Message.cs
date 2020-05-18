
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using Google.Protobuf;


namespace SocketMultplayerGameServer.Tool
{
    class Message
    {
        private byte[] buffer = new byte[1024];
        private int startIndex;

        public byte[] Buffer
        {
            get
            {
                return buffer;
            }
        }

        public int StartIndex
        {
            get
            {
                return startIndex;
            }
        }

        public int RemSize
        {
            get
            {
                return buffer.Length - startIndex;
            }
        }
        public void ReadBuffer(int len,Action<Mainpack> HandleRequest)
        {
            startIndex += len;
            if(startIndex<=4)
            {
                return;
            }
            int count = BitConverter.ToInt32(buffer, 0);
            while (true)
            {
                if (startIndex >= (count + 4))
                {
                    Mainpack pack = (Mainpack)Mainpack.Descriptor.Parser.ParseFrom(buffer, 4, count);
                    HandleRequest(pack);
                    Helper.Log("收到包");
                    Array.Copy(buffer, count + 4, buffer, 0, startIndex - count - 4);
                    startIndex -= (count + 4);
                }
                else
                {
                    return;
                }
            }
        }

        public static byte[] PackData(Mainpack pack)
        {
            byte[] data = pack.ToByteArray();//包体
            byte[] head = BitConverter.GetBytes(data.Length);//包头
            return head.Concat(data).ToArray();
        }

        public static byte[] PackDataUDP(Mainpack pack)
        {
            return pack.ToByteArray();
        }
    }
}
