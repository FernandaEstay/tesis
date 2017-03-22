using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class ScrDetectHead : MonoBehaviour {

    public GameObject BodySrcManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;

    private float multiplier = 100f;
    private float centerX = 45f;
    private float centerY = -15f;

	// Use this for initialization
	void Start () {
		if(BodySrcManager == null)
        {
            Debug.Log("Falta asignar Game Object as BodySrcManager");
        }
        else
        {
            bodyManager = BodySrcManager.GetComponent<BodySourceManager>();
        }
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(bodyManager == null)
        {
            return;
        }
        bodies = bodyManager.GetData();

        if(bodies == null)
        {
            return;
        }
        foreach(var body in bodies)
        {
            if(body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                var pos = body.Joints[TrackedJoint].Position;
                if (pos.X > -0.5f)
                {
                    gameObject.transform.position = new Vector3((pos.X * multiplier + centerX) * 1.62f, pos.Y * multiplier + centerY);
                }
                else if (pos.X < 0.5f)
                {
                    gameObject.transform.position = new Vector3((pos.X * multiplier + centerX) * 0.08f, pos.Y * multiplier + centerY);
                }
                else
                {
                    gameObject.transform.position = new Vector3(pos.X * multiplier + centerX, pos.Y * multiplier + centerY);
                }
                Debug.Log("posicion cabeza = " + pos.X + "," + pos.Y + "," + pos.Z);
            }
        }
	}

}
