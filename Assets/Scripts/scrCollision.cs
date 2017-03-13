using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scrCollision : MonoBehaviour
{
    private GameObject trash;
    private List<string> photosSelected;
    private GameObject[] photos;

    private static List<List<string>> listPhotos;
    private static int locked = 0;
    private static GameObject[] texts;
    public static GameObject containerDelete = null;
    private Color colorContainer;

    // Use this for initialization
    void Start()
    {
        //photosSelected = new List<string>();

        listPhotos = new List<List<string>>();

        //locker = true;
        texts = GameObject.FindGameObjectsWithTag("TextContainer");

        //inicializar listas de fotos
        foreach (GameObject text in texts)
        {
            List<string> photos = new List<string>();
            photos.Add(text.name);
            listPhotos.Add(photos);
        }

        //inicializar colores de container
        GameObject[] containers = GameObject.FindGameObjectsWithTag("container");
        ChangeColorContainer(containers[0], Color.cyan);
        ChangeColorContainer(containers[1], Color.green);
        ChangeColorContainer(containers[2], Color.magenta);
        ChangeColorContainer(containers[3], Color.yellow);



    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        UpdateAllTextContainer();
        if (Input.GetMouseButtonDown(0))
        { 
            if (Raycast.GetPhotoZoom() == null)
            {
                if (locked == 0)
                    Delete();
                else
                    reverseDelete();
            }
            else
            {
                SelectPhoto(Raycast.GetPhotoZoom());
            }
        }
     
    }

    public void SelectPhoto(GameObject photo)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {

            Renderer renderer = photo.transform.GetComponent<Renderer>();

            foreach (List<string> list in listPhotos)
            {
                if (list[0] == hit.transform.name.ToString())
                {
                    if (list.Contains(renderer.name) == false)
                    {
                        list.Add(renderer.name);

                        Raycast photoInstance = GameObject.Find(Raycast.GetPhotoZoom().transform.name).GetComponent<Raycast>();
                        photoInstance.CreateMarcadorPhoto(Raycast.GetPhotoZoom(), hit.transform.gameObject.GetComponent<Renderer>().material.color);

                        UpdateTextContainer(hit.transform.name.ToString());
                    }
                    break;
                }
            }
            /*colorContainer = transform.GetComponent<Renderer>().material.color;
            Debug.Log("luego del while" + colorContainer);
            StartCoroutine(MarcarContainer(hit.transform.gameObject));
            Debug.Log("luego del while" + colorContainer);
            hit.transform.GetComponent<Renderer>().material.color = colorContainer;*/
        }
    }

    IEnumerator MarcarContainer(GameObject cont)
    {
        int i = 0;
        cont.transform.GetComponent<Renderer>().material.color = Color.red;
        while (i < 100)
        {
            yield return null;
        }
        
    }
    private void reverseDelete()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {
            if (hit.transform.gameObject == containerDelete)
            {
                if (locked == 1)
                {

                    containerDelete = null;
                    //hit.transform.GetComponent<Renderer>().material.color = Color.white;
                    /*
                    trash = GameObject.FindGameObjectWithTag("Trash");
                    trash.transform.GetComponent<Renderer>().enabled = false;*/

                    photos = GameObject.FindGameObjectsWithTag("photo");
                    foreach (GameObject photo in photos)
                    {
                        Renderer rendererPhoto = photo.transform.GetComponent<Renderer>();
                        rendererPhoto.enabled = true;
                        int child = photo.transform.childCount;
                        for(int i = 0; i<child; i++)
                        {
                            if(photo.transform.GetChild(i).transform.GetComponent<Renderer>().enabled == false)
                            {
                                photo.transform.GetChild(i).transform.GetComponent<Renderer>().enabled = true;
                            }
                        }
                    }

                    locked = 0;
                }
            }
            
        }
    }
    void Delete()
    {
        if (locked == 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) == true)
            {
                Renderer rendContainer = hit.transform.GetComponent<Renderer>();
                //rendContainer.material.color = Color.cyan;

                photos = GameObject.FindGameObjectsWithTag("photo");

                /*trash = GameObject.FindGameObjectWithTag("Trash");
                trash.transform.GetComponent<Renderer>().enabled = true;*/

                List<string> photosSelected = null;
                foreach (List<string> list in listPhotos)
                {
                    if (list[0] == rendContainer.name)
                    {
                        photosSelected = list;
                    }
                }
                photos = GameObject.FindGameObjectsWithTag("photo");
                for (int i = 0; i < photos.Length; i++)
                {
                    Renderer rendererPhoto = photos[i].transform.GetComponent<Renderer>();
                    if (photosSelected.Contains(rendererPhoto.name) == true)
                    {
                        rendererPhoto.enabled = true;
                    }
                    else
                    {
                        rendererPhoto.enabled = false;
                        int child = photos[i].transform.childCount;
                        for(int y = 0; y<child; y++)
                        {
                            photos[i].transform.GetChild(y).transform.GetComponent<Renderer>().enabled = false;
                        }
                    }
                }
                containerDelete = hit.transform.gameObject;

            }
            UpdateAllTextContainer();
            locked = 1;
        }
    }

    public static bool RemovePhotosSelected(string nameContainer, string namePhoto)
    {
        foreach (List<string> list in listPhotos)
        {
            if (list[0] == nameContainer)
            {
                list.Remove(namePhoto);
                return true;
            }
        }
        return false;
    }

    public static List<string> PhotosContainer(string nameContainer)
    {
        foreach (List<string> list in listPhotos)
        {
            if (list[0] == nameContainer)
            {
                return list;
            }
        }
        return null;
    }
    public static GameObject GetContainerDelete()
    {
        return containerDelete;
    }
    public static void UpdateTextContainer(string nameContainer)
    {
        foreach (List<string> list in listPhotos)
        {
            if (list[0] == nameContainer)
            {
                foreach (GameObject tc in texts)
                {
                    if (tc.name.ToString() == nameContainer)
                    {
                        Text text = tc.GetComponent<Text>();
                        text.text = text.name + " :" + (list.Count - 1);
                        text.enabled = true;
                        break;
                    }
                   
                }
                
                break;
            }
        }
    }

    public void UpdateAllTextContainer()
    {
        foreach (List<string> list in listPhotos)
        {
             foreach (GameObject tc in texts)
             {
                    if (tc.name.ToString() == list[0])
                    {
                        Text text = tc.GetComponent<Text>();
                        text.text = text.name + " :" + (list.Count - 1);
                        text.enabled = true;
                        break;
                    }
             }

        }
    }


    public void ChangeColorContainer(GameObject cont, Color color)
    {
        Renderer rend = cont.transform.GetComponent<Renderer>();
        rend.material.color = color;
    }

    public static void SetLocked(int value)
    {
        locked = value;
    }
}

/*
void OnTriggerStay(Collider other)
{
    if (Input.GetMouseButtonUp(0))
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {

            Renderer renderer = other.transform.GetComponent<Renderer>();


            foreach (List<string> list in listPhotos)
            {
                if (list[0] == hit.transform.name.ToString())
                {
                    if (list.Contains(renderer.name) == false)
                    {
                        list.Add(renderer.name);

                        UpdateTextContainer(hit.transform.name.ToString());
                    }
                    break;
                }
            }

        }

    }
}
void OnTriggerEnter(Collider other)
{
    Renderer rend = transform.GetComponent<Renderer>();
    rend.material.color = Color.red;
}
void OnTriggerExit(Collider other)
{
    Renderer rend = transform.GetComponent<Renderer>();
    rend.material.color = Color.white;
}
*/
