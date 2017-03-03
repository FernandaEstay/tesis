using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scrCamera : MonoBehaviour {

    public Button btNext;
    public Button btBack;

    private Camera mainCamera;
    private Camera zoomCamera;
    private Vector3 posInicialCam;
    private Vector3 posfinalCam;
    private Vector3 pos;
    private GameObject[] containers;
    private GameObject[] textContainers;
    private GameObject trash;

    // Use this for initialization
    void Start() {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        zoomCamera = GameObject.Find("Zoom Camera").GetComponent<Camera>();
        trash = GameObject.FindGameObjectWithTag("Trash");

        posInicialCam = mainCamera.transform.position;
        posfinalCam.Set(posInicialCam.x + (156f * 3f), posInicialCam.y, posInicialCam.z);


    }
	
	// Update is called once per frame
	void Update () {
        if (mainCamera.enabled == true)
        {
            if (posInicialCam == mainCamera.transform.position)
            {
                btBack.gameObject.SetActive(false);
                btNext.gameObject.SetActive(true);
            }
            else if (posfinalCam == mainCamera.transform.position)
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

        trash.transform.GetComponent<Renderer>().enabled = true;
        Vector3 posTrash = new Vector3(trash.transform.position.x + 156f, trash.transform.position.y, trash.transform.position.z);
        trash.transform.position = posTrash;
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

        Vector3 posTrash = new Vector3(trash.transform.position.x - 156f, trash.transform.position.y, trash.transform.position.z);
        trash.transform.position = posTrash;
    }
    private void Zoom()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true && hit.transform.tag == "photo")
        {
            mainCamera.enabled = false;
            zoomCamera.enabled = true;
            zoomCamera.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -10f);
        }
    }
}
