using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

//任务数据类
public class QuestData
{
    //任务id
    public int id;
    //名称
    public string name;
    //敌人id
    public int enemyId;
    //敌人个数
    public int count;
    //完成敌人个数
    public int currentCount;
    //任务金钱
    public int money;
}

public class QuestDataManager
{
    //单例
    private static QuestDataManager instance;
    public static QuestDataManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new QuestDataManager();
            }
            return instance;
        }
    }

    //任务集合
    public Dictionary<int, QuestData> QuestDic = new Dictionary<int, QuestData>();

    private QuestDataManager()
    {
        //解析任务xml
        XmlDocument doc = new XmlDocument();
        //加载xml文件
        doc.Load(Application.dataPath + "/quest.xml");
        //根元素
        XmlElement rootEle = doc.LastChild as XmlElement;
        //遍历任务
        foreach (XmlElement questEle in rootEle)
        {
            //创建一个任务对象
            QuestData qd = new QuestData();
            qd.id = int.Parse(questEle.GetElementsByTagName("id")[0].InnerText);
            qd.name = questEle.GetElementsByTagName("name")[0].InnerText;
            qd.enemyId = int.Parse(questEle.GetElementsByTagName("enemyid")[0].InnerText);
            qd.count = int.Parse(questEle.GetElementsByTagName("count")[0].InnerText);
            XmlElement reward = questEle.GetElementsByTagName("reward")[0] as XmlElement;
            qd.money = int.Parse(reward.GetElementsByTagName("money")[0].InnerText);
            //添加到我们的任务集合
            QuestDic.Add(qd.id, qd);
        }
    }
}
