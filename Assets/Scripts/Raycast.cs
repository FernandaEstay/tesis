using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {

    private Vector3 dist;
    private float posX;
    private float posY;
    private Vector3 posInicial;
    private Vector3 posInicialCam;

    // Use this for initialization
    void Start () {
        posInicial = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
            Zoom();
        }  
    }
    private void Awake()
    {
        posInicialCam = Camera.main.transform.position;
    }
    private void OnMouseDown()
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;
        Camera.main.transform.position = posInicialCam;

    }
    private void OnMouseUp()
    {
        transform.position = posInicial;
        Vector3 scalePhoto = new Vector3(10, 10, 1);
        transform.localScale = scalePhoto;
        Camera.main.transform.position = posInicialCam;
    }

    private void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
        Vector3 scalePhoto = new Vector3(5, 5, 1);
        transform.localScale = scalePhoto;
        Camera.main.transform.position = posInicialCam;
    }
    private void Zoom()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan);
        RaycastHit hitExit;

        if (Physics.Raycast(ray, out hitExit) == true)
        {
            Renderer rend = hitExit.transform.GetComponent<Renderer>();
            Camera.main.transform.position = new Vector3(hitExit.transform.position.x, hitExit.transform.position.y, -10f);
        }
    }
}





