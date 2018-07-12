using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class PressPosition2 : MonoBehaviour {

	GameObject model;
	bool addScript = false;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (ShowModelEHD2.showModel) {
			if(!addScript){
				addScript = true;
				model = this.transform.GetChild (0).gameObject;
				model.AddComponent<LeanSelectable>();
				model.AddComponent<LeanTranslateSmooth> (); 
				model.AddComponent<m> ();
			}
		}
	}
	public void Restart(){
		addScript = false;
	}
}
