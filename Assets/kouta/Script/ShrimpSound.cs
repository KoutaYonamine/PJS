using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrimpSound : MonoBehaviour
{
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;
    public AudioClip sound4;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ShrimpMove>().ShrimpMoveSE)
        {
            //音(sound1)を鳴らす
            audioSource.PlayOneShot(sound1);
            GetComponent<ShrimpMove>().ShrimpMoveSE = false;
        }
        if (GetComponent<ShrimpMove>().SafetyAttackSE)
        {
            //音(sound2)を鳴らす
            audioSource.PlayOneShot(sound2);
            GetComponent<ShrimpMove>().SafetyAttackSE = false;
        }
        if (GetComponent<ShrimpMove>().SplashDownSE)
        {
            //音(sound3)を鳴らす
            audioSource.PlayOneShot(sound3);
            GetComponent<ShrimpMove>().SplashDownSE = false;
        }
        if (GetComponent<HPcontrol>().DamageSE)
        {
            //音(sound4)を鳴らす
            audioSource.PlayOneShot(sound4);
            GetComponent<HPcontrol>().DamageSE = false;
        }
    }
}
