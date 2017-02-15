using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadImage : MonoBehaviour {

    public int numPhotos;
    private GameObject[] quads;
  
    // Use this for initialization
    void Start ()
    {
       //obtiene todos los quads de la scena
        quads = GameObject.FindGameObjectsWithTag("photo");
        LoadFolderImages();

    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    private void LoadFolderImages()
    {
        string pathPrefix = @"file://";
        string pathImageAssets = @"C:\";
        string pathSmall = @"img\";
        string filename = @"photo";
        string fileSuffix = @".jpg";

        string indexSuffix = "";

        int i = 1;
        foreach(GameObject quad in quads) { 
            if (i < 10)
                indexSuffix = "00" + i.ToString();
            else if (i < 100)
                indexSuffix = "0" + i.ToString();
            else
                indexSuffix = i.ToString();

            Debug.Log(indexSuffix);

            string fullFilename = pathPrefix + pathImageAssets + pathSmall + filename + indexSuffix + fileSuffix;
       
            WWW www = new WWW(fullFilename);

            Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texTmp);

            quad.GetComponent<MeshRenderer>().material.mainTexture = texTmp;

            if (i > numPhotos)
                break;
            i += 1;
        }

    }
}
