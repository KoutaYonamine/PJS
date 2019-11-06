using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //下までいって止まる位置
    const float stopmove = 0;
    //上まで上がっていって止まる位置
    const float startpos = 8;

    const float sidestart = -12;
    //敵の移動スピード
    const float attckspeed = 0.1f;
    //true...攻撃     false...戻る
    private bool attckFlg;

    //止まっとく時間
    private float waittime;

    public Sprite hand;
    public Sprite hand2;
    public Sprite blue;
    public GameObject sub;

    private GameObject box;
    private int change = 1;
    private int count = 0;

    enum TEKI_MOVE
    {
        atunder,
        atside,
        buck,
        special
    }
    TEKI_MOVE mode = TEKI_MOVE.atunder;


    void Start()
    {
        attckFlg = true;
        waittime = 0;
    }

    
    void Update()
    {

        switch (mode)
        {
            case TEKI_MOVE.atunder: if (attckFlg = EnemyAttckUnder()) ;
                else if ((waittime += Time.deltaTime) > 1)
                {
                    mode = TEKI_MOVE.buck;
                    waittime = 0;
                };

                break;

            case TEKI_MOVE.atside: if(attckFlg = EnemyAttckSide()) ;
                else if ((waittime += Time.deltaTime) > 3)
                {
                    int num = Random.Range(0, 6);
                    if (num == 0 || num == 1)
                    {
                        mode = TEKI_MOVE.atside;
                        transform.position = new Vector3(sidestart, 0);
                    }
                    else if (num == 2)
                    {
                        GetComponent<SpriteRenderer>().sprite = hand2;
                        mode = TEKI_MOVE.special;
                        box = Instantiate(sub);
                        transform.position = new Vector3(sidestart, 0);
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = blue;
                        mode = TEKI_MOVE.atunder;
                        transform.position = new Vector3(0, startpos);
                    }

                    waittime = 0;
                };
                break;

            case TEKI_MOVE.buck: if (attckFlg = EnemyBuck()) ;
                else if ((waittime += Time.deltaTime) > 1)
                {
                    int num = Random.Range(0, 6);
                    Debug.Log(num);
                    if (num == 0 || num == 1)
                    {
                        GetComponent<SpriteRenderer>().sprite = hand;
                        mode = TEKI_MOVE.atside;
                        transform.position = new Vector3(sidestart, 0);
                    }
                    else if (num == 2 || num == 3)
                    {
                        GetComponent<SpriteRenderer>().sprite = hand2;
                        mode = TEKI_MOVE.special;
                        box = Instantiate(sub);
                        transform.position = new Vector3(sidestart, 0);
                    }
                    else mode = TEKI_MOVE.atunder;
                    waittime = 0;
                };
                break;

            case TEKI_MOVE.special: if (attckFlg = EnemySpecial(1)) ;
                else if (count < 30 && (waittime += Time.deltaTime) > 0.05f)
                {
                    EnemySpecial(2);
                    waittime = 0;
                    count++;
                }
                else if ((count >= 30) && (attckFlg = EnemySpecial(3)))
                    ;
                else if((count >= 30) && (waittime += Time.deltaTime) > 2)
                {
                    Destroy(box);
                    count = 0;
                    GetComponent<SpriteRenderer>().sprite = blue;
                    mode = TEKI_MOVE.buck;
                    waittime = 0;
                };



                break;
        }

        
    }

    //上から下に攻撃
    bool EnemyAttckUnder()
    {
        bool flg = true;

        if (transform.position.y < stopmove) flg = false;
        if(flg) transform.position -= new Vector3(0,attckspeed);
        
        return flg;

    }
    //左から右に攻撃
    bool EnemyAttckSide()
    {
        bool flg = true;

        if (transform.position.x > 10) flg = false;
        if (flg) transform.position += new Vector3(attckspeed, 0);

        return flg;
    }

    //上に戻る
    bool  EnemyBuck()
    {
        bool flg = true;

        if (transform.position.y > startpos) flg = false;
        if(flg) transform.position += new Vector3(0, attckspeed);
        
        return flg;
        
    }

    bool EnemySpecial(int mode)
    {
        bool flg = true;

        if (mode == 1)
        {
            if (transform.position.x > -8) flg = false;
            if (flg)
            {
                transform.position += new Vector3(attckspeed, 0);
                box.transform.position += new Vector3(-attckspeed, 0);
            }
        }
        else if(mode == 2)
        {
            transform.position += new Vector3(attckspeed * 2 * change, 0);
            box.transform.position += new Vector3(-attckspeed * 2 * change, 0);

            change *= -1;
        }
        else if(mode == 3)
        {
            if (transform.position.x > -1) flg = false;
            if (flg)
            {
                transform.position += new Vector3(attckspeed * 4 , 0);
                box.transform.position += new Vector3(-attckspeed * 4 , 0);
            }
        }
        return flg;

    }
}
