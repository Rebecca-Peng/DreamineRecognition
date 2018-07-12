using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModelEHDDesert : MonoBehaviour {

	public GameObject target;
	public GameObject main;
	int Index = 0;
	public Transform[] prefab = new Transform[3];
	bool display = true;
	bool match;
	Transform newCharacter;
	GameObject model;

	int[] currentNearest = new int[3];
	int[] calculateNum = new int[3];
	int sum = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		match = target.GetComponent<ImproEHD> ().match && target.GetComponent<ShapeRcognition2>().match && target.GetComponent<ImproHarris>().match;
		Debug.Log ("Match:   "+ match);
		if (match) {
			if (display) {

				currentNearest [0] = target.GetComponent<ImproEHD> ().currentNearest;
				currentNearest [1] = target.GetComponent<ShapeRcognition2> ().currentNearest;
				currentNearest [2] = target.GetComponent<ImproHarris> ().currentNearest;

				for (int i = 0; i < 3; i++) {
					if (currentNearest [i] == 2) {
						calculateNum [i] = -1;
					} else {
						calculateNum [i] = currentNearest [i];
					}
					sum += calculateNum [i];
				}

				if (sum <= 0) {
					Index = 0;
				} else if (sum >= 2) {
					Index = 2;
				} else {
					Index = 1;
				}

//				Index = target.GetComponent<ImproHarris> ().currentNearest;

				Debug.Log ("Index");
				Debug.Log (Index);

				newCharacter= Instantiate (prefab[Index], target.transform.position, Quaternion.identity,transform);
				newCharacter.transform.localScale *= 0.6f;
				newCharacter.transform.parent = target.transform.parent;
				model = newCharacter.GetChild (0).gameObject;
				//				model = newCharacter.Find ("Toon Crow").gameObject;
				model.GetComponent<Renderer> ().material.color = ImproEHD.COLORHSV;

				display = false;
			}
			//			Debug.Log (target.transform.position);
			newCharacter.LookAt(main.transform);
		}
	}
}
