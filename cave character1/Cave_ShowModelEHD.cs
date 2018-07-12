using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cave_ShowModelEHD : MonoBehaviour {

	public GameObject target;
//	public GameObject egg;
//	public GameObject main;
	int[] Index = new int[3];
	static public int IndexforFinal = 0;
	static public bool showModel = false;
	public Transform[] prefab = new Transform[3];
	bool display = true;
	bool match;
	Transform newCharacter;
	GameObject model;
	public GameObject precious;
//	public Text m_MyText;
//	Image mImage;
	//public Joyscript myJoystick;
	int[] currentNearest = new int[3];
	int[] calculateNum = new int[3];
	int sum = 0;

	public GameObject[] particle = new GameObject[3]; 


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		match = target.GetComponent<Cave_ImproEHD> ().match;
		if (match) {
			if (display) {
				Debug.Log ("display:  " + display);
				currentNearest [0] = target.GetComponent<Cave_ImproEHD> ().currentNearest;
				currentNearest [1] = target.GetComponent<Cave_ImproEHD> ().currentNearest;
				currentNearest [2] = target.GetComponent<Cave_ImproEHD> ().currentNearest;

				Index[0] = currentNearest [0];
				Index[1] = currentNearest [1];
				Index[2] = currentNearest [2];
				IndexforFinal = Index [0];

//				m_MyText.text = "EHD: " + Index [0] + "Countours: " + Index [1] + "Harris: " + Index [2];
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
					Debug.Log ("Display Models");
					showModel = true;
					//newCharacter.transform.Rotate(new Vector3(0, 90, 0),local;
					newCharacter.transform.eulerAngles = target.transform.eulerAngles;
					//newCharacter= Instantiate (prefab[Index], target.transform);
					newCharacter.transform.localScale *= 0.03f;
					newCharacter.transform.parent = target.transform;
					model = newCharacter.gameObject;
					//				model = newCharacter.Find ("Toon Crow").gameObject;
					model.GetComponent<Renderer> ().material.color = Cave_ImproEHD.COLORHSV;
//					particle [IndexforFinal].SetActive (true);
//					var col = GetComponent<ParticleSystem> ().colorOverLifetime;
//					col.enabled = true;
//					col.color = Cave_ImproEHD.COLORHSV;
					display = false;
					precious.SetActive (true);
//					mImage.enabled=true;
					//joystick.GetComponent(Joystick).enable=true;
					Debug.Log ("display:  " + display);
					//}
				}
			}
		}
		Debug.Log ("IndexforFinal: " + IndexforFinal);
	}


	void OnMouseDown(){
		if (showModel) {
			model.SetActive (false);
			particle[IndexforFinal].SetActive (true);
			var col = particle[IndexforFinal].GetComponent<ParticleSystem> ().colorOverLifetime;
			col.enabled = true;
			col.color = Cave_ImproEHD.COLORHSV;
		}
	}


	public void destory(){
		if (showModel) {
			model.SetActive (false);
		}
	}
		


	public void ReStart(){
		display = true;
		match = false;
		showModel = false;
		IndexforFinal = 0; 
		sum = 0;
		Debug.Log ("Showmodel Resart");
	}

}
