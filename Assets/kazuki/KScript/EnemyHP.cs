using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyHP : MonoBehaviour
{
    private Slider hp;
    private int alpha = 0;
    private bool flashflg = false;
    private int count = 0;

    private bool running = false;
    private bool Frunning = false;
    public GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hp.value == 1)
        {
            StartCoroutine("LoadSceneGameOver");
            hand.GetComponent<SEnemyScript>().mode = SEnemyScript.TEKI_MOVE.end;
            
        }

        if (flashflg == true && count < 10 && hp.value != 1)
        {
            StartCoroutine("Flashing");
        }
        else if (count == 10)
        {
            count = 0;
            flashflg = false;
        }
    }

    public void HpDamage(float damage)
    {
        if (hp.value != 1)
        {
            hp.value += damage;

            if (hp.value > 1)
                hp.value = 1;

            flashflg = true;
        }
    }

    IEnumerator LoadSceneGameOver()
    {
        if(running)
            yield break;
        running = true;

        AsyncOperation test = SceneManager.LoadSceneAsync("α");
        test.allowSceneActivation = false;
        while(!test.isDone)
        {
            yield return new WaitForSeconds(2.0f);
            test.allowSceneActivation = true;
        }
        running = false;
    }

    IEnumerator Flashing()
    {
        if (Frunning)
            yield break;
        Frunning = true;

        hand.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha);

        if (alpha == 0)
            alpha = 1;
        else alpha = 0;

        count++;
        yield return new WaitForSeconds(0.02f);
        Frunning = false;
        
        
    }
}
