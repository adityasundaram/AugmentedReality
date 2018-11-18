using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;
public class SceneController : MonoBehaviour {

    public Camera firstPersonCamera;
    public GamerController characterController;
    private bool loaded;
    void ProcessTouches()
    {
            Touch touch;
            if (Input.touchCount != 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }
        if (!loaded)
        {
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.FeaturePointWithSurfaceNormal | TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                SetSelectedPlane(hit.Trackable as DetectedPlane, Session.CreateAnchor(hit.Pose) as Anchor, hit);
                var generator = this.GetComponent<DetectedPlaneGenerator>();
                loaded = true;
                //generator.disablePlane();
                //stopShowingPlanes();
            }
        }
        else
        {
            /*RaycastHit hit;
            Ray ray = firstPersonCamera.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Touched this mofo " + hit.transform.name);
                Debug.Log("Height of object starting " + hit.transform.position.y + "Tall: " + hit.transform.lossyScale.y);
            }*/
        }

            
    }

    void stopShowingPlanes()
    {
        foreach (GameObject plane in GameObject.FindGameObjectsWithTag("Plane"))
        {
            Renderer r = plane.GetComponent<Renderer>();
            DetectedPlaneVisualizer t = plane.GetComponent<DetectedPlaneVisualizer>();
            //t.disableMesh();
            r.enabled = false;
            t.enabled = false;
        }
    }

    void SetSelectedPlane(DetectedPlane selectedPlane, Anchor anchor, TrackableHit hit)
    {
        //Debug.Log(" Selected Plane centered at " + selectedPlane.CenterPose.position);
        characterController.SetPlane(selectedPlane, anchor, hit);
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
        loaded = false;
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
