using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************
 * プレイヤーの回転処理を行います
 ************************************************************/
public class ShrimpRotation : MonoBehaviour
{
    private new Rigidbody2D rigidbody;          //Rigidbody2D
    [SerializeField]
    private float rotation_speed = 0; // プレイヤーの回転速度
    [SerializeField]
    private float RotationalSpeedIncrease = 0; //回転速度増加量
    [SerializeField]
    private float StopRotation = 0;         //回転をやめる速さ
    private int RightRotationPushed = 0;   //右回転ボタンを押した回数
    private int LeftRotationPushed = 0;   //左回転ボタンを押した回数
    private bool GetCaught;                 //シュリンプが捕まっているか

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの取得
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCaught = GetComponent<ShrimpMove>().GetCaught;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            RightRotationPushed = 0; //右回転ボタンの押した回数を初期化
            //回転速度上昇限界
            if(LeftRotationPushed <= 2)
            {
                LeftRotationPushed++;   //左回転ボタンを押した回数
            }

            //回転の方向を反転
            if (rotation_speed <= 0)
            {
                rotation_speed *= -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            LeftRotationPushed = 0; //左回転ボタンの押した回数を初期化
            //回転速度上昇限界
            if (RightRotationPushed >= -2)
            {
                RightRotationPushed--;  //右回転ボタンを押した回数
            }

            //回転の方向を反転
            if (rotation_speed >= 0)
            {
                rotation_speed *= -1;
            }
        }
        //Debug.Log(RightRotationPushed);
        //Debug.Log(LeftRotationPushed);
        //一定以下のスピードになるとします
        if (rigidbody.velocity.magnitude <= StopRotation)
        {
            if (!GetCaught)
            {
                transform.Rotate(new Vector3(0, 0, rotation_speed + (RotationalSpeedIncrease * (RightRotationPushed + LeftRotationPushed))) * Time.deltaTime, Space.Self);
            }
        }
    }

    void FixedUpdate()
    {
        
    }
}
