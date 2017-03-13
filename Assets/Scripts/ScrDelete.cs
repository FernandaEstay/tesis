using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript;

public class ScrDelete : MonoBehaviour {


    public static bool deleteModeActive = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) == true && Raycast.GetPhotoZoom() != null)
            {

                if(scrCollision.RemovePhotosSelected(scrCollision.GetContainerDelete().name, Raycast.GetPhotoZoom().transform.name))
                {
                    GameObject photoZoom = Raycast.GetPhotoZoom();
                    Raycast photoInstance = GameObject.Find(photoZoom.transform.name).GetComponent<Raycast>();

                    GameObject containerDelete = scrCollision.GetContainerDelete();

                    int child = photoZoom.transform.childCount;
                    int index = 0;
                    for(int i = 0; i < child; i++)
                    {
                        if (photoZoom.transform.GetChild(i).transform.GetComponent<Renderer>().material.color == containerDelete.transform.GetComponent<Renderer>().material.color)
                        {
                            Destroy(photoZoom.transform.GetChild(i).gameObject);
                        }
                        else
                        {
                            photoZoom.transform.GetChild(i).transform.position = new Vector3(photoZoom.transform.position.x - 3.75f + (index * 2.5f), photoZoom.transform.position.y - 4, -45f);
                            photoInstance.transform.GetChild(i).transform.GetComponent<Renderer>().enabled = false;
                            index += 1;
                        }
                    }


                    photoInstance.SetPositionPhoto(photoInstance.transform.gameObject);
                    
                    photoZoom.transform.GetComponent<Renderer>().enabled = false;
                    //DeleteMarcador(containerDelete, photoZoom);


                    Raycast.SetDeletePhotoSelected();

                    scrCollision.UpdateTextContainer(containerDelete.name);
                    AclararPhotosContainer(scrCollision.PhotosContainer(containerDelete.name));
                    scrCollision.SetLocked(1);

                }

            }

        }
    }

    /*
    public static void DeleteMarcador(GameObject photo, GameObject container)
    {
        int child = photo.transform.childCount;
        Debug.Log("numero de hijos " + child);
        Debug.Log("Delete marcador" + container.transform.GetComponent<Renderer>().material.color.ToString());
        for (int i = 0; i < child; i++)
        {
            Debug.Log("color child" + photo.transform.GetChild(i).transform.GetComponent<Renderer>().material.color.ToString());
            
        }
        child = photo.transform.childCount;
        for (int i = 0; i < child; i++)
        {
            photo.transform.GetChild(i).transform.position = new Vector3(photo.transform.position.x - 3.75f + (i * 2.5f), photo.transform.position.y - 4, -45f);
        }
    }
    */
    void AclararPhotosContainer(List<string> photosContainer)
    {
        GameObject[] photos = GameObject.FindGameObjectsWithTag("photo");
        for (int i = 0; i < photos.Length; i++)
        {
            Renderer rendererPhoto = photos[i].transform.GetComponent<Renderer>();
            if (rendererPhoto.enabled)
            {
                rendererPhoto.material.color = Color.white;
            }
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        Renderer photo = other.transform.GetComponent<Renderer>();
        photo.material.color = Color.red;
    }
    void OnTriggerExit(Collider other)
    {
        Renderer photo = other.transform.GetComponent<Renderer>();
        photo.material.color = Color.white;
    }*/
}
