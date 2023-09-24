using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //单例
    public static UIManager Instance;
    //血条
    private Image hpBar;
    //玩家
    private PlayerControl player;
    //对话框
    private Image dialog;
    //对话框显示的任务id
    private int questid;

    void Start()
    {
        Instance = this;
        hpBar = transform.Find("Head").Find("HpBar").GetComponent<Image>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        //获取对话框
        dialog = transform.Find("Dialog").GetComponent<Image>();
        //默认隐藏对话框
        dialog.gameObject.SetActive(false);
    }


    void Update()
    {
        hpBar.fillAmount = (float)player.Hp / player.MaxHp;
    }

    //显示对话框
    public void Show(string name, string content, int id = -1)
    {
        //鼠标显示
        Cursor.lockState = CursorLockMode.None;
        //显示对话框
        dialog.gameObject.SetActive(true);
        //设置标题
        dialog.transform.Find("NameText").GetComponent<Text>().text = name;
        //记录任务id
        questid = id;
        //判断该任务是否接收
        if (QuestManager.Instance.HasQuest(id))
        {
            //设置内容
            dialog.transform.Find("ContentText").GetComponent<Text>().text = "你已经接收该任务了";
        } else
        {
            //设置内容
            dialog.transform.Find("ContentText").GetComponent<Text>().text = content;
        }
    }

    //接收
    public void AcceptButtonClick()
    {
        //隐藏对话框
        dialog.gameObject.SetActive(false);
        //隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
        //接收任务
        QuestManager.Instance.AddQuest(questid);
    }

    //取消
    public void CancelButtonClick()
    {
        //隐藏对话框
        dialog.gameObject.SetActive(false);
        //隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }
}
