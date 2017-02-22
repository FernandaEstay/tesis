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
    /*
    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedMouse = 10f;

    private static readonly float[] BoundsX = new float[] { 0f, 200f };
    private static readonly float[] BoundsZ = new float[] { -18f, -4f };
    private static readonly float[] ZoomBounds = new float[] { 0f, 200f };

    private Camera cam;

    private Vector3 lastPanPosition;
    */
    // Use this for initialization
    void Start () {
        posInicial = Camera.main.transform.position;
        posfinal.Set(posInicial.x + (156f * 3f), posInicial.y, posInicial.z);
    }
	
	// Update is called once per frame
	void Update () {
        //HandleMouse();
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
    }/*
    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }
    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }*/
}
