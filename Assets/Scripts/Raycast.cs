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
    public Vector3 posInicial;
    private Vector3 offset;
    private Vector3 posMax;
    public static GameObject photoSelected = null;

    public GameObject prefabMarcador;


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
    public void Zoom()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {
            if (photoSelected == null)
            {
                StartCoroutine(ZoomIn(hit));
            }
            else if(photoSelected.name == hit.transform.name)
            {
                StartCoroutine(ZoomOut(hit, posInicial));
            }
        }
    }
    IEnumerator ZoomIn(RaycastHit photo)
    {
        photoSelected = photo.transform.gameObject;
        while (photo.transform.position.z > posMax.z)
        {
            Vector3 posCam = Camera.main.transform.position;
            posCam.Set(posCam.x, posCam.y, posCam.z + 15f);
            float step = 5f;
            photo.transform.position = Vector3.MoveTowards(photo.transform.position, posCam, step);
            yield return null;
        }
        PhotosChangeColor(photo.transform.gameObject, Color.grey);
    }

    IEnumerator ZoomOut(RaycastHit photo, Vector3 posInicial)
    {
        while (photo.transform.position.z < posInicial.z)
        {
            float step = 5f;
            photo.transform.position = Vector3.MoveTowards(photo.transform.position, posInicial, step);
            yield return null;
        }
        PhotosChangeColor(photo.transform.gameObject, Color.white);
        photoSelected = null;
    }
    private void ObjectChangeColor(GameObject obj, Color color)
    {
        Renderer rend = obj.transform.GetComponent<Renderer>();
        rend.material.color = color;
    }

    private void PhotosChangeColor(GameObject except, Color color)
    {
        GameObject[] photos = GameObject.FindGameObjectsWithTag("photo");

        foreach(GameObject photo in photos)
        {
            if(photo.name != except.name)
            {
                ObjectChangeColor(photo, color);
            }
        }
    }
    

    public void CreateMarcadorPhoto(GameObject photo, Color color)
    {


        GameObject obj = Instantiate(prefabMarcador) as GameObject;
        obj.transform.parent = photo.transform;

        int i = photo.transform.childCount;

        photo.transform.GetChild(i - 1).transform.position = new Vector3(photo.transform.position.x - 3.75f + ((i-1) * 2.5f), photo.transform.position.y - 4, -45f);
        obj.transform.GetComponent<Renderer>().material.color = color;

    }
    public static GameObject GetPhotoZoom()
    {
        return photoSelected;
    }

    public static void SetDeletePhotoSelected()
    {
        photoSelected = null;
    }
    public void SetPositionPhoto(GameObject photo)
    {
        photo.transform.position = posInicial;
        PhotosChangeColor(photo, Color.white);
    }
    /*public static void SetPositionPhotoDelete(Vector3 posInicial)
    {
        photoSelected.transform.position = posInicial;
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





