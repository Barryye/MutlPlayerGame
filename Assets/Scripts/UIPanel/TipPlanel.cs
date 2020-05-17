/*
 * FileName:     TipPlanel
 * CompanyName:  医奇科技
 * Author:       叶东健
 * CreateTime:   2020-05-15-11:14:42
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
 
public class TipPlanel : UGUIPanel 
{

    private Text text;
    private Animator anim;
    public void SetContext(string str,Color color)
    {
         text.text = str;
         text.color = color;
    }

    public void SetContext(string str)
    {
        text.text = str;
    }

    public override void OnEnter(bool isAni = false)
    {
        base.OnEnter(isAni);
        text = transform.GetComponent<Text>("Text");
        //anim = transform.GetComponent<Animator>();
        //anim.Play("Tip");
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnExit()
    {
        UIManager.Instance.PopPoolPanel(this.gameObject);
        //ebug.Log("提示退出");
    }

    public static void Open(string str)
    {
        TipPlanel panel= UIManager.Instance.PushPoolPanel(UIPanelName.TipPanel) as TipPlanel;
        panel.SetContext(str);

    }
}