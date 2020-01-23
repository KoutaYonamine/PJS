using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Constant;
using UnityEngine.SceneManagement;

public class SEnemyScript : MonoBehaviour
{
    //true...攻撃     false...戻る
    private bool attckFlg;

    //止まっとく時間
    private float waittime;

    //手のイラスト　Shandが左から来る攻撃の時  Sphandが両手での攻撃の時  Uhandが現在標準
    public Sprite Shand;
    public Sprite Sphand;
    public Sprite Sphand2;
    public Sprite Sphand3;
    public Sprite Uhand;
    public Sprite Catch;

    [SerializeField,TooltipAttribute("0->上から攻撃の確率  1->横攻撃の確率  2->両手攻撃の確率")]
    int[] probability = new int[3];

    //両手攻撃の時の右手のオブジェ
    public GameObject sub;
    //両手攻撃時の右手のオブジェクトを入れるやつ
    public GameObject box;
    //プルプルカウント
    private int count = 0;

    //プレイヤーの情報
    public GameObject player;
    private Vector3 playerpos;

    //タイトルの文字とゲージ
    public GameObject title;
    public GameObject slider;
    //タイトルを表示するか
    bool titleFlg = true;
    //死んだときにα値を変える変数
    private float endalpha = 1;
    private float time = 0;

    //！のオブジェクト
    public GameObject ex;
    private GameObject exbox;
    private GameObject exbox2;
    private bool exflg = true;

    //攻撃時の色
    private float red = Enemy.redcolor;

    private GameObject prefab;
    private float galpha = 0;

    private SpriteRenderer SRender;

    public enum TEKI_MOVE
    {
        atunder,        //上から攻撃
        atside,         //横からの攻撃
        atsider,        //横からの攻撃（右から）
        buck,           //戻っていく
        special,        //両手攻撃
        pcatch,         //捕まえる
        douga,          //上に上がっていく
        end             //終わり？
    }
    public TEKI_MOVE mode = TEKI_MOVE.atunder;


    void Start()
    {
        SRender = GetComponent<SpriteRenderer>();
        attckFlg = true;
        waittime = 0;
    }


