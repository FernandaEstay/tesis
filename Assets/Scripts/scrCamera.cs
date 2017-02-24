using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scrCamera : MonoBehaviour {

    public Button btNext;
    public Button btBack;

    private Camera mainCamera;
    private Camera zoomCamera;
    private Vector3 posInicial;
    private Vector3 posfinal;
    private Vector3 pos;
    private GameObject[] containers;
    private GameObject[] textContainers;

    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        zoomCamera = GameObject.Find("Zoom Camera").GetComponent<Camera>();

        posInicial = mainCamera.transform.position;
        posfinal.Set(posInicial.x + (156f * 3f), posInicial.y, posInicial.z);

    }
	
	// Update is called once per frame
	void Update () {
        if (mainCamera.enabled == true)
        {
            if (posInicial == mainCamera.transform.position)
            {
                btBack.gameObject.SetActive(false);
                btNext.gameObject.SetActive(true);
            }
            else if (posfinal == mainCamera.transform.position)
            {
                btNext.gameObject.SetActive(false);
                btBack.gameObject.SetActive(true);
            }
            else
            {
                btNext.gameObject.SetActive(true);
                btBack.gameObject.SetActive(true);
            }
        }
        else
        {
            btBack.gameObject.SetActive(false);
            btNext.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            textContainers = GameObject.FindGameObjectsWithTag("TextContainer");
            foreach (GameObject text in textContainers)
            {
                Text tx = text.GetComponent<Text>();
                tx.enabled = false;

            }
            Zoom();
        }
        if (Input.GetMouseButtonDown(0))
        {
            zoomCamera.enabled = false;
            mainCamera.enabled = true;
            textContainers = GameObject.FindGameObjectsWithTag("TextContainer");
            foreach (GameObject text in textContainers)
            {
                Text tx = text.GetComponent<Text>();
                tx.enabled = true;
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
        pos = mainCamera.transform.position;
        pos.Set(pos.x + 156f, pos.y, pos.z);
        mainCamera.transform.position = pos;
    }
    public void BackOnClicEvent()
    {
        containers = GameObject.FindGameObjectsWithTag("container");
        foreach (GameObject cont in containers)
        {
            Vector3 contPos = new Vector3(cont.transform.position.x - 156f, cont.transform.position.y, cont.transform.position.z);
            cont.transform.position = contPos;
        }
        pos = mainCamera.transform.position;
        pos.Set(pos.x - 156f, pos.y, pos.z);
        mainCamera.transform.position = pos;
    }
    private void Zoom()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {
            mainCamera.enabled = false;
            zoomCamera.enabled = true;
            zoomCamera.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -10f);
        }
    }
}
