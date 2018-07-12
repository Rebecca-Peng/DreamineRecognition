using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToOpen : MonoBehaviour {
	//public bool open;
	Animator lid, prop;
//	GameObject treasure;
//	static public bool enabled;
	// Use this for initialization
	void Start () {
		lid=gameObject.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
//		if (ShowItems.showmodel == true) {
//			treasure = ShowItems.model;
//			prop=treasure.GetComponent<Animator>();
//			lid.SetBool("open", true);
//			prop.SetBool ("appear", true);

//		}
//		openTheLid(enabled);
	}

	public void openTheLid(){
		lid.SetBool ("isOpen",true);
	}

}
