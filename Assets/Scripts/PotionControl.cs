using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //获取玩家脚本
            PlayerControl player = other.GetComponent<PlayerControl>();
            //添加血量
            player.Hp += 10;
            if(player.Hp > player.MaxHp)
            {
                player.Hp = player.MaxHp;
            }
            //删除自己
            Destroy(gameObject);
        }
    }

}
