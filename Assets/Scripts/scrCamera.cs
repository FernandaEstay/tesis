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
    private Vector3 velocityNext = new Vector3(6f, 0f, 0f); //posicion x debe ser divisor de 156f
    private Vector3 velocityBack = new Vector3(-6f, 0f, 0f); //posicion x debe ser divisor de 156f

    // Use this for initialization
    void Start() {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //zoomCamera = GameObject.Find("Zoom Camera").GetComponent<Camera>();
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

        /*if (Input.GetMouseButtonDown(1))
        {
            EnableTextContainer(false);
            Zoom();
        }
        if (Input.GetMouseButtonDown(0))
        {
            zoomCamera.enabled = false;
            mainCamera.enabled = true;
            EnableTextContainer(true);

        }*/
    }

    public void NextOnClicEvent()
    {
        MoveContainer(156f);
        StartCoroutine(MoveCameraNext(mainCamera, 156f));
        moveObjectX(trash, 156f);
    }

    public void BackOnClicEvent()
    {
        MoveContainer(-156f);   
        StartCoroutine(MoveCameraBack(mainCamera, -156f));
        moveObjectX(trash, -156f);
    }

    private void MoveContainer(float x)
    {
        containers = GameObject.FindGameObjectsWithTag("container");
        foreach (GameObject cont in containers)
        {
            moveObjectX(cont, x);
        }
    }
    private void moveObjectX(GameObject obj, float value)
    {
        Vector3 position = new Vector3(obj.transform.position.x + value, obj.transform.position.y, obj.transform.position.z);
        obj.transform.position = position;
    }


    IEnumerator MoveCameraNext(Camera cam, float value)
    {
        EnableTextContainer(false);
        Vector3 posFinal = cam.transform.position;
        posFinal.Set(posFinal.x + value, posFinal.y, posFinal.z);
        while(cam.transform.position.x < posFinal.x - 1f)
        {
            cam.transform.Translate(velocityNext.x , 0, 0);
            yield return null;
        }
        EnableTextContainer(true);

    }

    IEnumerator MoveCameraBack(Camera cam, float value)
    {
        EnableTextContainer(false);
        Vector3 posFinal = cam.transform.position;
        posFinal.Set(posFinal.x + value, posFinal.y, posFinal.z);
        while (cam.transform.position.x - 1f > posFinal.x)
        {
            cam.transform.Translate(velocityBack.x, 0, 0);
            yield return null;
        }
        EnableTextContainer(true);

    }

    private void EnableTextContainer(bool value)
    {
        textContainers = GameObject.FindGameObjectsWithTag("TextContainer");
        foreach (GameObject text in textContainers)
        {
            Text tx = text.GetComponent<Text>();
            tx.enabled = value;

        }
    } 

    /*private void Zoom()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true && hit.transform.tag == "photo")
        {
            mainCamera.enabled = false;
            zoomCamera.enabled = true;
            zoomCamera.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, -10f);
        }
    }*/
}
