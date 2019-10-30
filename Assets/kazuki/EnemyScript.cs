using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //下までいって止まる位置
    const float stopmove = 0;
    //上まで上がっていって止まる位置
    const float startmove = 6;
    //敵の移動スピード
    const float attckspeed = 0.1f;
    //true...攻撃     false...戻る
    private bool attckFlg;

    //止まっとく時間
    private float waittime;

    void Start()
    {
        attckFlg = true;
        waittime = 0;
    }

    
    void Update()
    {

        if (attckFlg && EnemyAttckUnder())
        {
            waittime += Time.deltaTime;
            if (waittime > 1)
            {
                attckFlg = false;
                waittime = 0;
            }
        }
        else if (attckFlg == false && EnemyBuck())
        {
            waittime += Time.deltaTime;
            if (waittime > 1.5f)
            {
                attckFlg = true;
                waittime = 0;
            }
        }
    }

    //上から下に攻撃
    bool EnemyAttckUnder()
    {
        bool flg = false;
        if (transform.position.y < stopmove)
            flg = true;
        if(!(flg))
        transform.position -= new Vector3(0,attckspeed);
        
        
        return flg;

    }
    //右から左に攻撃
    bool EnmeyAttckSide()
    {
        bool flg = false;
        if (transform.position.x > 10)
            flg = true;
        if (!(flg))
            transform.position -= new Vector3(attckspeed, 0);
        return flg;
    }

    //上に戻る
    bool  EnemyBuck()
    {
        bool flg = false;
        if (transform.position.y > startmove)
            flg = true;
        if(!(flg))
        transform.position += new Vector3(0, attckspeed);
        

        return flg;
        
    }

    
}
