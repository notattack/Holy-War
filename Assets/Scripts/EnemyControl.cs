using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //敌人id
    public int ID = 101;
    //玩家
    public PlayerControl player;
    //血量
    public int Hp = 100;
    //攻击力
    public int Attack = 20;
    //出生点
    private Vector3 position;
    //动画器
    private Animator animator;
    //攻击计时器
    private float timer = 1;
    //当前是否在攻击
    private bool isAttack = false;

    void Start()
    {
        //获取玩家脚本
        player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        //获得出生点
        position = transform.position;
        //获取动画器组件
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        //如果玩家死亡，停止动作
        if(player.Hp <= 0 || Hp <= 0)
        {
            animator.SetBool("Run", false);
            animator.SetBool("Attack", false);
            return;
        }
        //获取与玩家的距离
        float distance = Vector3.Distance(player.transform.position, transform.position);
        //如果7米内没发现玩家
        if (distance > 7)
        {
            if(Vector3.Distance(transform.position, position) > 1)
            {
                //转向出生点
                transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
                //向前移动
                transform.Translate(Vector3.forward * 2 * Time.deltaTime);
                //播放移动动画
                animator.SetBool("Run", true);
            } else
            {
                animator.SetBool("Run", false);
            }
        } else if(distance > 3f)
        {
            //朝向玩家移动
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            //移动
            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
            //播放移动动画
            animator.SetBool("Run", true);
            isAttack = false;
            animator.SetBool("Attack", false);
        } else
        {
            //攻击玩家
            //停止移动
            animator.SetBool("Run", false);
            //转向玩家
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            //攻击
            animator.SetBool("Attack", true);
            if(isAttack == false)
            {
                isAttack = true;
                timer = 1;
            }
            //计时器增加
            timer += Time.deltaTime;
            if(timer >= 2)
            {
                timer = 0;
                //打出伤害
                player.GetDamage(Attack);
            }
        }
    }

    //血瓶预设体
    public GameObject PotionPre;

    //受到攻击
    public void GetDamage(int damage)
    {
        if(Hp > 0)
        {
            //弹出伤害
            GetComponentInChildren<HpManager>().ShowText("-" + damage);
            Hp -= damage;
            if(Hp <= 0)
            {
                //掉落血瓶
                Instantiate(PotionPre, transform.position, transform.rotation);
                animator.SetTrigger("Die");
                QuestManager.Instance.AddEnemy(ID);
                //销毁自己
                Destroy(gameObject, 2f);
            }
        }
    }

 
    

}
