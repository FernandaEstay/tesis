using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using System.IO;
using System;

public class ScrFaceKinect : MonoBehaviour
{

    private KinectSensor kinectSensor;
    private int bodyCount;
    private Body[] bodies;
    private FaceFrameSource[] faceFrameSources;
    private FaceFrameReader[] faceFrameReaders;
    private BodySourceManager _BodyManager;
    private int updateFrame;

    private const double FaceRotationIncrementInDegrees = 1.0f;
    private float centerX = 45.5f;
    private float centerY = 31f;

    private float multX = -1.42f;
    private float multY = 2.6f;

    public GameObject BodySrcManager;
    private BodySourceManager bodyManager;
    //private LineRenderer lr;

    public GameObject marcador;


    void Start()
    {

        updateFrame = 0;

        // one sensor is currently supported
        kinectSensor = KinectSensor.GetDefault();

        // set the maximum number of bodies that would be tracked by Kinect
        bodyCount = kinectSensor.BodyFrameSource.BodyCount;

        // allocate storage to store body objects
        bodies = new Body[bodyCount];

        if (BodySrcManager == null)
        {
            Debug.Log("Falta asignar Game Object as BodySrcManager");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }

        // specify the required face frame results
        FaceFrameFeatures faceFrameFeatures =
            FaceFrameFeatures.BoundingBoxInColorSpace
                | FaceFrameFeatures.PointsInColorSpace
                | FaceFrameFeatures.BoundingBoxInInfraredSpace
                | FaceFrameFeatures.PointsInInfraredSpace
                | FaceFrameFeatures.RotationOrientation
                | FaceFrameFeatures.FaceEngagement
                | FaceFrameFeatures.Glasses
                | FaceFrameFeatures.Happy
                | FaceFrameFeatures.LeftEyeClosed
                | FaceFrameFeatures.RightEyeClosed
                | FaceFrameFeatures.LookingAway
                | FaceFrameFeatures.MouthMoved
                | FaceFrameFeatures.MouthOpen;

        // create a face frame source + reader to track each face in the FOV
        faceFrameSources = new FaceFrameSource[bodyCount];
        faceFrameReaders = new FaceFrameReader[bodyCount];
        for (int i = 0; i < bodyCount; i++)
        {
            // create the face frame source with the required face frame features and an initial tracking Id of 0
            faceFrameSources[i] = FaceFrameSource.Create(kinectSensor, 0, faceFrameFeatures);

            // open the corresponding reader
            faceFrameReaders[i] = faceFrameSources[i].OpenReader();
        }

        marcador.transform.GetComponent<Renderer>().material.color = UnityEngine.Color.red;

    }

