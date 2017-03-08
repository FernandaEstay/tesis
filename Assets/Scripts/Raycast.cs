using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {
    /*
    private Vector3 dist;
    private float posX;
    private float posY;
    private Vector3 posInicial;
    private Vector3 posInicialCam;
    private Camera mainCamera;
    */
    private Vector3 posInicial;
    private Vector3 offset;
    private Vector3 posMax;
    public static int lockPhoto = 0;

    // Use this for initialization
    void Start ()
    {
        posMax = new Vector3(0f, 0f, -45f);
        posInicial = transform.position;
        /*mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        posInicial = transform.position;*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Zoom();
        }

    }
    private void Zoom()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {
            if (lockPhoto == 0)
            {
                Debug.Log(lockPhoto);
                lockPhoto = 1;
                Debug.Log(lockPhoto);
                StartCoroutine(ZoomIn(hit));
            }
            else if(lockPhoto == 1)
            {
                Debug.Log(lockPhoto);
                lockPhoto = 0;
                StartCoroutine(ZoomOut(hit, posInicial));
                Debug.Log(lockPhoto);
            }
        }
    }
    IEnumerator ZoomIn(RaycastHit photo)
    {
        Debug.Log(lockPhoto);
        while (photo.transform.position.z > posMax.z)
        {
            Vector3 posCam = Camera.main.transform.position;
            posCam.Set(posCam.x, posCam.y, posCam.z + 15f);
            float step = 5f;
            photo.transform.position = Vector3.MoveTowards(photo.transform.position, posCam, step);
            yield return null;
        }
        Debug.Log(lockPhoto);
    }

    IEnumerator ZoomOut(RaycastHit photo, Vector3 posInicial)
    {
        Debug.Log(posInicial);
        while (photo.transform.position.z < posInicial.z)
        {
            float step = 5f;
            photo.transform.position = Vector3.MoveTowards(photo.transform.position, posInicial, step);
            yield return null;
        }
        Debug.Log(lockPhoto);

    }
    /*private void OnMouseDown()
    {
        dist = mainCamera.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;

    }
    private void OnMouseUp()
    {
        transform.position = posInicial;
        Vector3 scalePhoto = new Vector3(10, 10, 1);
        transform.localScale = scalePhoto;
    }
    */
    /*private void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
        Vector3 scalePhoto = new Vector3(5, 5, 1);
        transform.localScale = scalePhoto;
    }*/
}





