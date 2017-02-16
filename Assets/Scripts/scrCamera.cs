using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scrCamera : MonoBehaviour {

    private Vector3 posInicial;
    private Vector3 posfinal;
    private Vector3 pos;
    private GameObject[] containers;
    public Button btNext;
    public Button btBack;


	// Use this for initialization
	void Start () {
        posInicial = Camera.main.transform.position;
        posfinal.Set(posInicial.x + (156f * 3f), posInicial.y, posInicial.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (posInicial == Camera.main.transform.position)
        {
            btBack.gameObject.SetActive(false);
        }
        else if(posfinal == Camera.main.transform.position)
        {
            btNext.gameObject.SetActive(false);
        }
        else
        {
            if (!btNext.IsActive())
            {
                btNext.gameObject.SetActive(true);
            }
            if (!btBack.IsActive())
            {
                btBack.gameObject.SetActive(true);
            }
        }
	}

    public void NextOnClicEvent()
    {
        containers = GameObject.FindGameObjectsWithTag("container");
        foreach(GameObject cont in containers)
        {
            Vector3 contPos = new Vector3(cont.transform.position.x + 156f, cont.transform.position.y, cont.transform.position.z);
            cont.transform.position = contPos;
        }
        pos = Camera.main.transform.position;
        pos.Set(pos.x + 156f, pos.y, pos.z);
        Camera.main.transform.position = pos;
    }
    public void BackOnClicEvent()
    {
        containers = GameObject.FindGameObjectsWithTag("container");
        foreach (GameObject cont in containers)
        {
            Vector3 contPos = new Vector3(cont.transform.position.x - 156f, cont.transform.position.y, cont.transform.position.z);
            cont.transform.position = contPos;
        }
        pos = Camera.main.transform.position;
        pos.Set(pos.x - 156f, pos.y, pos.z);
        Camera.main.transform.position = pos;
    }
}