    private void FixedUpdate()
    {
        if (updateFrame < 1)
        {
            updateFrame++;
            return;
        }
        updateFrame = 0;
        // get bodies either from BodySourceManager object get them from a BodyReader
        var bodySourceManager = bodyManager.GetComponent<BodySourceManager>();
        bodies = bodySourceManager.GetData();
        if (bodies == null)
        {
            return;
        }


        // iterate through each body and update face source
        for (int i = 0; i < bodyCount; i++)
        {
            // check if a valid face is tracked in this face source				
            if (faceFrameSources[i].IsTrackingIdValid)
            {
                using (FaceFrame frame = faceFrameReaders[i].AcquireLatestFrame())
                {
                    if (frame != null)
                    {
                        if (frame.TrackingId == 0)
                        {
                            continue;
                        }

                        // do something with result
                        var result = frame.FaceFrameResult;

                        // extract face rotation in degrees as Euler angles
                        if (result.FaceRotationQuaternion != null )
                        {
                            int pitch, yaw, roll;
                            ExtractFaceRotationInDegrees(result.FaceRotationQuaternion, out pitch, out yaw, out roll);
                            multY = 1.73f;
                            /*
                            if (pitch > 8)
                            {
                                multY = 1.73f;
                            }
                            else if (pitch > 3 && pitch < 8)
                            {
                                multY = 0f;
                            }
                            else
                            {
                                multY = 5.5f;
                            }*/
                            Vector3 posRay = new Vector3(yaw * multX + centerX, pitch* multY + centerY, 0);
                            Vector3 pos = Camera.main.WorldToScreenPoint(posRay);
                            Ray ray = Camera.main.ScreenPointToRay(pos);

                            RaycastHit hit;

                            Debug.DrawLine(ray.origin, ray.direction, UnityEngine.Color.red);
                            Debug.DrawLine(ray.origin, posRay, UnityEngine.Color.cyan);

                            marcador.transform.position = posRay;

                            Debug.Log("posRay= " + posRay + "ray= " + ray.origin + 100 * ray.direction);


                            if (Physics.Raycast(ray, out hit) == true)
                            {
                                Debug.Log("choque con photo " + hit.transform.gameObject.tag);
                                hit.transform.GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
                            }
                            else{
                                GameObject[] photos = GameObject.FindGameObjectsWithTag("photo");
                                for (int y = 0; y < photos.Length; y++)
                                {
                                    Renderer rendererPhoto = photos[y].transform.GetComponent<Renderer>();
                                    if (rendererPhoto.material.color == UnityEngine.Color.yellow)
                                    {
                                        rendererPhoto.material.color = UnityEngine.Color.white;
                                    }
                                }
                            }
                            /*
                            Vector3 fwd = transform.TransformDirection(posRay);

                            if (Physics.Raycast(transform.position, fwd, 100))
                            {
                                Debug.Log("choque");
                            }
                            */
                                //updateFrame = !updateFrame;
                        }
                    }
                }
            }
            else
            {
                // check if the corresponding body is tracked 
                if (bodies[i].IsTracked)
                {
                    // update the face frame source to track this body
                    faceFrameSources[i].TrackingId = bodies[i].TrackingId;
                }
            }
        }
    }
    private static void ExtractFaceRotationInDegrees(Windows.Kinect.Vector4 rotQuaternion, out int pitch, out int yaw, out int roll)
    {
        double x = rotQuaternion.X;
        double y = rotQuaternion.Y;
        double z = rotQuaternion.Z;
        double w = rotQuaternion.W;

        // convert face rotation quaternion to Euler angles in degrees
        double yawD, pitchD, rollD;
        pitchD = Math.Atan2(2 * ((y * z) + (w * x)), (w * w) - (x * x) - (y * y) + (z * z)) / Math.PI * 180.0;
        yawD = Math.Asin(2 * ((w * y) - (x * z))) / Math.PI * 180.0;
        rollD = Math.Atan2(2 * ((x * y) + (w * z)), (w * w) + (x * x) - (y * y) - (z * z)) / Math.PI * 180.0;

        // clamp the values to a multiple of the specified increment to control the refresh rate
        double increment = FaceRotationIncrementInDegrees;
        pitch = (int)(Math.Floor((pitchD + ((increment / 2.0) * (pitchD > 0 ? 1.0 : -1.0))) / increment) * increment);
        yaw = (int)(Math.Floor((yawD + ((increment / 2.0) * (yawD > 0 ? 1.0 : -1.0))) / increment) * increment);
        roll = (int)(Math.Floor((rollD + ((increment / 2.0) * (rollD > 0 ? 1.0 : -1.0))) / increment) * increment);
    }
}

    /*
    void Update()
    {

        if (bodyManager == null)
        {
            return;
        }
        bodies = bodyManager.GetData();

        if (bodies == null)
        {
            return;
        }
        int i = 0;
        foreach (var body in bodies)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                Debug.Log("detecta un cuerpo");
                faceFrameSources[i].TrackingId = body.TrackingId;

                if (faceFrameSources[i].IsTrackingIdValid)
                {
                    Debug.Log("Entra a faceFrameSources isTrackingIdValid");
                    using (FaceFrame frame = faceFrameReaders[i].AcquireLatestFrame())
                    {
                        if (frame != null)
                        {
                            if (frame.TrackingId == 0)
                            {
                                continue;
                            }
                        }

                        Debug.Log(" captura resultados");

                        var result = frame.FaceFrameResult;

                        var eyeLeft = result.FacePointsInColorSpace[FacePointType.EyeLeft];
                        var eyeRigh = result.FacePointsInColorSpace[FacePointType.EyeRight];

                        goEyeLeft.transform.position = new Vector3(eyeLeft.X * 10f, eyeLeft.Y * 10f);
                        goEyeRight.transform.position = new Vector3(eyeRigh.X * 10f, eyeRigh.Y * 10f);

                        /*     
                        archivoX.Write(result.FaceRotationQuaternion.X + "\n");
                        archivoY.Write(result.FaceRotationQuaternion.Y + "\n");
                        archivoZ.Write(result.FaceRotationQuaternion.Z + "\n");*/

                        /*
                        Debug.Log("Angulos: " + result.FaceRotationQuaternion.X.ToString() + "," + result.FaceRotationQuaternion.Y.ToString() + "," + result.FaceRotationQuaternion.Z.ToString());
                        //Pitch Angle neutral = 0 // - 90 = looking down towards the floor  //+90 = looking up towards the ceiling
                        if (result.FaceRotationQuaternion.X > 90)
                        {
                            Debug.Log("Usuario Mira hacia el techo");
                        }
                        else if (result.FaceRotationQuaternion.X < -90)
                        {
                            Debug.Log("Usuario Mira hacia el suelo");
                        }
                        else
                        {
                            Debug.Log("Usuario Mira al frente eje X");
                        }

                        if (result.FaceRotationQuaternion.Y < -90)
                        {
                            Debug.Log("Usuario Mira hacia la izquierda");
                        }
                        else if (result.FaceRotationQuaternion.Y > 90)
                        {
                            Debug.Log("Usuario Mira hacia la derecha");
                        }
                        else
                        {
                            Debug.Log("Usuario Mira al frente eje Y");
                        }

                        if (result.FaceRotationQuaternion.Z > -90)
                        {
                            Debug.Log("Usuario esta sobre su hombro derecho");
                        }
                        else if (result.FaceRotationQuaternion.Z < 90)
                        {
                            Debug.Log("Usuario esta sobre su hombro izquierdo");
                        }
                        else
                        {
                            Debug.Log("Usuario Mira al frente eje Z");
                        }

                    }
                }
                i++;
            }
        }
    }
}
/*
        // iterate through each body and update face source
        for (int i = 0; i < bodyCount; i++)
        {

            //Compruebe si un rostro válido se rastrea en esta fuente de rostro
            if (faceFrameSources[i].IsTrackingIdValid)
            {
                using (FaceFrame frame = faceFrameReaders[i].AcquireLatestFrame())
                {
                    if (frame != null)
                    {
                        if (frame.TrackingId == 0)
                        {
                            continue;
                        }

                        // do something with result
                        
                        var result = frame.FaceFrameResult;

                        var eyeLeft = result.FacePointsInColorSpace[FacePointType.EyeLeft];
                        var eyeRigh = result.FacePointsInColorSpace[FacePointType.EyeRight];

                        goEyeLeft.transform.position = new Vector3(eyeLeft.X * 10f, eyeLeft.Y * 10f);
                        goEyeRight.transform.position = new Vector3(eyeRigh.X * 10f, eyeRigh.Y * 10f);

                        /*     
                        archivoX.Write(result.FaceRotationQuaternion.X + "\n");
                        archivoY.Write(result.FaceRotationQuaternion.Y + "\n");
                        archivoZ.Write(result.FaceRotationQuaternion.Z + "\n");*/
                        // X - Pitch
                        // Y - Yaw
                        // Z - Roll


                        //Pitch Angle neutral = 0 // - 90 = looking down towards the floor  //+90 = looking up towards the ceiling
  /*                      if (result.FaceRotationQuaternion.X > 90)
                        {
                            Debug.Log("Usuario Mira hacia el techo");
                        }
                        else if(result.FaceRotationQuaternion.X < -90)
                        {
                            Debug.Log("Usuario Mira hacia el suelo");
                        }
                        else
                        {
                            Debug.Log("Usuario Mira al frente eje X");
                        }

                        if(result.FaceRotationQuaternion.Y < -90)
                        {
                            Debug.Log("Usuario Mira hacia la izquierda");
                        }
                        else if(result.FaceRotationQuaternion.Y > 90)
                        {
                            Debug.Log("Usuario Mira hacia la derecha");
                        }
                        else
                        {
                            Debug.Log("Usuario Mira al frente eje Y");
                        }

                        if(result.FaceRotationQuaternion.Z > -90)
                        {
                            Debug.Log("Usuario esta sobre su hombro derecho");
                        }
                        else if(result.FaceRotationQuaternion.Z < 90)
                        {
                            Debug.Log("Usuario esta sobre su hombro izquierdo");
                        }
                        else
                        {
                            Debug.Log("Usuario Mira al frente eje Z");
                        }

                    }
                }
            }

            else
            {
                // check if the corresponding body is tracked 
                Debug.Log(bodies[i].IsTracked);
                /*if (bodies[i].IsTraking)
                {
                    // update the face frame source to track this body
                    faceFrameSources[i].TrackingId = bodies[i].TrackingId;
                }*/
          /*  }
        }*/

    

