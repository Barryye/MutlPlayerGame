/*
 * FileName:     LoginPanel
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-13-13:59:23
 * UnityVersion: 2018.4.0f1
 * Version：     1.0
 * Description:
 * 
*/

using GameFrameWork.UI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登錄註冊Panel
/// </summary>
public class LoginPanel : UGUIPanel 
{
    private InputField user, passWord ;
    private Button LogonBtn,LoginBtn;
    //网络请求
    private UserRequest userRequest;

    private void Start()
    {
        userRequest = GameObject.FindObjectOfType<UserRequest>();
    }
    public override void OnOpen()
    {
        user = transform.GetComponent<InputField>("UserName");
        passWord = transform.GetComponent<InputField>("PassWord");
        

        transform.GetComponent<Button>("LoginBtn").onClick.AddListener(OnLoginClick);
        transform.GetComponent<Button>("LogonBtn").onClick.AddListener(()=>{
            UIManager.Instance.PushPanelFromRes(UIPanelName.LogonPanel);
        });
        base.OnOpen();
    }

    private void OnLoginClick()
    {
        if (user.text == "" || user.text == null)
        {
            Debug.LogWarning("用户名跟密码不能为空");
            return;
        }
        userRequest.Login(user.text, passWord.text);
    }
}