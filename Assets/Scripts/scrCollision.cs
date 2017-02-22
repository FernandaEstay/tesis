using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrCollision : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnMouseExit()
    {
        Renderer rend = transform.GetComponent<Renderer>();
        rend.material.color = Color.white;
    }
    private void OnMouseUp()
    {
        Renderer rend = transform.GetComponent<Renderer>();
        rend.material.color = Color.white;
    }
    private void OnMouseDown()
    {
        Renderer rend = transform.GetComponent<Renderer>();
        rend.material.color = Color.white;
    }
    void OnTriggerStay(Collider other)
    {
        if(Input.GetMouseButtonUp(0)){
            Renderer renderer = other.transform.GetComponent<Renderer>();
            renderer.enabled = false;
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
