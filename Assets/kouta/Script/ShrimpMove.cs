using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************
 * シュリンプの移動関連の処理を行います。
 ************************************************************/
public class ShrimpMove : MonoBehaviour
{
    private new Rigidbody2D rigidbody;          //Rigidbody2D
    [SerializeField]
    private float Initial_velocity = 0;         //シュリンプの初速度
    [SerializeField]
    private float Drop_speed = 0;               //シュリンプの落下速度
    [SerializeField]
    private float Deceleration_coefficient = 0; //シュリンプへの減速係数　※大きくすると減速度が大きくなる
    [SerializeField]
    private float Safety_Repulsion;             //シュリンプがタグ:Chanceに触れた時の反発係数
    [SerializeField]
    private float SideWall_Repulsion;           //シュリンプがタグ:SideWallに触れた時の反発係数
    [SerializeField]
    private float UnderWall_Repulsion;          //シュリンプがタグ:UnderWallに触れた時の反発係数
    Vector3 move_force;                         //水中移動に掛かる力
    Vector3 UnderWater_force;                   //空中移動に掛かる力
    bool SpaceButtonPushed = false;             //Spaceが押されたかのフラグ
    bool UnderWaterStayed = false;              //シュリンプが水中にいるかのフラグ
    bool AerialStayed = false;                  //シュリンプが空中にいるかのフラグ
    bool Safety = false;                        //Chanceに触れたかのフラグ
    bool SideWall = false;                      //SideWallに触れたかのフラグ
    bool UnderWall = false;                      //UnderWallに触れたかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    /**********************************************************
     * Update関数内では、Rigidbodyの使用は原則禁止
     * Rigidbodyを使う処理はFixedUpdate内に記述しましょう！
     * Update is called once per frame
    ************************************************************/
    void Update()
    {
        //SpaceKeyの入力処理
        if (UnderWaterStayed)   //水中にいますか？
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Keypad5)
                || Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                SpaceButtonPushed = true;   //SpaceKeyが押されました
                //Debug.Log("Update内のGetKeyDown");
            }

        }
    }

    /**************************************************
     * FixedUpdate関数内では、Input系の使用は原則禁止
     * Input系を使う処理はUpdate関数内に記述しましょう！
    ****************************************************/
    void FixedUpdate()
    {
        /************************************************************
        * シュリンプの水中での処理
        ************************************************************/
        //水中にいてSpaceKeyが押されたらシュリンプが移動
        if (UnderWaterStayed && SpaceButtonPushed)
        {
            //Debug.Log("スペースが押されました");
            //シュリンプの進行方向に加わる力を格納
            move_force = this.gameObject.transform.rotation * new Vector3(Initial_velocity, 0, 0);
            //Rigidbody2Dに力を加える（シュリンプ発射）
            rigidbody.AddForce(move_force, ForceMode2D.Impulse);
            SpaceButtonPushed = false;  //SpaceKey入力のリセット

        }

        /************************************************************
        * シュリンプの空中での処理
        ************************************************************/
        if (AerialStayed)   //空中にいますか？
        {
            //シュリンプの落下処理
            UnderWater_force = new Vector3(0, Drop_speed, 0);
            rigidbody.AddForce(-UnderWater_force);
        }
        //シュリンプの減速処理
        rigidbody.AddForce(-rigidbody.velocity * Deceleration_coefficient);

        /************************************************************
        * アタック後のシュリンプの挙動処理
        ************************************************************/
        //Chance状態のエネミーにアタック後
        if (Safety)
        {
            rigidbody.velocity = rigidbody.velocity * Safety_Repulsion;
            Safety = false;
        }
        //Tag:SideWallにアタック後
        if (SideWall)
        {
            rigidbody.velocity = rigidbody.velocity * SideWall_Repulsion;
            SideWall = false;
        }
        //Tag:UnderWallにアタック後
        if (UnderWall)
        {
            rigidbody.velocity = rigidbody.velocity * UnderWall_Repulsion;
            UnderWall = false;
        }
        
    }

    /************************************************************
    * シュリンプのセクションエリア毎の処理
    ************************************************************/
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("コリジョンに触れています");
        /************************************************************
        * シュリンプの水中での処理
        ************************************************************/
        if (collision.gameObject.tag == "Underwater")
        {
            //Debug.Log("水中にいます");
            UnderWaterStayed = true;    //シュリンプは水中にいます
            AerialStayed = false;       //シュリンプは空中にいません
        }
        /************************************************************
        * シュリンプの空中での処理
        ************************************************************/
        if (collision.gameObject.tag == "Aerial")
        {
            //Debug.Log("空中にいます");
            UnderWaterStayed = false;   //シュリンプは水中にいません
            AerialStayed = true;        //シュリンプは空中にいます
        }
    }

    /************************************************************
    * 当たり判定関連の処理
    ************************************************************/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /************************************************************
        * Chance状態のエネミーとの当たり判定処理
        ************************************************************/
        if (collision.gameObject.tag == "Safety")
        {
            Safety = true;
        }
        /************************************************************
        * Tag:SideWallとの当たり判定処理
        ************************************************************/
        if (collision.gameObject.tag == "SideWall")
        {
            SideWall = true;
        }
        /************************************************************
        * Tag:UnderWallとの当たり判定処理
        ************************************************************/
        if (collision.gameObject.tag == "UnderWall")
        {
            UnderWall = true;
        }
    }
}


//途中の作業はここに書いて何してたかわかるようにする
/***************************************************
 * 
*****************************************************/