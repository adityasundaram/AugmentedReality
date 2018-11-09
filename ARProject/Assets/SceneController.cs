using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;
public class SceneController : MonoBehaviour {

    public Camera firstPersonCamera;
    public GamerController characterController;

    void ProcessTouches()
    {
            Touch touch;
            if (Input.touchCount != 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                SetSelectedPlane(hit.Trackable as DetectedPlane, hit.Trackable.CreateAnchor(hit.Pose) as Anchor);
                var generator = this.GetComponent<DetectedPlaneGenerator>();
                generator.disablePlane();
                //stopShowingPlanes();
            }
    }

    void stopShowingPlanes()
    {
        foreach (GameObject plane in GameObject.FindGameObjectsWithTag("plane"))
        {
            Renderer r = plane.GetComponent<Renderer>();
            DetectedPlaneVisualizer t = plane.GetComponent<DetectedPlaneVisualizer>();
            t.disableMesh();
            r.enabled = false;
            t.enabled = false;
        }
    }

    void SetSelectedPlane(DetectedPlane selectedPlane, Anchor anchor)
    {
        //Debug.Log(" Selected Plane centered at " + selectedPlane.CenterPose.position);
        characterController.SetPlane(selectedPlane, anchor);
    }

    void QuitOnConnectionErrors()
    {
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "Camera permission is needed to run this application.", 5));
        }
        else if (Session.Status.IsError())
        {
            // This covers a variety of errors.  See reference for details
            StartCoroutine(CodelabUtils.ToastAndExit(
                "ARCore encountered a problem connecting. Please restart the app.", 5));
        }
    }

    // Use this for initialization
    void Start () {
        QuitOnConnectionErrors();
	}
	
	// Update is called once per frame
	void Update () {
        // The session status must be Tracking in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking)
        {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ProcessTouches();
    }
}
