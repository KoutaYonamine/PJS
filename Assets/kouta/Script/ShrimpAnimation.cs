using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************************
 * シュリンプのアニメーションを実行します。
 ************************************************************/
public class ShrimpAnimation : MonoBehaviour
{
    private new Rigidbody2D rigidbody;          //Rigidbody2D
    [SerializeField]
    private float IdleSpeed = 0;                //Idleに戻すためのスピード

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Keyの入力処理
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Keypad5)
                || Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<Animator>().SetTrigger("OnceAttack");
        }

        
    }

    void FixedUpdate()
    {
        //一定以下のスピードになるとIdleにアニメーションします
        if (rigidbody.velocity.magnitude < IdleSpeed)
        {
            GetComponent<Animator>().SetTrigger("IdleSpeed");
        }
    }

        /************************************************************
        * 当たり判定関連の処理
        ************************************************************/
        private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Animator>().SetTrigger("Collision");
    }
}
