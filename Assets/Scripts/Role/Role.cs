/*
 * FileName:     Role
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-14-09:22:48
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class Role : MonoBehaviour
{

    public string playerName;
    public string playerID;
    public int hp;
    private void Start()
    {
        
    }
    public  Mainpack Move(Vector3 forward, float moveSpeed)
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

        Mainpack pack = new Mainpack();
        PlayerPack playerPack = new PlayerPack();
        PosPack pos = new PosPack();
        pos.PosX = transform.position.x;
        pos.PosY = transform.position.y;

        playerPack.Pospack = pos;

        pack.Playerpack.Add(playerPack);
        return pack;
    }

    public Mainpack Rotate(float RoteSpeed)
    {
        transform.Rotate(Vector3.up * Time.deltaTime * RoteSpeed);
        Mainpack pack = new Mainpack();
        return pack;
    }

    public Mainpack Fire()
    {
        Mainpack pack = new Mainpack();
        return pack;
    }

    public Mainpack Chat()
    {
        Mainpack pack = new Mainpack();
        return pack;
    }
}