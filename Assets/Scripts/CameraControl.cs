﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //玩家角色
    private Transform player;
    //保存向量
    private Vector3 dir;

    void Start()
    {
        //获取玩家角色
        player = GameObject.FindWithTag("Player").transform;
        //获取向量
        dir = player.transform.position - transform.position;
    }


    void Update()
    {
        //刷新摄像机的位置
        transform.position = player.transform.position - dir;
        //如果鼠标呼出状态，则不作任何事情
        if(Cursor.lockState == CursorLockMode.None)
        {
            return;
        }

        //获取鼠标在X轴数值
        float mouseX = Input.GetAxis("Mouse X");
        //旋转摄像机
        transform.Rotate(Vector3.up, mouseX * 90 * Time.deltaTime);
    }
}
