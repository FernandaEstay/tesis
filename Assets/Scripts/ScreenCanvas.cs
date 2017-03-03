using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCanvas : MonoBehaviour {

    public Text txtTime;
    private float time;
    private string text;
    float min;
    float seg;

    // Use this for initialization
    void Start () {
        time = 0f;
    }

    // Update is called once per frame
    
    void Update()
    {
        time += Time.deltaTime;
        min = Mathf.Floor(time / 60);
        seg = time % 60;
        if (min.ToString().Length == 1)
        {
            text = "Tiempo: 0" + min.ToString();
        }
        else
        {
            text = "Tiempo: " + min.ToString();
        }
        if (Mathf.RoundToInt(seg).ToString().Length == 1)
        {
            text = text + ":0" + Mathf.RoundToInt(seg).ToString();
        }
        else
        {
            text = text + ":" + Mathf.RoundToInt(seg).ToString();
        }
        txtTime.text = text;
    }


}
