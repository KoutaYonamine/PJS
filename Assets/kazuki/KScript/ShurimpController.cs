using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;
using UnityEngine.UI;

public class ShurimpController : MonoBehaviour
{
    Rigidbody2D rig;
    Vector2 Pposition;

    bool RotaFlg;

    public Slider enemyhp;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Pposition = transform.position;
        RotaFlg = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (RotaFlg)
        {
            Protate();
            Getkeystate();
        }
    }
    private void FixedUpdate()
    {
        
    }
    void Protate()
    {
        transform.Rotate(0, 0, -1f);
    }
    void Getkeystate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(transform.up * 3, ForceMode2D.Impulse);
            enemyhp.GetComponent<EnemyHP>().HpDamage(0.01f);
            //RotaFlg = false;
        }
    }

}
