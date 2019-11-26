using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************
 * プレイヤーの回転処理を行います
 ************************************************************/
public class ShrimpRotation : MonoBehaviour
{
    [SerializeField]
    private float rotation_speed = 0; // プレイヤーの回転速度

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate( new Vector3(0, 0, rotation_speed) * Time.deltaTime, Space.Self );
    }
}
