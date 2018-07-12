using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModel2 : MonoBehaviour {
	
	public GameObject target;
	public GameObject egg;
	public GameObject main;
	int Index = 0;
	public Transform[] prefab = new Transform[3];
	bool display = true;
	bool match;
	Transform newCharacter;
	GameObject model;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		match = target.GetComponent<ShapeRcognition2> ().match;
//		Debug.Log (match);
		if (match) {
			if (display) {
				Index = target.GetComponent<ShapeRcognition2> ().currentNearest;
//				Debug.Log ("Index");
//				Debug.Log (Index);

				newCharacter= Instantiate (prefab[Index], target.transform.position, Quaternion.identity);
				newCharacter.transform.localScale *= 0.4f;
				newCharacter.transform.parent = target.transform.parent;
				model = newCharacter.GetChild (0).gameObject;
//				model = newCharacter.Find ("Toon Crow").gameObject;
				model.GetComponent<Renderer> ().material.color = ShapeRcognition2.colorHSV;

				egg.SetActive(false);
				display = false;
			}
			newCharacter.LookAt(main.transform);
			newCharacter.transform.RotateAround(main.transform.position, Vector3.up, 80 * Time.deltaTime);
//			Debug.Log (target.transform.position);
		}
	}
}
