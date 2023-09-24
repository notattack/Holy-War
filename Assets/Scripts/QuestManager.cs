using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager 
{
    //单例
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new QuestManager();
            }
            return instance;
        }
    }

    //任务列表
    private List<QuestData> QuestList = new List<QuestData>();

    //是否接收了任务
    public bool HasQuest(int id)
    {
        //遍历已有的任务
        foreach (QuestData qd in QuestList)
        {
            if(qd.id == id)
            {
                return true;
            }
        }
        return false;
    }

    //添加任务
    public void AddQuest(int id)
    {
        //如果没有接受该任务
        if (!HasQuest(id))
        {
            QuestList.Add(QuestDataManager.Instance.QuestDic[id]);
        }
    }

    //击杀了敌人
    public void AddEnemy(int enemyid)
    {
        //遍历任务
        for(int i = 0; i <QuestList.Count; i++)
        {
            QuestData qd = QuestList[i];
            //遍历任务中是否有该敌人需求
            if(qd.enemyId == enemyid)
            {
                //增加敌人任务完成数量
                qd.currentCount++;
                //如果数量满足需求
                if(qd.currentCount >= qd.count)
                {
                    //任务完成,任务奖励，光效等等
                    Debug.Log("任务完成");
                    //删除任务
                    qd.currentCount = 0;
                    QuestList.Remove(qd);
                    //读取一个光效
                    GameObject go = Resources.Load<GameObject>("fx_hr_arthur_pskill_03_2");
                    //获取玩家
                    Transform player = GameObject.FindWithTag("Player").transform;
                    //创建光效
                    GameObject.Instantiate(go, player.position, player.rotation);
                }
            }
        }
    }

}
