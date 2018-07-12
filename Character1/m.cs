using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class m : MonoBehaviour {

	public float moveForce = 2, jumpValue =5;
	Rigidbody myBody;
	float speed=1;
	Vector3 old;
	Vector3 current;
	float height = 0.5f;
	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody> ();
		old = transform.position;
		current = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		//		Vector3 moveH= new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"));
		//		Vector3	moveV= new Vector3(CrossPlatformInputManager.GetAxis("Vertical"));
		//		myBody.AddForce (moveH);
		//		myBody.AddForce (moveV);
		Vector3 temp = transform.position;
		temp.y = height;
		transform.position = temp;

		Debug.Log (transform.position.y);

		current = transform.position;
		//		float moveH= CrossPlatformInputManager.GetAxis("Horizontal")* speed;
		//		float moveV= CrossPlatformInputManager.GetAxis("Vertical")* speed;
		//
		//
		////		Debug.Log (moveH);
		//		Vector3 movement = new Vector3(moveH, 0, moveV);
		Vector3 movement=current-old;
		//myBody.AddForce (moveH, 0, moveV);
		//transform.Rotate(-moveV,moveH,0f);
		//transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,0f);
		//transform.rotation = Quaternion.LookRotation(new Vector3(moveH, 0, moveV));
		if(movement!=Vector3.zero){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
			transform.Translate (movement * moveForce * Time.deltaTime, Space.World);
		}
		old = current;
		//transform.Translate(0f, 0f,moveV); 
		//		if (CrossPlatformInputManager.GetButton("Jump")) {
		//			Debug.Log (CrossPlatformInputManager.GetButton( ("Jump")));
		//			//myBody.AddForce (Vector3.up *jumpValue);
		//			transform.Translate(0f, jumpValue*Time.deltaTime,0f); 
		//		}

	}
	//	public void jump(){
	//		Debug.Log (CrossPlatformInputManager.GetButton( ("Jump")));
	//		//if (CrossPlatformInputManager.GetButton ("Jump")) {
	//		//myBody.AddForce (Vector3.up *jumpValue*Time.deltaTime);
	//		transform.Translate(0f, jumpValue*Time.deltaTime,0f); 
	//		//}
	//	}
}
