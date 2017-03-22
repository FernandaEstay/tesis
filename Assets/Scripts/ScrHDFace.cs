using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;


public class ScrHDFace : MonoBehaviour {
	
	private KinectSensor sensor = null;

	private BodyFrameSource bodySource = null;
	private BodyFrameReader bodyReader = null;

	private HighDefinitionFaceFrameSource highDefinitionFaceFrameSource = null;
	private HighDefinitionFaceFrameReader highDefinitionFaceFrameReader = null;

	private FaceAlignment currentFaceAlignment = null;
	private FaceModel currentFaceModel = null;

	private Body currentTrackedBody = null;
	private ulong currentTrackingId = 0;

	private ulong CurrentTrackingId
	{
		get
		{
			return currentTrackingId;
		}

		set
		{
			currentTrackingId = value;
		}
	}

	private FaceModel CurrentFaceModel
	{
		get
		{
			return currentFaceModel;
		}

		set
		{
			if (currentFaceModel != null)
			{
				currentFaceModel.Dispose ();
				currentFaceModel = null;
			}

			currentFaceModel = value;
		}
	}

	private static double VectorLength(CameraSpacePoint point)
	{
		var result = Mathf.Pow(point.X, 2) + Mathf.Pow(point.Y, 2) + Mathf.Pow(point.Z, 2);

		result = Mathf.Sqrt(result);

		return result;
	}

	private static Body FindClosestBody(BodyFrame bodyFrame)
	{
		Body result = null;
		double closestBodyDistance = double.MaxValue;

		Body[] bodies = new Body[bodyFrame.BodyCount];
		bodyFrame.GetAndRefreshBodyData(bodies);

		foreach (var body in bodies)
		{
			if (body.IsTracked)
			{
				var currentLocation = body.Joints[JointType.SpineBase].Position;

				var currentDistance = VectorLength(currentLocation);

				if (result == null || currentDistance < closestBodyDistance)
				{
					result = body;
					closestBodyDistance = currentDistance;
				}
			}
		}

		return result;
	}

	private static Body FindBodyWithTrackingId(BodyFrame bodyFrame, ulong trackingId)
	{
		Body result = null;

		Body[] bodies = new Body[bodyFrame.BodyCount];
		bodyFrame.GetAndRefreshBodyData(bodies);

		foreach (var body in bodies)
		{
			if (body.IsTracked)
			{
				if (body.TrackingId == trackingId)
				{
					result = body;
					break;
				}
			}
		}

		return result;
	}

	void Start(){
		sensor = KinectSensor.GetDefault();
		bodySource = sensor.BodyFrameSource;
		bodyReader = bodySource.OpenReader();
		bodyReader.FrameArrived += BodyReader_FrameArrived;

		highDefinitionFaceFrameSource = HighDefinitionFaceFrameSource.Create(sensor);
		highDefinitionFaceFrameSource.TrackingIdLost += HdFaceSource_TrackingIdLost;

		highDefinitionFaceFrameReader = highDefinitionFaceFrameSource.OpenReader();
		highDefinitionFaceFrameReader.FrameArrived += HdFaceReader_FrameArrived;

		CurrentFaceModel = FaceModel.Create();
		currentFaceAlignment = FaceAlignment.Create();

		sensor.Open();
	}

	private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
	{
		var frameReference = e.FrameReference;
		using (var frame = frameReference.AcquireFrame())
		{
			if (frame == null)
			{
				// We might miss the chance to acquire the frame, it will be null if it's missed
				return;
			}

			if (currentTrackedBody != null)
			{
				currentTrackedBody = FindBodyWithTrackingId(frame, CurrentTrackingId);

				if (currentTrackedBody != null)
				{
					return;
				}
			}

			Body selectedBody = FindClosestBody(frame);

			if (selectedBody == null)
			{
				return;
			}

			currentTrackedBody = selectedBody;
			CurrentTrackingId = selectedBody.TrackingId;

			highDefinitionFaceFrameSource.TrackingId = CurrentTrackingId;
		}
	}

	private void HdFaceSource_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
	{
		var lostTrackingID = e.TrackingId;

		if (CurrentTrackingId == lostTrackingID)
		{
			CurrentTrackingId = 0;
			currentTrackedBody = null;

			highDefinitionFaceFrameSource.TrackingId = 0;
		}
	}

	private void HdFaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
	{
		using (var frame = e.FrameReference.AcquireFrame())
		{
			// We might miss the chance to acquire the frame; it will be null if it's missed.
			// Also ignore this frame if face tracking failed.
			if (frame == null || !frame.IsFaceTracked)
			{
				return;
			}

			frame.GetAndRefreshFaceAlignmentResult(currentFaceAlignment);
		}
	}
}