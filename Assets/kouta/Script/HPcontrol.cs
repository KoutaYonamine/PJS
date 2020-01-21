using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの取得
        rigidbody = GetComponent<Rigidbody2D>();
        Shrimp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        if(Damage)
        {
            
            if (Time.time > AlphaNextTime)
            {
                if (ShrimpAlpha)
                {
                    GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
                    ShrimpAlpha = false;
                }
                else if (!ShrimpAlpha)
                {
                    Shrimp.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                    ShrimpAlpha = true;
                    AlphaChangeCount++;
                    if(AlphaChangeCount == 5)
                    {
                        AlphaChangeCount = 0;
                        Damage = false;
                    }
                }
                AlphaNextTime += AlphaInterval;
            }
        }

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
            SceneManager.LoadScene("α");
            //Destroy(Shrimp); //シュリンプを削除
        }
        else if (Health == -1)
        {
            SceneManager.LoadScene("α");
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
                SceneManager.LoadScene("α");
                //Destroy(Shrimp); //シュリンプを削除
            }
            else if (Health == -1)
            {
                SceneManager.LoadScene("α");
            }
            Health--;
        }
    }
}


//途中の作業はここに書いて何してたかわかるようにする
/***************************************************
 * 
*****************************************************/
