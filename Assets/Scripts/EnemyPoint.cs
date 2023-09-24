using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    //关联敌人预设体
    public GameObject EnemyPre;
    //敌人生成的数量
    public int Num = 3;
    //计时器
    private float timer;

    void Update()
    {
        //计时器增加
        timer += Time.deltaTime;
        //2秒检测
        if(timer > 2)
        {
            timer = 0;
            //查看有几个敌人
            int n = transform.childCount;
            if(n < Num)
            {
                //随机位置
                Vector3 v = transform.position;
                v.x += Random.Range(-5, 5f);
                v.z += Random.Range(-5, 5f);
                //随机一个旋转
                Quaternion q = Quaternion.Euler(0, Random.Range(0, 360), 0);
                //创建一个敌人
                GameObject go = GameObject.Instantiate(EnemyPre, v, q);
                //设置父子关系
                go.transform.SetParent(transform);
            }
        }
    }
}
