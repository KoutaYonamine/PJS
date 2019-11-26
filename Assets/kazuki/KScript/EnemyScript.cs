using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyScript : MonoBehaviour
{
    //下までいって止まる位置
    const float stopmove = 0;
    //上まで上がっていって止まる位置
    const float startpos = 8;
    //横攻撃の初期位置
    const float sidestart = -12;
    //敵の移動スピード
    const float attckspeed = 0.1f;
    //true...攻撃     false...戻る
    private bool attckFlg;

    //止まっとく時間
    private float waittime;

    //手のイラスト　Shandが左から来る攻撃の時  Sphandが両手での攻撃の時  Uhandが現在標準
    public Sprite Shand;
    public Sprite Sphand;
    public Sprite Uhand;

    //両手攻撃の時の右手のオブジェ
    public GameObject sub;

    //両手攻撃時のアニメーション
    private Animator anim;
    private Animator anim2;

    //両手攻撃時の右手のオブジェクトを入れるやつ
    private GameObject box;

    //プルプルカウント
    private int count = 0;
    private bool puruFlg = false;
    private bool puruFlg2 = false;

    enum TEKI_MOVE
    {
        atunder,        //上から攻撃
        atside,         //横からの攻撃
        buck,           //戻っていく
        special         //両手攻撃
    }
    TEKI_MOVE mode = TEKI_MOVE.atunder;


    void Start()
    {
        attckFlg = true;
        waittime = 0;
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {

        switch (mode)
        {
            //上から攻撃
            case TEKI_MOVE.atunder: if (attckFlg = EnemyAttckUnder()) { if (gameObject.tag != "Danger") gameObject.tag = "Danger"; }
                else if ((waittime += Time.deltaTime) > 2)
                {
                    mode = TEKI_MOVE.buck;
                    waittime = 0;
                }
                else if (gameObject.tag != "Safety") gameObject.tag = "Safety"; 

                break;

            //横から攻撃
            case TEKI_MOVE.atside: if (attckFlg = EnemyAttckSide()) { if (gameObject.tag != "Danger") gameObject.tag = "Danger"; }
                else if ((waittime += Time.deltaTime) > 3)
                {
                    if (Random.Range(0, 7) == 0)
                    {
                        mode = TEKI_MOVE.atside;
                        transform.position = new Vector3(sidestart, 0);
                    }
                    else if (Random.Range(0, 1) == 0)
                    {
                        GetComponent<SpriteRenderer>().sprite = Sphand;
                        mode = TEKI_MOVE.special;
                        box = Instantiate(sub);
                        //anim2 = box.GetComponent<Animator>();
                        transform.position = new Vector3(sidestart, 0,0);
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().sprite = Uhand;
                        mode = TEKI_MOVE.atunder;
                        transform.position = new Vector3(0, startpos);
                    }

                    waittime = 0;
                }
                else if (gameObject.tag != "Safety") gameObject.tag = "Safety";
                break;

            //戻って行く
            case TEKI_MOVE.buck: if (attckFlg = EnemyBuck()) ;
                else if ((waittime += Time.deltaTime) > 2)
                {
                    if (Random.Range(0,7) == 0)
                    {
                        GetComponent<SpriteRenderer>().sprite = Shand;
                        mode = TEKI_MOVE.atside;
                        transform.position = new Vector3(sidestart, 0);
                    }
                    else if (Random.Range(0,1) == 0)
                    {
                        GetComponent<SpriteRenderer>().sprite = Sphand;
                        mode = TEKI_MOVE.special;
                        box = Instantiate(sub);
                        //anim2 = box.GetComponent<Animator>();
                        transform.position = new Vector3(sidestart, 0,0);
                    }
                    else mode = TEKI_MOVE.atunder;
                    waittime = 0;
                };
                break;
                //this.gameObject,new Vector2(12, 0),new Quaternion(0, 180, 0, 0)
            //両手攻撃
            case TEKI_MOVE.special: if (!(puruFlg) && (attckFlg = EnemySpecial("Move"))) { if (gameObject.tag != "Danger") gameObject.tag = "Danger"; }
                else if (count < 100 /*&& (waittime += Time.deltaTime) > 0.05f*/)
                {
                    //if(!(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
                    EnemySpecial("Puru");

                    //waittime = 0;
                    count+=100;
                }
                else if (!(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.5f) && (attckFlg = EnemySpecial("Atack")))
                {
                    Debug.Log(transform.position);
                    //anim.SetBool("start", false);
                    //anim2.SetBool("start", false);
                }
                else if (!(puruFlg2)&& (waittime += Time.deltaTime) > 2)
                {
                    Destroy(box);
                    count = 0;
                    puruFlg = false;
                    GetComponent<SpriteRenderer>().sprite = Uhand;
                    mode = TEKI_MOVE.buck;
                    waittime = 0;
                }
                else
                   if (gameObject.tag != "Safety") gameObject.tag = "Safety";
                
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

    //両手攻撃
    bool EnemySpecial(string mode)
    {
        bool flg = true;


        switch (mode)
        {
            case "Move":
                if (transform.position.x >= -8) { flg = false; puruFlg = true; transform.position = new Vector3(-8, 0, 0); }
                else if (flg)
                {
                    transform.position += new Vector3(attckspeed, 0);
                    box.transform.position += new Vector3(-attckspeed, 0);
                }
                break;

            case "Puru":
                
                    anim.Play("EnemyHand");
                puruFlg2 = true;
                    //anim2.SetBool("start", true);
                break;

            case "Atack":
                if (transform.position.x > -1) flg = false;
                else if (flg)
                {
                    puruFlg2 = false;
                    transform.position += new Vector3(attckspeed * 10, 0);
                    box.transform.position += new Vector3(-attckspeed * 10, 0);
                    Debug.Log("a");
                }
                
                break;
        }
        
        return flg;

    }
}
