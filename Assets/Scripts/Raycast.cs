using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {

    private Vector3 dist;
    private float posX;
    private float posY;
    private Vector3 posInicial;
    private Vector3 posInicialCam;
    private Camera mainCamera;
    // Use this for initialization
    void Start ()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        posInicial = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
    private void OnMouseDown()
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

    private void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
        Vector3 scalePhoto = new Vector3(5, 5, 1);
        transform.localScale = scalePhoto;
    }
}





