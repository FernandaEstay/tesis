using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrCollision : MonoBehaviour
{
    private GameObject[] texts;
    public int cont = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if(Input.GetMouseButtonUp(0)){
            Renderer renderer = other.transform.GetComponent<Renderer>();
            renderer.enabled = false;
            texts = GameObject.FindGameObjectsWithTag("TextContainer");
            foreach (GameObject tc in texts)
            {
                if(tc.name.ToString() == transform.name.ToString())
                {
                    cont += 1;
                    Text text = tc.GetComponent<Text>();
                    text.text = text.name + " :" + cont;
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Renderer rend = transform.GetComponent<Renderer>();
        rend.material.color = Color.red;
    }
    void OnTriggerExit(Collider other)
    {
        Renderer rend = transform.GetComponent<Renderer>();
        rend.material.color = Color.white;
    }
}
