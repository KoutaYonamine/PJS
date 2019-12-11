using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchShurimp : MonoBehaviour
{
    private const int defalthp = 100;
    private int count;
    public Sprite[] shurimp;
    private bool clickFlg;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (clickFlg == true && Input.GetKeyDown(KeyCode.UpArrow) && count != 0)
        {
            KeyClick();
        }
    }

    void KeyClick()
    {
        count--;
        GetComponent<SpriteRenderer>().sprite = shurimp[count%2];
        Debug.Log(count);
    }

    public void scriptstart()
    {
        clickFlg = true;
        count = defalthp;
    }
}
