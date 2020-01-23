using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;     //UIを使用可能にする


/*************************************************************
 * シュリンプのHP関連の処理を行います。
 ************************************************************/
public class HPcontrol : MonoBehaviour
{
    private new Rigidbody2D rigidbody;          //Rigidbody2D
    public GameObject Shrimp;
    public int Health = 3;                      // 体力
    public GameObject HP1;                      // プレイヤー残り体力1を示すUI
    public GameObject HP2;                      // プレイヤー残り体力2を示すUI
    public GameObject HP3;                      // プレイヤー残り体力3を示すUI
    private bool ShrimpAlpha = true;                    // アルファ値
    private bool Damage = false;                // ダメージ受けたかのフラグ
    private float AlphaNextTime = 0;            //  変更までの時間
    private float AlphaInterval = 0.05f;                // 変更周期
    private int AlphaChangeCount = 0;           //点滅回数
    private float HP1Alpha = 1;                  //HPUIのアルファ値
    private float HP2Alpha = 1;                  //HPUIのアルファ値
    private float HP3Alpha = 1;                  //HPUIのアルファ値
    public bool ShrimpDied = false;            //シュリンプ死亡確認


    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //被弾時点滅処理
        if(Damage)  //被弾しましたか？
        {
            
            if (Time.time > AlphaNextTime)　//点滅周期
            {
                if (ShrimpAlpha)    //シュリンプに色はついてますか？
                {
                    //シュリンプの色を消します
                    GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                    ShrimpAlpha = false;    //シュリンプの色が消えました。
                }
                else if (!ShrimpAlpha)  //シュリンプの色は消えていますか？
                {
                    //シュリンプに色をつけます。
                    Shrimp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                    ShrimpAlpha = true;     //シュリンプに色を付けました。
                    AlphaChangeCount++;     //点滅回数計測

                    if(AlphaChangeCount == 5)   //点滅させる回数
                    {
                        AlphaChangeCount = 0;   //点滅回数計測を初期化
                        Damage = false;         //被弾判定初期化
                    }
                }
                AlphaNextTime += AlphaInterval;     //次の点滅反転のタイミングを更新
            }
        }

        // 残り体力によって非表示にすべき体力アイコンを消去する
        if (Health == 2)
        { // 体力2になった場合
            if (HP3Alpha >= 0)
            {
                //HPUIを上に移動させつつアルファ値を下げていく
                HP3.gameObject.transform.Translate(new Vector3(0, 100, 0) * Time.deltaTime);
                HP3.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, HP3Alpha);
                HP3Alpha -= 0.1f;
            }
            else if(HP3Alpha < 0)   //HPUIの色は完全に消えましたか？ 
            {
                Destroy(HP3); // 3つめのアイコンを消去
            }
        }
        else if (Health == 1)
        { // 体力1になった場合
            if (HP2Alpha >= 0)
            {
                //HPUIを上に移動させつつアルファ値を下げていく
                HP2.gameObject.transform.Translate(new Vector3(0, 100, 0) * Time.deltaTime);
                HP2.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, HP2Alpha);
                HP2Alpha -= 0.1f;
            }
            else if (HP2Alpha < 0)  //HPUIの色は完全に消えましたか？
            {
                Destroy(HP2); // 3つめのアイコンを消去
            }
        }
        else if (Health == 0)
        { // 体力0になった場合
            ShrimpDied = true;  //シュリンプが力尽きました
            if (HP1Alpha >= 0)
            {
                //HPUIを上に移動させつつアルファ値を下げていく
                HP1.gameObject.transform.Translate(new Vector3(0, 100, 0) * Time.deltaTime);
                HP1.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, HP1Alpha);
                HP1Alpha -= 0.1f;
            }
            else if (HP1Alpha < 0)  //HPUIの色は完全に消えましたか？
            {
                Destroy(HP1); // 3つめのアイコンを消去
            }
            //SceneManager.LoadScene("α");
            //Destroy(Shrimp); //シュリンプを削除
        }
    }

    /************************************************************
    * エネミーとの当たり判定関連の処理
    ************************************************************/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /************************************************************
        * デンジャー状態のエネミーとの当たり判定処理
        ************************************************************/
        if (collision.gameObject.tag == "Danger")
        {
            Damage = true;
            AlphaNextTime = Time.time;

            Debug.Log("エネミーと衝突:Danger");
            // 残り体力によって非表示にすべき体力アイコンを消去する
            if (Health == 2)
            { // 体力2になった場合
                Destroy(HP3); // 3つめのアイコンを消去
            }
            else if (Health == 1)
            { // 体力1になった場合
                Destroy(HP2); // 2つめのアイコンを消去
            }
            else if (Health == 0)
            { // 体力0になった場合
                Destroy(HP1); // 1つめのアイコンを消去
            }
            Health--;
        }
    }
}


//途中の作業はここに書いて何してたかわかるようにする
/***************************************************
 * HP0になった後の処理を書く
*****************************************************/
