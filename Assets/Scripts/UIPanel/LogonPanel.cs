/*
 * FileName:     LogonPanel
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-12-19:19:52
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using GameFrameWork.UI;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;
 
/// <summary>
/// 注册
/// </summary>
public class LogonPanel : UGUIPanel 
{
    private InputField user, passWord, playerName;
    private Button logonBtn,returnBtn;
    //网络请求
    private UserRequest userRequest;
    private void Start()
    {
        userRequest = GameObject.FindObjectOfType<UserRequest>();
        user = transform.GetComponent<InputField>("UserName");
        passWord = transform.GetComponent<InputField>("PassWord");
        playerName = transform.GetComponent<InputField>("playerName");

        transform.GetComponent<Button>("LogonBtn").onClick.AddListener(OnLogonClick);
        transform.GetComponent<Button>("ReturnBtn").onClick.AddListener(()=>{
            UIManager.Instance.PushPanelFromRes(UIPanelName.LoginPanel);
        });
    }
    private void OnLogonClick()
    {
        if(user.text==""|| passWord.text==null || playerName.text == null)
        {
            Debug.LogWarning("用户名跟密码不能为空");
            return;
        }
        userRequest.Logon(user.text,passWord.text, playerName.text);
    }
}