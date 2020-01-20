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
    public int Health = 2;                         // 体力
    public GameObject HP1;                      // プレイヤー残り体力1を示すUI
    public GameObject HP2;                      // プレイヤー残り体力2を示すUI
    public GameObject HP3;                      // プレイヤー残り体力3を示すUI

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
