using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModel : MonoBehaviour {

	public GameObject target;
	int Index = 0;
	public Transform[] prefab = new Transform[3];
	bool display = true;
	bool match;
	Transform newCharacter;
	public static GameObject model;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		match = target.GetComponent<ShapeRcognition2> ().match;
		if (match) {
			if (display) {
				Index = target.GetComponent<ShapeRcognition2> ().currentNearest;
				newCharacter = Instantiate (prefab [Index], target.transform.position, Quaternion.identity);
				newCharacter.transform.localScale *= 0.4f;
				newCharacter.transform.parent = target.transform.parent;
				model = newCharacter.GetChild (0).gameObject;
				model.GetComponent<Renderer> ().material.color = ShapeRcognition2.colorHSV;
				//model = model.transform.GetChild (0);
				display = false;
//				Debug.Log (display);
			}
		}
	}
}
