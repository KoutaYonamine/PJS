using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;

public class SEnemyScript : MonoBehaviour
{
    //true...攻撃     false...戻る
    private bool attckFlg;

    //止まっとく時間
    private float waittime;

    //手のイラスト　Shandが左から来る攻撃の時  Sphandが両手での攻撃の時  Uhandが現在標準
    public Sprite Shand;
    public Sprite Sphand;
    public Sprite Uhand;
    public Sprite Catch;

    [SerializeField,TooltipAttribute("0->上から攻撃の確率  1->横攻撃の確率  2->両手攻撃の確率")]
    int[] probability = new int[3];

    //両手攻撃の時の右手のオブジェ
    public GameObject sub;
    //両手攻撃時の右手のオブジェクトを入れるやつ
    private GameObject box;
    //プルプルカウント
    private int count = 0;

    public GameObject player;
    private Vector3 playerpos;

    enum TEKI_MOVE
    {
        atunder,        //上から攻撃
        atside,         //横からの攻撃
        buck,           //戻っていく
        special,        //両手攻撃
        pcatch          //捕まえる
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
            //上から下に攻撃
            case TEKI_MOVE.atunder:
                //攻撃中は触れるとプレイヤーにダメージ
                if (attckFlg = EnemyAttckUnder()) { if (gameObject.tag != "Danger") gameObject.tag = "Danger"; }

                //大体2秒後に上に戻る
                else if ((waittime += Time.deltaTime) > 2) EnemyModeChang(mode);

                //攻撃が終わった後攻撃できるように
                else if (gameObject.tag != "Safety") gameObject.tag = "Safety";

                break;

            //横から攻撃
            case TEKI_MOVE.atside:
                //攻撃中は触れるとプレイヤーにダメージ
                if (attckFlg = EnemyAttckSide()) { if (gameObject.tag != "Danger") gameObject.tag = "Danger"; }

                //大体2秒後になにかしらの攻撃に移る
                else if ((waittime += Time.deltaTime) > 2) { EnemyModeChang(mode); tag = "Safety"; }

                //攻撃が終わった後攻撃できるように
                else if (gameObject.tag != "Safety") gameObject.tag = "Safety";

                break;

            //戻って行く
            case TEKI_MOVE.buck:
                //上に戻っていく   *攻撃可能
                if (attckFlg = EnemyBuck()) ;

                //上まで戻った後に大体2秒後に攻撃に入る
                else if ((waittime += Time.deltaTime) > 2) EnemyModeChang(mode);

                break;

            //両手攻撃
            case TEKI_MOVE.special:
                bool changflg;      //攻撃中はfalse 攻撃後にtrueに
                changflg = EnemySpecial();  //攻撃中なのか判断


                if (changflg) //攻撃終わり
                {
                    //攻撃できるように
                    if (gameObject.tag != "Safety") { gameObject.tag = "Safety"; box.tag = "Safety"; }
                    //2秒後に上に戻る
                    if ((waittime += Time.deltaTime) > 2)
                    {
                        EnemyModeChang(mode);
                    }
                }
                //攻撃中
                else if (gameObject.tag != "Danger") { gameObject.tag = "Danger"; box.tag = "Danger"; }

                break;

