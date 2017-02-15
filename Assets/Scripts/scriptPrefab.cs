using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptPrefab : MonoBehaviour {

    public GameObject prefab;
    public float gridX = 8f;
    public float gridY = 5f;
    public int numberPage = 4; 
    public float spacing = 13f;
    private int separationPages = 0;

    void Start()
    {
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
    }

    // Update is called once per frame
    void Update () {
		
	}
}
