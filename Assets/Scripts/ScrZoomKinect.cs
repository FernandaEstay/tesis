using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;


public class ScrZoomKinect : MonoBehaviour {

    private KinectSensor kinectSensor;
    private Body[] bodies;
    public GameObject BodySrcManager;
    private BodySourceManager bodyManager;

    private GameObject photo;
    private GameObject marcador;

    private bool zoomIn = true;
    private bool zoomOut = false;
    private Vector3 posMax;
    private Vector3 posInicial;

    private static bool ZoomStart = false;

    // Use this for initialization
    void Start () {

        if (BodySrcManager == null)
        {
            Debug.Log("Falta asignar Game Object as BodySrcManager");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }

        posMax = new Vector3(0f, 0f, -45f);
    }

    // Update is called once per frame
    void Update()
    {
        if(bodyManager == null)
        {
            return;
        }
        bodies = bodyManager.GetData();
        if(bodies == null)
        {
            return;
        }
        foreach( var body in bodies)
        {
            if(body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                if(ScrFaceKinect.GetPhotoSelected() != null && ScrFaceKinect.GetMarcador() != null)
                {
                    photo = ScrFaceKinect.GetPhotoSelected();
                    marcador = ScrFaceKinect.GetMarcador();
                    switch (body.HandRightState)
                    {
                        case HandState.Open:
                            if(zoomIn == true)
                            {
                                posInicial = photo.transform.position;
                                ZoomStart = true;
                                StartCoroutine(ZoomIn(photo, marcador));
                            }
                            else
                            {
                                return;
                            }
                            break;
                        case HandState.Closed:
                            if (zoomOut == true)
                            {
                                StartCoroutine(ZoomOut(photo, posInicial, marcador));
                                ZoomStart = false;
                            }
                            else
                            {
                                return;
                            }
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }
        
    }

    IEnumerator ZoomIn(GameObject photo, GameObject marcador)
    {
        zoomIn = false;
        ObjectChangeColor(photo.transform.gameObject, Color.white);
        marcador.SetActive(false);
        while (photo.transform.position.z > posMax.z)
        {
            Vector3 posCam = Camera.main.transform.position;
            posCam.Set(posCam.x, posCam.y, posCam.z + 15f);
            float step = 5f;
            photo.transform.position = Vector3.MoveTowards(photo.transform.position, posCam, step);
            yield return null;
        }
        PhotosChangeColor(photo.transform.gameObject, Color.grey);
        zoomOut = true;
    }

    IEnumerator ZoomOut(GameObject photo, Vector3 posInicial, GameObject marcador)
    {
        zoomOut = false;
        marcador.SetActive(true);
        while (photo.transform.position.z < posInicial.z)
        {
            float step = 5f;
            photo.transform.position = Vector3.MoveTowards(photo.transform.position, posInicial, step);
            yield return null;
        }
        PhotosChangeColor(photo.transform.gameObject, Color.white);
        photo = null;
        zoomIn = true;
    }

    private void PhotosChangeColor(GameObject except, Color color)
    {
        GameObject[] photos = GameObject.FindGameObjectsWithTag("photo");

        foreach (GameObject photo in photos)
        {
            if (photo.name != except.name)
            {
                ObjectChangeColor(photo, color);
            }
        }
    }

    private void ObjectChangeColor(GameObject obj, Color color)
    {
        Renderer rend = obj.transform.GetComponent<Renderer>();
        rend.material.color = color;
    }
    public static bool GetZoomStart()
    {
        return ZoomStart;
    }
}
