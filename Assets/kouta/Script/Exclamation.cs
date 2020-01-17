using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************
 * エクスクラメーションに関する処理を行います。
 ************************************************************/
public class Exclamation : MonoBehaviour
{
    [SerializeField]
    private float Size;                 //エクスクラメーションの大きさです。
    [SerializeField]
    private float Speed;                //大きさを変える速さです。
    private bool SizeSwitching = true;      //大きくする、小さくするを切り替えます。

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //エクスクラメーションの拡大縮小
        if (SizeSwitching && Size <= 0.35f)
        {
            Size += Speed;

            if (Size >= 0.35f)
            {
                SizeSwitching = false;
            }
        }

        if (SizeSwitching == false && Size >= 0.3f)
        {
            Size -= Speed;

            if (Size <= 0.3f)
            {
                SizeSwitching = true;
            }
        }

        this.transform.localScale = new Vector3(Size,Size,0);
    }
}
