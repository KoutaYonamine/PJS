using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchShurimp : MonoBehaviour
{
    public int defalthp;
    private int count=1;
    public Sprite[] shurimp;
    private bool clickFlg = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (clickFlg == true && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Space))
            && count != 0)
        {
            KeyClick();

            
        }
        if(count == 0)
        {
            gameObject.tag = "Safety";
            GetComponent<SEnemyScript>().ChangMode();
            count++;
            clickFlg = false;
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
    }
}
