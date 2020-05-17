using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;

public class GameRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        //actionCode = ActionCode.TranslateMessage;

        EventManger.AddListener<Mainpack>(ActionCode.HeartBeat, HeartBeat);
        base.Awake();
    }

    public override void OnDestroy()
    {
        EventManger.RemoveListener<Mainpack>(ActionCode.HeartBeat, HeartBeat);
        base.OnDestroy();
    }
    public override void SendRequest(Mainpack pack)
    {
        base.SendRequest(pack);
    }

    public override void OnResponse(Mainpack pack)
    {
        base.OnResponse(pack);
    }

    /// <summary>
    /// ÐÄÌø»Ø°ü
    /// </summary>
    /// <param name="pack"></param>
    private void HeartBeat(Mainpack pack)
    {
        SendRequest(pack);
    }

}
