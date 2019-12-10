using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchShurimp : MonoBehaviour
{
    private const int defalthp = 10;
    private int count;
    public Sprite[] shurimp;
    

    // Start is called before the first frame update
    void Start()
    {
        count = defalthp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && count != 0)
        {
            KeyClick();
        }
    }

    void KeyClick()
    {
        count--;
        GetComponent<SpriteRenderer>().sprite = shurimp[count%2];
    }
}
