﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
//#define Num 3

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;



public class ShapeRcognition2 : MonoBehaviour {
//	double resultSrc;
	int sampleNum = 3;
	public const int NUM = 3;

	string[] model = new string[NUM]{"01","09","10"};
	Texture2D[] modelTexture = new Texture2D[NUM] ;
	List<MatOfPoint>[] modelContours = new List<MatOfPoint>[NUM];
	Mat[] modelMat = new Mat[NUM];
	Mat[] Hierarchy = new Mat[NUM];
	double[] sample = new double[NUM];

	List<MatOfPoint> dstContours = new List<MatOfPoint> ();
	Texture2D dstTexture;
	Mat dstMat = new Mat();
	Mat dstHierarchy = new Mat();
	string newImage = "test01";

	double[] resultDst = new double[NUM];
	double threshold = 0.1;
	bool getNewImg = true;
	double[] matchPortion = new double[NUM];
	double[] diff = new double[NUM];
	double currentDifference = 1;
	double maxPortion = 0;
	int maxIndex;
	public int currentNearest = 0;
	public bool match;

	//color
	public static Color colorHSV;
	float h,s,v;

	// Use this for initialization
	void Start () {

		for(int i = 0; i < sampleNum;i++){
			modelTexture[i] = Resources.Load (model[i]) as Texture2D;
			modelContours[i] = new List<MatOfPoint>();
			modelMat [i] = new Mat ();
			Hierarchy [i] = new Mat ();
			sample [i] = 0;
		}

		for (int i = 0; i < sampleNum; i++) {
			loadContours (modelContours[i],modelTexture[i],modelMat[i],Hierarchy[i]);
			sample [i] = compareContours (modelContours[i],modelContours[i]);
			Debug.Log (modelContours [i].Count);
			Debug.Log (sample [i]);
		}
				
	}

	// Update is called once per frame
	void Update () {
//		Debug.Log("getImage");
//		Debug.Log(Capture.getImage);
		if(getNewImg){
			if (CaptureEHD.getImage) {
//			dstTexture = Resources.Load (newImage) as Texture2D;
				dstTexture = CaptureEHD.dstTexture;
//				dstTexture = CaptureEHD.dstTexture;
				colorHSV = dstTexture.GetPixel (dstTexture.width / 2, dstTexture.height / 2);
				Color.RGBToHSV(colorHSV,out h,out s, out v);
				colorHSV=Color.HSVToRGB(h, s, 1);
//				Debug.Log (h);
//				Debug.Log (s);
//				Debug.Log (colorHSV);
				loadContours (dstContours, dstTexture, dstMat, dstHierarchy);
				Debug.Log (dstContours.Count);
				for (int i = 0; i < sampleNum; i++) {
					resultDst [i] = compareContours (modelContours [i], dstContours);
//					Debug.Log (resultDst[i]);
					matchPortion [i] = resultDst [i] / sample [i];
//					Debug.Log ("matchPortion:");
//					Debug.Log (matchPortion [i]);
					double diff2 = Math.Abs (matchPortion [i]-1);
//					Debug.Log (diff2);
					diff [i] = Math.Abs( (resultDst [i] - sample [i])/sample[i]);
					Debug.Log (diff [i]);
					if (diff [i] < currentDifference) {
						currentDifference = diff [i];
						currentNearest = i;
					}

				}
				maxPortion = resultDst.Max ();
				maxIndex = resultDst.ToList ().IndexOf (maxPortion);
//			Debug.Log (maxIndex);

//			for (int i = 0; i < sampleNum; i++) {
//				if (i != maxIndex) {
//					matchPortion [i] = resultDst [i] / sample [i];
//					//				Debug.Log (matchPortion [i]);
//					diff [i] = Math.Abs (resultDst[i] - sample [i]);
////					Debug.Log (diff [i]);
//					if (diff [i] < currentDifference) {
//						currentDifference = diff [i];
//						currentNearest = i;
//					}
//				}
//			}

				getNewImg = false;
				match = true;
//				Debug.Log ("getImage");
			}
		}
//		Debug.Log ("currentNearest");
//		Debug.Log (currentNearest);

	}


	void loadContours(List<MatOfPoint> sContours,Texture2D sTexture,Mat sMat,Mat sHierarchy){
//		sTexture = Resources.Load (name) as Texture2D;
		sMat = new Mat (sTexture.height, sTexture.width, CvType.CV_8UC1);
		Utils.texture2DToMat (sTexture, sMat);
		Imgproc.threshold (sMat, sMat, 127, 255, Imgproc.THRESH_BINARY);
		Imgproc.findContours (sMat, sContours, sHierarchy, Imgproc.RETR_CCOMP, Imgproc.CHAIN_APPROX_NONE);
	}

	double compareContours(List<MatOfPoint> sContours,List<MatOfPoint> dContours){
	    double count;
		count = 0;
		double result = 0;
		for (int i = 0; i < sContours.Count; i++) {
			for (int j = 0; j < dContours.Count; j++) {
				double returnVal = Imgproc.matchShapes (sContours [i], dContours [j], Imgproc.CV_CONTOURS_MATCH_I1, 0);
				if (returnVal < threshold) {
					count ++;
				}
				result = count / (dContours.Count* sContours.Count);

			}
		}
		return result;
	}
		
}
