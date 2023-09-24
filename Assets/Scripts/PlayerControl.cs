using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色状态
public enum PlayerState
{
    idle,
    run,
    die,
    attack,
    attack2
}

public class PlayerControl : MonoBehaviour
{
    //刚体
    private Rigidbody rBody;
    //角色状态
    private PlayerState state = PlayerState.idle;
    //动画器组件
    private Animator animator;
    //最大血量
    public int MaxHp = 100;
    //当前血量
    public int Hp = 100;

    void Start()
    {
        //获取刚体组件
        rBody = GetComponent<Rigidbody>();
        //隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
        //获取动画器组件
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        //按下Alt键
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            //显示隐藏鼠标
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
        //如果鼠标呼出状态，则不做任何事
        if(Cursor.lockState == CursorLockMode.None)
        {
            return;
        }

        //判断状态
        switch (state)
        {
            case PlayerState.idle:
                //允许旋转
                Rotate();
                //允许切换移动
                Move();
                //允许切换攻击
                Attack();
                //播放站立动画
                animator.SetBool("Run", false);
                break;
            case PlayerState.run:
                //允许旋转
                Rotate();
                //允许移动
                Move();
                //攻击
                Attack();
                //播放跑步动画
                animator.SetBool("Run", true);
                break;
            case PlayerState.die:
                break;
            case PlayerState.attack:
                break;
            case PlayerState.attack2:
                break;
        }
        //要删除的特效
        Transform fx = null;
        //刷新红色火焰特效
        foreach(Transform trans in fxList)
        {
            //特效移动
            trans.Translate(Vector3.forward * 20 * Time.deltaTime);
            //判断周围有没有敌人
            Collider[] colliders = Physics.OverlapSphere(trans.position, 1f);
            //遍历特效
            foreach(Collider collider in colliders)
            {
                //如果附近有敌人
                if(collider.tag == "Enemy")
                {
                    //敌人掉血
                    collider.GetComponent<EnemyControl>().GetDamage(20);
                    fx = trans;
                    //产生爆炸
                    GameObject fxPre = Resources.Load<GameObject>("Explosion");
                    GameObject go = Instantiate(fxPre, collider.transform.position, collider.transform.rotation);
                    Destroy(go, 2f);
                    break;
                }
            }
        }
        //删除掉待删除的特效
        if(fx != null)
        {
            fxList.Remove(fx);
        }
    }


    //旋转
    void Rotate()
    {
        //玩家角色同步摄像机旋转
        transform.rotation = Camera.main.transform.parent.rotation;
    }

    //移动
    void Move()
    {
        //获取水平轴
        float horizontal = Input.GetAxis("Horizontal");
        //获取垂直轴
        float vertical = Input.GetAxis("Vertical");
        //移动向量
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        if (dir != Vector3.zero)
        {
            //纵向移动
            rBody.velocity = transform.forward * vertical * 8;
            //横向移动
            rBody.velocity += transform.right * horizontal * 6;
            //切换成移动状态
            state = PlayerState.run;
        }
        else
        {
            state = PlayerState.idle;
        }
    }

    //攻击
    void Attack()
    {
        //左键攻击
        if (Input.GetMouseButtonDown(0))
        {
            //攻击动画
            animator.SetTrigger("Attack");
            //攻击状态
            state = PlayerState.attack;
        }
        //右键2号攻击
        if (Input.GetMouseButtonDown(1))
        {
            //攻击动画
            animator.SetTrigger("Attack2");
            //第二种攻击状态
            state = PlayerState.attack2;
        }
    }

    //结束攻击
    void AttackEnd()
    {
        //恢复站立状态
        state = PlayerState.idle;
    }

    //受到攻击
    public void GetDamage(int damage)
    {
        //减血
        Hp -= damage;
        //如果血量为0
        if(Hp <= 0)
        {
            state = PlayerState.die;
            //播放一次死亡动画
            animator.SetTrigger("Die");
            //死亡3秒后复活
            Invoke("Revive", 3f);
        }
    }

    //复活
    public void Revive()
    {
        //如果是死亡状态
        if(state == PlayerState.die)
        {
            //复活
            animator.SetTrigger("Revive");
            //复活为站立状态
            state = PlayerState.idle;
            //复活位置
            Hp = MaxHp;
        }
    }

    //对敌人造成伤害
    void Damage(int damage)
    {
        //获取3米内的物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider collider in colliders)
        {
            //判断敌人是不是在60度范围
            if(collider.tag == "Enemy" && Vector3.Angle(collider.transform.position - transform.position, transform.forward) < 30)
            {
                //敌人受到伤害
                collider.GetComponent<EnemyControl>().GetDamage(damage);
            }
        }
    }

    //特效
    Transform FX(string name, float desTime)
    {
        //加载特效
        GameObject fxPre = Resources.Load<GameObject>(name);
        //实例化特效
        GameObject go = Instantiate(fxPre, transform.position, transform.rotation);
        //删除特效物体
        Destroy(go, desTime);
        return go.transform;
    }

    void Attack1_1()
    {
        Damage(20);
        FX("fx_hr_arthur_attack_01_1", 0.5f);
    }

    //特效数组
    List<Transform> fxList = new List<Transform>();

    void Attack1_2()
    {
        Damage(20);
        FX("fx_hr_arthur_attack_01_2", 0.5f);
        //添加红色火焰特效
        for(int i = 0; i < 5; i++)
        {
            //创建火焰
            Transform fire = FX("Magic fire pro red", 1f);
            //加到特效数组中
            fxList.Add(fire);
            //1秒后清空数组
            Invoke("ClearFXList", 1f);
            //设置火焰旋转
            fire.transform.rotation = transform.rotation;
            fire.transform.Rotate(fire.transform.up, 15 * i - 30);
        }
    }

    void ClearFXList()
    {
        fxList.Clear();
    }

    void Attack2_0()
    {
        Damage(20);
        FX("fx_hr_arthur_pskill_03_1", 1f);
        FX("RotatorPS2", 4f);
    }

    void Attack2_1()
    {
        Damage(20);
        FX("fx_hr_arthur_pskill_01", 2f);
    }

    void Attack2_2()
    {
        Damage(20);
    }

}