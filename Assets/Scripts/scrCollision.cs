using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrCollision : MonoBehaviour
{
    private GameObject[] texts;
    private List<string> listPhotos;
    private List<string> listPhotosSelected;
    private GameObject trash;
    private GameObject[] photos;


    // Use this for initialization
    void Start()
    {
        listPhotos = new List<string>();
        listPhotosSelected = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (listPhotos.Count != 0)
        {
            if (Input.GetMouseButtonDown(1) )
                Delete();
            if(Input.GetMouseButtonDown(0) )
                reverseDelete();
        }

    }

    private void reverseDelete()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {
            if(hit.transform.GetComponent<Renderer>().material.color == Color.cyan)
            {
                hit.transform.GetComponent<Renderer>().material.color = Color.white;
                trash = GameObject.FindGameObjectWithTag("Trash");
                trash.transform.GetComponent<Renderer>().enabled = false;

                photos = GameObject.FindGameObjectsWithTag("photo");
                foreach (GameObject photo in photos)
                {
                    Renderer rendererPhoto = photo.transform.GetComponent<Renderer>();
                    if (listPhotosSelected.Contains(rendererPhoto.name))
                    {
                        rendererPhoto.enabled = false;
                    }
                    else if (listPhotos.Contains(rendererPhoto.name))
                    {
                        if(rendererPhoto.enabled == true)
                            rendererPhoto.enabled = false;
                        else
                            listPhotos.Remove(rendererPhoto.name);
                    }
                    else
                        rendererPhoto.enabled = true;
                }

            }
        }
    }
    void Delete()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == true)
        {
            Renderer rendContainer = hit.transform.GetComponent<Renderer>();
            rendContainer.material.color = Color.cyan;

            trash = GameObject.FindGameObjectWithTag("Trash");
            trash.transform.GetComponent<Renderer>().enabled = true;

            photos = GameObject.FindGameObjectsWithTag("photo");
            foreach (GameObject photo in photos)
            {
                Renderer rendererPhoto = photo.transform.GetComponent<Renderer>();
                if (listPhotos.Contains(photo.name))
                {
                    rendererPhoto.enabled = true;
                }
                else if(rendererPhoto.enabled == false)
                {
                    listPhotosSelected.Add(rendererPhoto.name);
                }
                else
                {
                    rendererPhoto.enabled = false;
                }
            }

        }
    }

    void OnTriggerStay(Collider other)
    {
        if(Input.GetMouseButtonUp(0)){
            Renderer renderer = other.transform.GetComponent<Renderer>();
            renderer.enabled = false;
            if (!listPhotos.Contains(renderer.name))
            {
                listPhotos.Add(renderer.name);

                texts = GameObject.FindGameObjectsWithTag("TextContainer");

                foreach (GameObject tc in texts)
                {
                    if (tc.name.ToString() == transform.name.ToString())
                    {
                        Text text = tc.GetComponent<Text>();
                        text.text = text.name + " :" + listPhotos.Count;
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
}
