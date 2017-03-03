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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        UpdateAllTextContainer();
        if (Input.GetMouseButtonDown(1))
            Delete();
        if (Input.GetMouseButtonDown(0))
            reverseDelete();
    }
    private void reverseDelete()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {
            if (hit.transform.GetComponent<Renderer>().material.color == Color.cyan)
            {
                if (locked == 1)
                {

                    hit.transform.GetComponent<Renderer>().material.color = Color.white;
                    /*
                    trash = GameObject.FindGameObjectWithTag("Trash");
                    trash.transform.GetComponent<Renderer>().enabled = false;*/

                    photos = GameObject.FindGameObjectsWithTag("photo");
                    foreach (GameObject photo in photos)
                    {
                        Renderer rendererPhoto = photo.transform.GetComponent<Renderer>();
                        rendererPhoto.enabled = true;
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
                rendContainer.material.color = Color.cyan;

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
                foreach (List<string> list in listPhotos)
                {

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
                    }
                }

            }
            UpdateAllTextContainer();
            locked = 1;
        }
    }

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

    public static string GetNameContainer()
    {
        GameObject[] containers = GameObject.FindGameObjectsWithTag("container");
        foreach (GameObject cont in containers)
        {
            if(cont.transform.GetComponent<Renderer>().material.color == Color.cyan)
            {
                return cont.transform.name;
            }
        }
        return null;
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

    public static void SetLocked(int value)
    {
        locked = value;
    }
}
