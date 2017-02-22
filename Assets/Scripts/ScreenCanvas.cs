using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCanvas : MonoBehaviour {

    public Text txtTime;
    private float time;
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
        txtTime.text = "Tiempo: " + min.ToString() + ":" + Mathf.RoundToInt(seg).ToString();
    }


}
