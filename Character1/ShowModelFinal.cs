using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowModelFinal : MonoBehaviour {

	int Index = 0;
	public Transform[] prefab = new Transform[3];
	public GameObject target;
	bool display = true;
	Transform newCharacter;
	GameObject model;
	static public bool appear;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (display) {
			if (ShowModelEHD.showModel) {
				Index = ShowModelEHD.IndexforFinal;
				Debug.Log ("Index:   " + Index);
				newCharacter = Instantiate (prefab [Index], target.transform.position, Quaternion.identity, target.transform);
				newCharacter.transform.eulerAngles = target.transform.eulerAngles;
				newCharacter.transform.parent = target.transform.parent;
				newCharacter.transform.localScale *= 0.03f;
				newCharacter.transform.parent = target.transform;
				model = newCharacter.GetChild (0).gameObject;
				model.GetComponent<Renderer> ().material.color = ImproEHD.COLORHSV;
				display = false;
				appear = true;
			}
		}
	}
}
