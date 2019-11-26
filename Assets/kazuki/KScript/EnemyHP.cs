using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        
    }

    public void HpDamage(float damage)
    {
        hp.value += damage;
    }
}
