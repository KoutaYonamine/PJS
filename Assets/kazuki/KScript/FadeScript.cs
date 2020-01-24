using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    private GameObject Canvas;
    private Image Panel;
    
    //パネルのフェードイン
    public void PanelFade()
    {

        if (Panel == null)
        {
            Canvas = Resources.Load<GameObject>("Canvas");
            GameObject canvas = Instantiate(Canvas);
            Panel = canvas.GetComponentInChildren<Image>();
        }
        float alpha = Panel.color.a + 0.015f;
        Panel.color = new Color(Panel.color.r, Panel.color.b, Panel.color.g, alpha);
    }
}