            case TEKI_MOVE.pcatch:
                break;
        }
    }

    //上から下に攻撃
    bool EnemyAttckUnder()
    {
        bool flg = true;

        if (transform.position.y < Enemy.stopmove) flg = false;
        if (flg) transform.position -= new Vector3(0, Enemy.attckspeed);

        return flg;

    }
    //左から右に攻撃
    bool EnemyAttckSide()
    {
        bool flg = true;

        if (transform.position.x > Enemy.sidestop) flg = false;
        if (flg) transform.position += new Vector3(Enemy.attckspeed, 0);

        return flg;
    }

    //上に戻る
    bool EnemyBuck()
    {
        bool flg = true;

        if (transform.position.y > Enemy.startpos) flg = false;
        if (flg) transform.position += new Vector3(0, Enemy.attckspeed);

        return flg;

    }

    //両手攻撃
    bool EnemySpecial()
    {
        bool puruflg = false;      //trueなら震える
        bool attckflg = false;     //trueなら攻撃
        bool endflg = false;       //trueなら攻撃終了
        float attckspeed = Enemy.attckspeed;

        if (transform.position.x < Enemy.purustartpos)
        {
            transform.position += new Vector3(attckspeed, 0);
            box.transform.position += new Vector3(-attckspeed, 0);
            Debug.Log("1");
        }
        else if (count < 40) puruflg = true;
        else  attckflg = true;
        
            
        if (puruflg)
        {
            int inversion;
            if (count % 2 == 0)
                inversion = 1;
            else inversion = -1;

            if ((waittime += Time.deltaTime) > 0.04f)
            {
                transform.position += new Vector3(Enemy.puruspeed * inversion, 0);
                box.transform.position += new Vector3(Enemy.puruspeed * inversion, 0);
                waittime = 0;
                count++;
            }
            
            Debug.Log("2");
        }

        if (attckflg == true && transform.position.x < Enemy.attckstoppos)
        {
            transform.position += new Vector3(attckspeed * 10, 0);
            box.transform.position += new Vector3(-attckspeed * 10, 0);
            Debug.Log("3");
        }
        else if (transform.position.x >= Enemy.attckstoppos && (waittime += Time.deltaTime) > 0.3f)
        {
            endflg = true;
        }
       
        return endflg;
    }

    //敵のモーションチェンジ
    void EnemyModeChang(TEKI_MOVE Mode)
    {
        float sidestart = Enemy.sidestart;
        
        

        if(player != null)
        playerpos = player.transform.position;

        switch (Mode)
        {
            //上に戻る行動へ
            case TEKI_MOVE.atunder:
                mode = TEKI_MOVE.buck;
                waittime = 0;
                break;

            //攻撃へ
            case TEKI_MOVE.atside:
                if (Random.Range(0, probability[1]) == 0)
                {
                    mode = TEKI_MOVE.atside;
                    if (playerpos != null)
                        transform.position = new Vector3(sidestart, playerpos.y);
                    else transform.position = new Vector3(sidestart, 0);
                    tag = "get hold";
                }
                else if (Random.Range(0, probability[2]) == 0)
                {
                    GetComponent<SpriteRenderer>().sprite = Sphand;
                    mode = TEKI_MOVE.special;
                    box = Instantiate(sub);
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(sidestart, playerpos.y);
                        box.transform.position = new Vector3(-sidestart, playerpos.y);
                    }
                    else
                        transform.position = new Vector3(sidestart, 0);
                }
                else
                {
                    GetComponent<SpriteRenderer>().sprite = Uhand;
                    mode = TEKI_MOVE.atunder;
                    if (playerpos != null)
                        transform.position = new Vector3(playerpos.x, Enemy.startpos);
                    else transform.position = new Vector3(0, Enemy.startpos);
                }
                waittime = 0;
                break;

            //上へ戻る行動へ
            case TEKI_MOVE.special:
                Destroy(box);
                count = 0;
                GetComponent<SpriteRenderer>().sprite = Uhand;
                mode = TEKI_MOVE.buck;
                waittime = 0;
                break;
            
            //攻撃へ
            case TEKI_MOVE.buck:
                if (Random.Range(0, probability[1]) == 0)
                {
                    GetComponent<SpriteRenderer>().sprite = Shand;
                    mode = TEKI_MOVE.atside;
                    if (playerpos != null)
                        transform.position = new Vector3(sidestart, playerpos.y);
                    else transform.position = new Vector3(sidestart, 0);
                    tag = "get hold";
                }
                else if (Random.Range(0, probability[2]) == 0)
                {
                    GetComponent<SpriteRenderer>().sprite = Sphand;
                    mode = TEKI_MOVE.special;
                    box = Instantiate(sub);
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(sidestart, playerpos.y);
                        box.transform.position = new Vector3(-sidestart, playerpos.y);
                    }
                    else
                        transform.position = new Vector3(sidestart, 0);
                }
                else
                {
                    mode = TEKI_MOVE.atunder;
                    if (playerpos != null)
                        transform.position = new Vector3(playerpos.x, Enemy.startpos);
                    else transform.position = new Vector3(0, Enemy.startpos);
                }
                waittime = 0;
                break;
        }
    }


    public void OnCollisionEnter2D()
    {
        if (tag == "get hold")
        {
            transform.position += new Vector3(2, 0);
            GetComponent<SpriteRenderer>().sprite = Catch;
            mode = TEKI_MOVE.pcatch;
            GetComponent<CatchShurimp>().Scriptstart();
        }
    }

}
