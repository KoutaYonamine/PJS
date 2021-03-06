﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchShurimp : MonoBehaviour
{
    public int defalthp;
    private int count=1;
    public Sprite[] shurimp;
    public GameObject player;
    private GameObject boutan;
    private bool clickFlg = false;

    private SpriteRenderer color;

  [SerializeField] int damageTime;

    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (clickFlg == true && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space))
            && count != 0 && player.GetComponent<HPcontrol>().Health != 0)
        {
            KeyClick();
        }

        if(count == 0 )
        {
            gameObject.tag = "Safety";
            GetComponent<SEnemyScript>().ChangMode();
            count++;
            clickFlg = false;
            time = 0;
            Destroy(boutan);
        }
        else if(clickFlg == true && count != 0)
        {

            if(color == null)
            color = GetComponent<SpriteRenderer>();

            if (color.color.b != 1)
                color.color = new Color(1,1,1,1);

            if ((time += Time.deltaTime) > damageTime)
            {
                if((player.GetComponent<HPcontrol>().Health -= 1) == 0)
                {
                    GetComponent<SEnemyScript>().mode = SEnemyScript.TEKI_MOVE.douga;
                    clickFlg = false;
                    Destroy(boutan);
                }
                time = 0;
                if(player.GetComponent<HPcontrol>().Health != 0)
                color.color = new Color(1, 0.55f, 0.55f, 1);
                
            }
        }
    }

    void KeyClick()
    {
        count--;
        GetComponent<SpriteRenderer>().sprite = shurimp[count%2];
        Debug.Log(count);
    }

    public void Scriptstart()
    {
        clickFlg = true;
        count = defalthp;

        boutan = Resources.Load<GameObject>("renndaUISprite_0");
        boutan = Instantiate(boutan);
        if(transform.position.x > 0)
            boutan.transform.position = new Vector3(transform.position.x -3, transform.position.y);
        else
            boutan.transform.position = new Vector3(transform.position.x + 3, transform.position.y);
    }
}
