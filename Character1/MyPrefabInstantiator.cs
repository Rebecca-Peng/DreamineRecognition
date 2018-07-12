using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MyPrefabInstantiator : MonoBehaviour, ITrackableEventHandler {

	private TrackableBehaviour mTrackableBehaviour;

	static public bool found;
	// Use this for initialization
	void Start ()
	{
		found = false;
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();

		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}               

	// Update is called once per frame
	void Update ()
	{
	}

	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED) {
			found = true;
		} else {
			found = false;
		}
	}

}