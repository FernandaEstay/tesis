using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCollision : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    void OnTriggerEnter(Collider other) {
        if (Input.GetMouseButtonUp(1) == true)
        {
            Debug.Log("imagen guardada");
        }
    }
    void OnTriggerStay(Collider other) {
        if (OnMouseUp())
        {

            Debug.Log("ima");
            Renderer renderer = other.transform.GetComponent<Renderer>();
            renderer.enabled = false;
            //other.enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan);
        RaycastHit hitExit;

        Debug.Log("sale de la foto");
        if (Physics.Raycast(ray, out hitExit) == true)
        {
            Renderer rend = hitExit.transform.GetComponent<Renderer>();
            //rend.material.shader = Shader.Find("Transparent/Diffuse");
            rend.material.color = Color.green;
            //Color tempColor = rend.material.color;
            //tempColor.a = 0.3F;
            //rend.material.color = tempColor;

            /*Material material = nuevo Material(Shader.Find("Transparent / Diffuse"));
            Material.color = Color.green;
            GetComponent<Renderer>().Material = material;
            /*
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            hit.collider.gameObject.GetComponent < Material > = Color.red;
            */
        }
    }
    private bool OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
            return true;
        else
            return false;
    }
    private void OnMouseExit()
    {

        
    }
    private void OnMouseOver()
    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) == true)
            {
                Renderer rend = hit.transform.GetComponent<Renderer>();
                //rend.material.shader = Shader.Find("Transparent/Diffuse");
                rend.material.color = Color.red;
                //Color tempColor = rend.material.color;
                //tempColor.a = 0.3F;
                //rend.material.color = tempColor;

                /*Material material = nuevo Material(Shader.Find("Transparent / Diffuse"));
                Material.color = Color.green;
                GetComponent<Renderer>().Material = material;
                /*
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                hit.collider.gameObject.GetComponent < Material > = Color.red;
                */
            }
        
    }
}
