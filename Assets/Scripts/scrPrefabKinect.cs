using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrPrefabKinect : MonoBehaviour
{

    public GameObject prefab;
    public float gridX = 4f;
    public float gridY = 3f;
    public int numberPage = 1;
    public float spacing = 13f;
    public Text[] textContainer;
    private float separationPages = 0.5f;

    public GameObject container;
    float distX = -20f;
    float distY = 7f;

    void Start()
    {
        //carga de quads para imagenes
        for (int p = 0; p < numberPage; p++)
        {
            for (int y = 0; y < gridY; y++)
            {
                for (int x = 0; x < gridX; x++)
                {
                    Vector3 pos = new Vector3(x + separationPages, y, 0) * spacing;
                    Instantiate(prefab, pos, Quaternion.identity);

                }
            }
            separationPages += 12f;
        }

        //carga de objetos contenedores
        for (int y = 0; y < 4; y++)
        {
            Vector3 pos = new Vector3(distX, distY, 0);
            distY += 12f;
            Instantiate(container, pos, Quaternion.identity);
        }

        int i = 0;
        GameObject[] containers;
        containers = GameObject.FindGameObjectsWithTag("container");
        foreach (Text text in textContainer)
        {
            containers[i].name = text.text;
            text.name = text.text;
            i += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}