    void Update()
    {

        
        if(Input.GetKeyDown(KeyCode.P))
        {
            mode = TEKI_MOVE.douga;
        }
        if (titleFlg == true)
        {
            
            title.SetActive(true);

            GetComponent<TitleScript>().TittleScript();
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            {
                titleFlg = false;
                SRender.sprite = Uhand;
                mode = TEKI_MOVE.buck;
                player.SetActive(true);
                title.SetActive(false);
                slider.SetActive(true);
            }
        }else
        switch (mode)
        {
            //上から下に攻撃
            case TEKI_MOVE.atunder:
                    
                    if (exflg == true)
                    {
                        if ((waittime += Time.deltaTime) > 1)
                        {
                            exflg = false;
                            waittime = 0;
                        }
                    }
                    else
                    {
                        //攻撃中は触れるとプレイヤーにダメージ
                        if (attckFlg = EnemyAttckUnder()) { if (gameObject.tag != "Danger") gameObject.tag = "Danger"; }

                        //大体2秒後に上に戻る
                        else if ((waittime += Time.deltaTime) > 2) EnemyModeChang(mode);

                        //攻撃が終わった後攻撃できるように
                        else if (gameObject.tag != "Safety") gameObject.tag = "Safety";
                    }
                break;

            //横から攻撃
            case TEKI_MOVE.atside:
                    if (exflg == true && player.GetComponent<HPcontrol>().ShrimpDied == false)
                    {
                        if ((waittime += Time.deltaTime) > 1)
                        {
                            exflg = false;
                            waittime = 0;
                        }
                    }
                    else
                    {
                        //攻撃中は触れるとプレイヤーにダメージ
                        if (attckFlg = EnemyAttckSide()) { /*if (gameObject.tag != "GetHold") gameObject.tag = "GetHold";*/ }

                        //大体2秒後になにかしらの攻撃に移る
                        else if ((waittime += Time.deltaTime) > 2) { tag = "Safety"; EnemyModeChang(mode); }

                        //攻撃が終わった後攻撃できるように
                        else if (gameObject.tag != "Safety") gameObject.tag = "Safety";
                    }
                break;

            case TEKI_MOVE.atsider:
                    if (exflg == true)
                    {
                        if ((waittime += Time.deltaTime) > 1)
                        {
                            exflg = false;
                            waittime = 0;
                        }
                    }
                    else
                    {
                        //攻撃中は触れるとプレイヤーにダメージ
                        if (attckFlg = EnemyAttckSideR()) { /*if (gameObject.tag != "GetHold") gameObject.tag = "GetHold";*/ }

                        //大体2秒後になにかしらの攻撃に移る
                        else if ((waittime += Time.deltaTime) > 2) { tag = "Safety"; transform.rotation = Quaternion.identity; EnemyModeChang(mode); }

                        //攻撃が終わった後攻撃できるように
                        else if (gameObject.tag != "Safety") gameObject.tag = "Safety";
                    }
                break;

            //戻って行く
            case TEKI_MOVE.buck:
                    //上に戻っていく   *攻撃可能
                    if (attckFlg = EnemyBuck()) ;

                    //上まで戻った後に大体2秒後に攻撃に入る
                    else if ((waittime += Time.deltaTime) > 2 )
                    {
                        if (player == null || player.GetComponent<SpriteRenderer>().color.a == 0)
                        {
                            SceneManager.LoadScene("α");
                        }
                        else
                        {
                            EnemyModeChang(mode);
                            GetComponent<BoxCollider2D>().enabled = true;
                            exflg = true;
                        }
                    }
                    //else
                    //{
                    //    EnemyModeChang(mode);
                    //    GetComponent<BoxCollider2D>().enabled = true;
                    //    exflg = true;
                    //}
                    break;

            //両手攻撃
            case TEKI_MOVE.special:
                bool changflg;      //攻撃中はfalse 攻撃後にtrueに
                

                    if (exflg == true)
                    {
                        if ((waittime += Time.deltaTime) > 1)
                        {
                            exflg = false;
                            waittime = 0;
                        }
                    }
                    else
                    {
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
                    }
                break;

            case TEKI_MOVE.pcatch:
                break;
            case TEKI_MOVE.douga:

                    if (box)
                        Destroy(box);
                    if (exbox)
                        Destroy(exbox);
                    if (exbox2)
                        Destroy(exbox2);

                   

                    if (SRender.sprite == Catch)
                    {
                        mode = TEKI_MOVE.buck;
                        Debug.Log("a");
                        
                    }
                    else
                    {
                        mode = TEKI_MOVE.atside;
                        transform.position = new Vector3(Enemy.sidestart, player.transform.position.y);
                        SRender.sprite = Shand;
                    }

                break;
            case TEKI_MOVE.end:
                    

                    if (endalpha > 0 && (time += Time.deltaTime) > 0.1f)
                    {
                        SRender.color = new Color(1, 1, 1, endalpha -= 0.1f);
                        slider.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color(1, 1, 1, endalpha);
                        slider.GetComponentInChildren<Image>().color = new Color(1, 1, 1, endalpha);
                        if (box != null)
                            box.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, endalpha);

                        time = 0;
                    }
                    else if(endalpha < 0 &&(time += Time.deltaTime) > 0.1f)
                    {
                        if (prefab == null)
                        {
                            prefab = Resources.Load<GameObject>("gekitai");
                            prefab = Instantiate(prefab);
                        }
                        else if(galpha <= 1) prefab.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, galpha += 0.1f);

                        time = 0;
                    }

