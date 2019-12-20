using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyHP : MonoBehaviour
{
    private Slider hp;

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
            SceneManager.LoadScene("α");
        }
    }

    public void HpDamage(float damage)
    {
        if (hp.value != 1)
        {
            hp.value += damage;

            if (hp.value > 1)
                hp.value = 1;
        }
    }
}
