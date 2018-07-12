using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
//#define Num 3

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;

public class EHD : MonoBehaviour {
	
	Mat srcMat = new Mat();
	Mat srcEdgeMat = new Mat();
	double[] tan = new double[2000000];

	Mat dstMat = new Mat();
	Mat dstEdgeMat = new Mat();
	double[] tandst = new double[2000000];

	int[,] Gx = new int[3,3]{{1,0,-1},{2,0,-2},{1,0,-1}};
	int[,] Gy = new int[3,3]{{1,2,1},{0,0,0},{-1,-2,-1}};

	int[,] threshold = new int[2, 36] {
		 {
			-170,-160,-150,-140,-130,-120,-110,-100,-90,-80,-70,-60,-50,-40,-30,-20,-10,0,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180
		}, 
		{
			-180,-170,-160,-150,-140,-130,-120,-110,-100,-90,-80,-70,-60,-50,-40,-30,-20,-10,0,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170
		}
		};																									
	int[] count = new int[36];
	int[] count2 = new int[36];

	int standardWidth = 1024;
	int standardHeight = 1024;

	int sum01;
	int sum02;
	double m01;
	double m02;

	double ss01;
	double ss02;
	double N;

	double x01;
	double x02;
		
//	
	// Use this for initialization
	void Start () {
		Texture2D srcTexture = Resources.Load ("Rock") as Texture2D;
		srcMat = new Mat (srcTexture.height, srcTexture.width, CvType.CV_8UC1);
		Utils.texture2DToMat (srcTexture, srcMat); 
//		Debug.Log (srcMat.get(1024,1024)[0]);
		Imgproc.resize(srcMat,srcMat,new Size(standardWidth,standardHeight));
		Imgproc.cvtColor (srcMat, srcMat, Imgproc.COLOR_RGB2BGRA);
		Imgproc.GaussianBlur (srcMat, srcMat, new Size (5, 5), 1.4, 1.4);
		Imgproc.Canny (srcMat, srcEdgeMat, 50, 250);

		Texture2D dstTexture = Resources.Load ("Rock03") as Texture2D;
		dstMat = new Mat (dstTexture.height, dstTexture.width, CvType.CV_8UC1);
		Utils.texture2DToMat (dstTexture, dstMat); 
		//		Debug.Log (srcMat.get(1024,1024)[0]);
		Imgproc.resize(dstMat,dstMat,new Size(standardWidth,standardHeight));
		Imgproc.cvtColor (dstMat, dstMat, Imgproc.COLOR_RGB2BGRA);
		Imgproc.GaussianBlur (dstMat, dstMat, new Size (5, 5), 1.4, 1.4);
		Imgproc.Canny (dstMat, dstEdgeMat, 50, 250);

		int t = 0;

		for (int i = 0; i < standardWidth; i++) {
			for (int j = 0; j < standardHeight; j++) {
				if (srcEdgeMat.get (i, j) [0] != 0) {
					t++;
				}
			}
		}

//		Debug.Log (t);
		int rowLength = srcMat.rows();
		int colLength = srcMat.cols ();
		double sobelX = 0;
		double sobelY = 0;
		int k = 0;

		int rowLengthdst = dstMat.rows();
		int colLengthdst = dstMat.cols ();
		double dstSobelX = 0;
		double dstSobelY = 0;
		int k2 = 0;

		long distance = 0;
//		
//		double tan = 0;

		for (int i = 1; i < rowLength - 1; i++) {
			for (int j = 1; j < colLength - 1; j++) {
				if(srcEdgeMat.get(i,j)[0] != 0){
				sobelX = srcMat.get (i - 1, j - 1)[0] * Gx [0, 0] + srcMat.get (i - 1, j + 1)[0] * Gx [0, 2] +
						 srcMat.get (i, j)[0] * Gx [1, 1] + srcMat.get (i, j + 1)[0] * Gx [1, 2] +
						 srcMat.get (i + 1, j - 1)[0] * Gx [2, 0] + srcMat.get (i + 1, j + 1)[0] * Gx [2, 2];
				sobelY = srcMat.get (i - 1, j - 1)[0] * Gy [0, 0] + srcMat.get (i - 1, j)[0] * Gy [0, 1] + srcMat.get (i - 1, j + 1)[0] * Gy [0, 2] +
						 srcMat.get (i + 1, j - 1)[0] * Gy [2, 0] + srcMat.get (i + 1, j)[0] * Gy [2, 1] + srcMat.get (i + 1, j + 1)[0] * Gy [2, 2];
				if (sobelX == 0) {
					sobelX = 1e-6;
				}
				tan [k] = Math.Atan (sobelY/sobelX) * (180.0/3.14159);
				k++;
//				test = Math.Atan (sobelX/sobelY);
				}

			}
		}
		Debug.Log (k);

		for (int i = 1; i < rowLengthdst - 1; i++) {
			for (int j = 1; j < colLengthdst - 1; j++) {
				if(dstEdgeMat.get(i,j)[0] != 0){
					dstSobelX = dstMat.get (i - 1, j - 1)[0] * Gx [0, 0] + dstMat.get (i - 1, j + 1)[0] * Gx [0, 2] +
						dstMat.get (i, j)[0] * Gx [1, 1] + dstMat.get (i, j + 1)[0] * Gx [1, 2] +
						dstMat.get (i + 1, j - 1)[0] * Gx [2, 0] + dstMat.get (i + 1, j + 1)[0] * Gx [2, 2];
					dstSobelY = dstMat.get (i - 1, j - 1)[0] * Gy [0, 0] + dstMat.get (i - 1, j)[0] * Gy [0, 1] + dstMat.get (i - 1, j + 1)[0] * Gy [0, 2] +
						dstMat.get (i + 1, j - 1)[0] * Gy [2, 0] + dstMat.get (i + 1, j)[0] * Gy [2, 1] + dstMat.get (i + 1, j + 1)[0] * Gy [2, 2];
					if (dstSobelX == 0) {
						dstSobelX = 1e-6;
					}
					tandst [k2] = Math.Atan (dstSobelY/dstSobelX) * (180.0/3.14159);
					k2++;
					//				test = Math.Atan (sobelX/sobelY);
				}

			}
		}

//		Debug.Log (rowLengthdst);
		Debug.Log (k2);

		for (int i = 0; i < k; i++) {
			for (int j = 0; j < 36; j++) {
				if (tan [i] <= threshold [0, j] && tan [i] > threshold [1, j]) {
					count[j]++;
					}
				}
			}
			

		for (int i = 0; i < 36; i++) {
			if (count [i] != 0) {
//				Debug.Log ("K1:"+i+" "+"Count:"+count[i]);
				}
			}

		for (int i = 0; i < k2; i++) {
			for (int j = 0; j < 36; j++) {
				if (tandst [i] <= threshold [0, j] && tandst [i] > threshold [1, j]) {
					count2[j]++;
				}
			}
		}


		for (int i = 0; i < 36; i++) {
			if (count2 [i] != 0) {
//				Debug.Log ("K2:"+i+" "+"Count2:"+count2[i]);
			}
		}
			

		for (int i = 0; i < 36; i++) {
			sum01 += count [i];
			sum02 += count2 [i];
//			distance +=(count2 [i] - count [i]) * (count2 [i] - count [i]);
		}
		m01 = sum01 / 36;
		m02 = sum02 / 36;

		for (int i = 1; i < 36; i++) {
			ss01 += (count [i] - m01) * (count [i] - m01);
			ss02 += (count2 [i] - m02) * (count2 [i] - m02);
			N += ((count [i] - m01) - (count2 [i] - m02)) * ((count [i] - m01) - (count2 [i] - m02));

		}

		x01 = N / (ss01 + ss02);


		Debug.Log (x01);
//		Debug.Log (x02);

		}
		
		
	
	// Update is called once per frame
	void Update () {
		
	}
}
