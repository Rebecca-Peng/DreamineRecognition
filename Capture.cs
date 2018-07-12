using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rect= UnityEngine.Rect;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;
public class Capture : MonoBehaviour {


	static public Texture2D dstTexture;
	static public bool getImage, grab;
	//public GameObject cube;
	Color rectColor = new Color(0,0,255);
	float x = Screen.width/4+Screen.width/256;
	float y = (Screen.height-Screen.width/2)/2+Screen.width/256;
	float w = Screen.width/2-Screen.width/128;
	float l = Screen.width/2-Screen.width/128;
	float lineWidth = 2;

	// Use this for initialization
	void Start () {
		dstTexture= new Texture2D(Screen.width/3, Screen.width/3,  TextureFormat.RGBA32, false);

	}
	
	// Update is called once per frame
	void Update () {
		if(grab){
			StartCoroutine(Impo());
//			Debug.Log ("grab");
		}
//		cube.GetComponent<Renderer> ().material.mainTexture = dstTexture;
	}


	IEnumerator Impo()
	{

		yield return new WaitForEndOfFrame();

		dstTexture.ReadPixels(new Rect(x,y,w,l), 0, 0, false);
//		dstTexture.ReadPixels(new Rect(Screen.width/4, (Screen.height-Screen.width/2)/2,Screen.width/2, Screen.width/2), 0, 0, false);
//		Debug.Log("Impo");
//		Debug.Log(getImage);
		getImage = true;
		grab=false;

	}

	void OnGUI()
	{
//		if (GUI.Button(new Rect(0,0,Screen.width/6, Screen.width/6), "ReadPixels")) {
//			grab=true;
////			Debug.Log("here");
//		}
//		DrawQuad (new Rect(x,y,w,lineWidth),rectColor);
//		DrawQuad (new Rect(x,y,lineWidth,l),rectColor);
//		DrawQuad (new Rect(x+w,y,lineWidth,l),rectColor);
//		DrawQuad (new Rect(x,y+l,w,lineWidth),rectColor);
	}

	void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		//		GUI.Box(position,GUIContent.none, style);
		GUI.Box(position, GUIContent.none);
	}


}