                    break;
        }
    }

    //上から下に攻撃
    bool EnemyAttckUnder()
    {
        bool flg = true;

        if (exbox != null && transform.position.y < 5.0f)
            Destroy(exbox);

        if (transform.position.y < Enemy.stopmove)
        {
            flg = false;
            SRender.color = new Color(1, 1, 1, 1);
        }
        if (flg)
        {
            transform.position -= new Vector3(0, Enemy.attckspeed);

            SRender.color = new Color(1,red,red,1);

            
        }
        return flg;

    }

    //左から右に攻撃
    bool EnemyAttckSide()
    {
        bool flg = true;

        if (exbox != null && transform.position.x > -6.5f)
            Destroy(exbox);

        if (transform.position.x > Enemy.sidestop)
        {
            flg = false;
            SRender.color = new Color(1, 1, 1, 1);
        }
        if (flg)
        {
            transform.position += new Vector3(Enemy.attckspeed, 0);
            SRender.color = new Color(1, red, red, 1);
        }
        return flg;
    }

    //右から左に攻撃
    bool EnemyAttckSideR()
    {
        bool flg = true;

        if (exbox != null && transform.position.x < 6.5f)
            Destroy(exbox);

        if (transform.position.x < -Enemy.sidestop)
        {
            flg = false;
            SRender.color = new Color(1, 1, 1, 1);
        }
        if (flg)
        {
            transform.position -= new Vector3(Enemy.attckspeed, 0);
            SRender.color = new Color(1, red, red, 1);
        }
        return flg;
    }

    //上に戻る
    bool EnemyBuck()
    {
        bool flg = true;

        if (transform.position.y > Enemy.startpos)
        {
            flg = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
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
            SRender.color = new Color(1, red, red, 1);
            box.GetComponent<SpriteRenderer>().color = new Color(1, red, red, 1);
        }
        else if (count < 40) puruflg = true;
        else  attckflg = true;
        
            
        if (puruflg)
        {
            if (exbox != null)
            {
                Destroy(exbox);
                if (exbox2 != null)
                    Destroy(exbox2);
            }
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
            if(transform.position.y - player.transform.position.y > 1 )
            {
                transform.position -= new Vector3(0, 0.1f);
                box.transform.position -= new Vector3(0, 0.1f);
            }
            else if(transform.position.y - player.transform.position.y < -1)
            {
                transform.position += new Vector3(0, 0.1f);
                box.transform.position += new Vector3(0, 0.1f);
            }
        }

        if (attckflg == true && transform.position.x < Enemy.attckstoppos && (waittime += Time.deltaTime) > 0.75f)
        {
            if (SRender.sprite != Sphand2)
            {
                SRender.sprite = Sphand2;
                box.GetComponent<SpriteRenderer>().sprite = Sphand2;
            }
            else
            {
                transform.position += new Vector3(attckspeed * 10, 0);
                box.transform.position += new Vector3(-attckspeed * 10, 0);
            }

            if (transform.position.x >= Enemy.attckstoppos)
                waittime = 0;
        }
        else if (transform.position.x >= Enemy.attckstoppos && (waittime += Time.deltaTime) > 0.3f)
        {
            endflg = true;
        }
        else if(transform.position.x >= Enemy.attckstoppos )
        {
            if (SRender.sprite != Sphand3)
            {
                SRender.sprite = Sphand3;
                box.GetComponent<SpriteRenderer>().sprite = Sphand3;
            }

            if (waittime > 0.1f)
            {
                SRender.color = new Color(1, 1, 1, 1);
                box.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
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
                exflg = true;
                if (Random.Range(0, probability[1]) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        mode = TEKI_MOVE.atside;
                        if (playerpos != null)
                        {
                            transform.position = new Vector3(sidestart, playerpos.y);
                            exbox = Instantiate(ex);
                            exbox.transform.position = new Vector3(-7.3f, playerpos.y);
                        }
                        else transform.position = new Vector3(sidestart, 0);
                        gameObject.tag = "GetHold";
                    }
                    else
                    {
                        mode = TEKI_MOVE.atsider;
                        transform.rotation = new Quaternion(0,180,0,0);
                        if (playerpos != null)
                        {
                            transform.position = new Vector3(-sidestart, playerpos.y);
                            exbox = Instantiate(ex);
                            exbox.transform.position = new Vector3(7.3f, playerpos.y);
                        }
                        else transform.position = new Vector3(-sidestart, 0);
                        gameObject.tag = "GetHold";

                    }

                }
                else if (Random.Range(0, probability[2]) == 0)
                {
                    SRender.sprite = Sphand;
                    mode = TEKI_MOVE.special;
                    box = Instantiate(sub);
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(sidestart, playerpos.y);
                        box.transform.position = new Vector3(-sidestart, playerpos.y);

                        exbox = Instantiate(ex);
                        exbox.transform.position = new Vector3(7.3f, playerpos.y);
                        exbox2 = Instantiate(ex);
                        exbox2.transform.position = new Vector3(-7.3f, playerpos.y);
                    }
                    else
                        transform.position = new Vector3(sidestart, 0);
                }
                else
                {
                    SRender.sprite = Uhand;
                    mode = TEKI_MOVE.atunder;
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(playerpos.x, Enemy.startpos);
                        exbox = Instantiate(ex);
                        exbox.transform.position = new Vector3(playerpos.x, 3.7f);
                    }
                    else transform.position = new Vector3(0, Enemy.startpos);
                }
                waittime = 0;
                break;

            case TEKI_MOVE.atsider:
                exflg = true;
                if (Random.Range(0, probability[1]) == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        mode = TEKI_MOVE.atside;
                        if (playerpos != null)
                        {
                            transform.position = new Vector3(sidestart, playerpos.y);
                            exbox = Instantiate(ex);
                            exbox.transform.position = new Vector3(-7.3f, playerpos.y);
                        }
                        else transform.position = new Vector3(sidestart, 0);
                        gameObject.tag = "GetHold";
                    }
                    else
                    {
                        mode = TEKI_MOVE.atsider;
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                        if (playerpos != null)
                        {
                            transform.position = new Vector3(-sidestart, playerpos.y);
                            exbox = Instantiate(ex);
                            exbox.transform.position = new Vector3(7.3f, playerpos.y);
                        }
                        else transform.position = new Vector3(-sidestart, 0);
                        gameObject.tag = "GetHold";

                    }

                }
                else if (Random.Range(0, probability[2]) == 0)
                {
                    SRender.sprite = Sphand;
                    mode = TEKI_MOVE.special;
                    box = Instantiate(sub);
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(sidestart, playerpos.y);
                        box.transform.position = new Vector3(-sidestart, playerpos.y);

                        exbox = Instantiate(ex);
                        exbox.transform.position = new Vector3(7.3f, playerpos.y);
                        exbox2 = Instantiate(ex);
                        exbox2.transform.position = new Vector3(-7.3f, playerpos.y);
                    }
                    else
                        transform.position = new Vector3(sidestart, 0);
                }
                else
                {
                    SRender.sprite = Uhand;
                    mode = TEKI_MOVE.atunder;
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(playerpos.x, Enemy.startpos);
                        exbox = Instantiate(ex);
                        exbox.transform.position = new Vector3(playerpos.x, 3.7f);
                    }
                    else transform.position = new Vector3(0, Enemy.startpos);
                }
                waittime = 0;
                break;


            //上へ戻る行動へ
            case TEKI_MOVE.special:
                Destroy(box);
                count = 0;
                SRender.sprite = Uhand;
                transform.position = new Vector3(0, transform.position.y);
                mode = TEKI_MOVE.buck;
                waittime = 0;
                break;
            
            //攻撃へ
            case TEKI_MOVE.buck:

                if (transform.rotation != Quaternion.identity)
                    transform.rotation = Quaternion.identity;
                if (Random.Range(0, probability[1]) == 0)
                {
                    SRender.sprite = Shand;
                    if (Random.Range(0, 2) == 0)
                    {
                        mode = TEKI_MOVE.atside;
                        if (playerpos != null)
                        {
                            transform.position = new Vector3(sidestart, playerpos.y);
                            exbox = Instantiate(ex);
                            exbox.transform.position = new Vector3(-7.3f, playerpos.y);
                        }
                        else transform.position = new Vector3(sidestart, 0);
                        gameObject.tag = "GetHold";
                    }
                    else
                    {
                        mode = TEKI_MOVE.atsider;
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                        if (playerpos != null)
                        {
                            transform.position = new Vector3(-sidestart, playerpos.y);
                            exbox = Instantiate(ex);
                            exbox.transform.position = new Vector3(7.3f, playerpos.y);
                        }
                        else transform.position = new Vector3(-sidestart, 0);
                        gameObject.tag = "GetHold";

                    }
                }
                else if (Random.Range(0, probability[2]) == 0)
                {
                    SRender.sprite = Sphand;
                    mode = TEKI_MOVE.special;
                    box = Instantiate(sub);
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(sidestart, playerpos.y);
                        box.transform.position = new Vector3(-sidestart, playerpos.y);

                        exbox = Instantiate(ex);
                        exbox.transform.position = new Vector3(7.3f, playerpos.y);
                        exbox2 = Instantiate(ex);
                        exbox2.transform.position = new Vector3(-7.3f, playerpos.y);
                    }
                    else
                        transform.position = new Vector3(sidestart, 0);
                }
                else
                {
                    mode = TEKI_MOVE.atunder;
                    if (playerpos != null)
                    {
                        transform.position = new Vector3(playerpos.x, Enemy.startpos);
                        exbox = Instantiate(ex);
                        exbox.transform.position = new Vector3(playerpos.x, 3.7f);
                    }
                    else transform.position = new Vector3(0, Enemy.startpos);
                }
                waittime = 0;
                break;
        }
    }
    
    //つかみ攻撃当たった時
    public void OnCollisionEnter2D()
    {
        if (gameObject.tag == "GetHold" && (mode == TEKI_MOVE.atside || mode == TEKI_MOVE.atsider) && player.GetComponent<HPcontrol>().ShrimpDied == false)
        {
            transform.position = player.transform.position;
            SRender.sprite = Catch;
            mode = TEKI_MOVE.pcatch;
            GetComponent<CatchShurimp>().Scriptstart();
            SRender.color = new Color(1, 1, 1, 1);
            if (exbox != null)
                Destroy(exbox);
        }
        else if (player.GetComponent<HPcontrol>().ShrimpDied == true)
        {
            transform.position = player.transform.position;
            SRender.sprite = Catch;
            mode = TEKI_MOVE.buck;
            SRender.color = new Color(1, 1, 1, 1);
            Destroy(player);
        }

    }

    //捕まえてる海老が逃げた時
    public void ChangMode()
    {
        SRender.sprite = Uhand;
        mode = TEKI_MOVE.buck;
        player.GetComponent<ShrimpMove>().GetCaught = false;
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

}
