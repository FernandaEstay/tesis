using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript;

public class ScrDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerStay(Collider other)
    {      
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) == true)
            {

                if(scrCollision.RemovePhotosSelected(scrCollision.GetNameContainer(), other.transform.name))
                {
                    other.transform.GetComponent<Renderer>().enabled = false;
                    scrCollision.UpdateTextContainer(scrCollision.GetNameContainer());
                    scrCollision.SetLocked(1);

                }

            }

        }
    }

 
    void OnTriggerEnter(Collider other)
    {
        Renderer photo = other.transform.GetComponent<Renderer>();
        photo.material.color = Color.red;
    }
    void OnTriggerExit(Collider other)
    {
        Renderer photo = other.transform.GetComponent<Renderer>();
        photo.material.color = Color.white;
    }
}
