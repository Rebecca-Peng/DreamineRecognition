using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rect= UnityEngine.Rect;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;

public class CaptureEHD : MonoBehaviour {
	static public Texture2D dstTexture;
	static public bool getImage, grab;
	Color rectColor = new Color(55,44,13);
	Color rectColor2 = new Color(0,255,255);
	float lineWidth = Screen.width/256;
	float x = Screen.width/4-Screen.width/256;
	float y = (Screen.height-Screen.width/2)/2+Screen.width/256;
	float w = Screen.width/2+Screen.width/128;
	float l = Screen.width/2+Screen.width/128;
	bool cap;
	bool menabled;
	public GameObject startUI;
	// Use this for initialization
	void Start () {
		dstTexture= new Texture2D(Screen.width/2, Screen.width/2,  TextureFormat.RGBA32, false);


	}

	// Update is called once per frame
	void Update () {
		lineWidth = Screen.width/256;
		x = Screen.width/4-Screen.width/256;
		y = (Screen.height-Screen.width/2)/2+Screen.width/256;
		w = Screen.width/2+Screen.width/128;
		l = Screen.width/2+Screen.width/128;
		if(grab){
			StartCoroutine(Impo());
			//			Debug.Log ("grab");
		}
	}

	IEnumerator Impo()
	{

		yield return new WaitForEndOfFrame();

		dstTexture.ReadPixels(new Rect(x+lineWidth,y+lineWidth,Screen.width/2, Screen.width/2), 0, 0, false);
		//		Debug.Log("Impo");
		getImage = true;
		grab=false;

	}

	void OnGUI()
	{

		if (!cap && menabled) {
			if (GUI.Button (new Rect (0, 0, Screen.width / 6, Screen.width / 6), "Snap")) {
				grab = true;
				//GUI.Box(new Rect(Screen.width/4+Screen.width/256, (Screen.height-Screen.width/2)/2+Screen.width/256,), "ReadPixels")
				cap = true;
			}
		}

		Debug.Log("getImage" + getImage);
		Debug.Log("menalbe"+ menabled);
		if(!getImage && menabled){

			//		DrawQuad (new Rect(x,y,w,lineWidth),rectColor);
			//		DrawQuad (new Rect(x,y,lineWidth,l),rectColor);
			//		DrawQuad (new Rect(x+w,y,lineWidth,l),rectColor);
			//		DrawQuad (new Rect(x,y+l,w,lineWidth),rectColor);

			DrawQuad (new Rect(0,0,x,Screen.height),rectColor);
			DrawQuad (new Rect(0,0,Screen.width,y),rectColor);
			DrawQuad (new Rect(x+w,0,Screen.width-x-w,Screen.height),rectColor);
			DrawQuad (new Rect(0,y+l,Screen.width,Screen.height-l-y),rectColor);
			GUI.Button (new Rect (0, 0, Screen.width / 6, Screen.width / 6), "Snap");
			//DrawQuad (new Rect(x+lineWidth,y+lineWidth,Screen.width/2, Screen.width/2),rectColor2);
		}



	}

	void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		//		GUI.Box(position,GUIContent.none, style);
		GUI.Box(position, GUIContent.none);
	}

	public void setenable(){
		menabled = true;
		startUI.SetActive (false);
	}

	public void ReStart(){
		dstTexture= new Texture2D(Screen.width/2, Screen.width/2,  TextureFormat.RGBA32, false);
		getImage = false;
		grab = false;
		cap = false;
		menabled = false;
	}
}