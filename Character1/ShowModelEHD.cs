using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowModelEHD : MonoBehaviour {

	public GameObject target;
	public GameObject egg;
	public GameObject main;
	int[] Index = new int[3];
	static public int IndexforFinal = 0;
	static public bool showModel = false;
	public Transform[] prefab = new Transform[3];
	bool display = true;
	bool match;
	Transform newCharacter;
	GameObject model;
	public GameObject precious, joystick;
	public Text m_MyText;
	Image mImage;
	//public Joyscript myJoystick;
	int[] currentNearest = new int[3];
	int[] calculateNum = new int[3];
	int sum = 0;
	//System.Collections.Generic.mytype m;

	//public Transform myModelPrefab;
	//public GameObject imagetarget;

	// Use this for initialization
	void Start () {
//		mTrackableBehaviour = imagetarget.GetComponent<TrackableBehaviour>();
//
//		if (mTrackableBehaviour)
//		{
//			mTrackableBehaviour.RegisterTrackableEventHandler(this);
//		}

		mImage = joystick.GetComponent<Image> ();
		//mJoystick=joystick.GetComponent<Joystick>();
	}
	
	// Update is called once per frame
	void Update () {
		match = target.GetComponent<ImproEHD> ().match && target.GetComponent<ShapeRcognition2>().match && target.GetComponent<ImproHarris>().match;
		if (match) {
			if (display) {
				
				currentNearest [0] = target.GetComponent<ImproEHD> ().currentNearest;
				currentNearest [1] = target.GetComponent<ShapeRcognition2> ().currentNearest;
				currentNearest [2] = target.GetComponent<ImproHarris> ().currentNearest;

				Index[0] = currentNearest [0];
				Index[1] = currentNearest [1];
				Index[2] = currentNearest [2];
				IndexforFinal = Index [0];

				m_MyText.text = "EHD: " + Index [0] + "Countours: " + Index [1] + "Harris: " + Index [2];
//				if (currentNearest [1] == 0) {
//					if (currentNearest [0] == 1) {
//						Index = 1;
//					}
//				}
//
//				if (currentNearest [2] == 2 || currentNearest [1] == 2) {
//					Index = 2;
//				}

		
//				Debug.Log (sum);
				Debug.Log ("Index" + Index);


				//display = false;
				//newCharacter= Instantiate (prefab[Index], target.transform.position, Quaternion.identity);
				if (MyPrefabInstantiator.found) {
					//if () {
						newCharacter = Instantiate (prefab [Index [0]], target.transform.position, Quaternion.identity, target.transform);
						//newCharacter.transform.Rotate(new Vector3(0, 90, 0),local;
						newCharacter.transform.eulerAngles = target.transform.eulerAngles;
						//newCharacter= Instantiate (prefab[Index], target.transform);
						newCharacter.transform.localScale *= 0.03f;
						newCharacter.transform.parent = target.transform;
						model = newCharacter.GetChild (0).gameObject;
						//				model = newCharacter.Find ("Toon Crow").gameObject;
						model.GetComponent<Renderer> ().material.color = ImproEHD.COLORHSV;
						showModel = true;
						//newCharacter.LookAt(main.transform);
						egg.SetActive (false);
						display = false;
						precious.SetActive (true);
						mImage.enabled=true;
						//joystick.GetComponent(Joystick).enable=true;
						Debug.Log ("match display");
					//}
				}
			}
//			newCharacter.LookAt(main.transform);
//			newCharacter.transform.RotateAround(main.transform.position, Vector3.up, 80 * Time.deltaTime);
			//			Debug.Log (target.transform.position);
		}
		Debug.Log ("IndexforFinal: " + IndexforFinal);
	}

//	public void OnTrackableStateChanged(
//		TrackableBehaviour.Status previousStatus,
//		TrackableBehaviour.Status newStatus)
//	{
//		if (newStatus == TrackableBehaviour.Status.DETECTED ||
//			newStatus == TrackableBehaviour.Status.TRACKED)
//		{
//			found = true;
//		}
//	}
//	private void OnTrackingFound()
//	{
//		if (myModelPrefab != null)
//		{
//			Transform myModelTrf = GameObject.Instantiate(myModelPrefab) as Transform;
//
//			myModelTrf.parent = mTrackableBehaviour.transform;             
//			myModelTrf.localPosition = new Vector3(0f, 0f, 0f);
//			myModelTrf.localRotation = Quaternion.identity;
//			myModelTrf.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
//
//			myModelTrf.gameObject.active = true;
//		}
//	}
	//public 

	public void ReStart(){
		display = true;
		match = false;
		showModel = false;
		IndexforFinal = 0; 
		sum = 0;
	}

}
