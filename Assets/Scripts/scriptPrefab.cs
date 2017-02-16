using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptPrefab : MonoBehaviour
{

    public GameObject prefab;
    public float gridX = 8f;
    public float gridY = 5f;
    public int numberPage = 4;
    public float spacing = 13f;
    private int separationPages = 0;

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
            separationPages += 12;
        }

        //carga de objetos contenedores
        for (int y = 0; y < 4; y++)
        {
            Vector3 pos = new Vector3(distX, distY, 0);
            distY += 12f;
            Instantiate(container, pos, Quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
