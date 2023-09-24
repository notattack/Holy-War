using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpControl : MonoBehaviour
{
    //计时器
    private float timer = 0;

    //设置文本
    public void SetText(string text)
    {
        GetComponent<TMP_Text>().text = text;
    }


    void Update()
    {
        timer += Time.deltaTime;
        //如果超过1秒
        if(timer > 1f)
        {
            Destroy(gameObject);
        }
        //移动
        transform.Translate(Vector3.up * Time.deltaTime);
    }
}
