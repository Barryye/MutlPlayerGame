/*
 * FileName:     PlayerNetWork
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-13-17:59:29
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
 
public class PlayerNetWork : MonoBehaviour 
{
    public GameRequest GameRequest;
    public Role role;
    public float moveSpeed = 10;
    public float rotSpeed = 10;
    private void Start()
    {
        role = GetComponent<Role>();
    }

    private void Update()
    {

    }

    private void Move()
    {
        //role.Move();
    }

    private void Rotate()
    {
        //role.Rotate();
    }

    private void Fire()
    {
        Mainpack pack= role.Fire();
        GameRequest.SendRequest(pack);
    }

   
}