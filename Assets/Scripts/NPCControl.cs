using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : MonoBehaviour
{
    //姓名
    public string Name = "村民";
    //对话
    public string Content = "最近村外石头人很多，快去击杀两只吧！";
    //任务ID
    public int QuestID = 1001;
    //玩家
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        //获取npc和玩家距离
        float dis = Vector3.Distance(player.position, transform.position);
        //距离小于4米，按下F键
        if (dis < 4 && Input.GetKeyDown(KeyCode.F))
        {
            //显示对话面板
            UIManager.Instance.Show(Name, Content, QuestID);
        }
    }
}
