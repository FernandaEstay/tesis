using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCanvas : MonoBehaviour {

    public Text txtTime;
    private float time;


    // Use this for initialization
    void Start () {
        time = 0f;
    }

    // Update is called once per frame
    
    void Update()
    {
        time += Time.deltaTime;
        txtTime.text = "Tiempo: " + time.ToString("F1");
    }


}
