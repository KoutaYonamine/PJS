using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    public Sprite[] ebi;
    float waittime;

    int num;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
            UnityEngine.Application.Quit();
        }
    }

    public void TittleScript()
    {
        if ((waittime += Time.deltaTime) > 0.3f)
        {
            GetComponent<SpriteRenderer>().sprite = ebi[num];
            num++;
            if (num == 4)
                num = 0;
            waittime = 0;
        }
    }
}
