using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;


public class ImproHarris : MonoBehaviour {
	public const int NUM = 3;
	int sampleNum = 3;
	float cannyMin = 10;
	float cannyMax = 100;
	/// Detector parameters
	int blockSize = 2;
	int apertureSize = 3;
	double k = 0.04;

	//initialized src
	string[] srcName = new string[NUM]{"01","09","10"};
	Texture2D[] srcTexture = new Texture2D[NUM] ;
	Mat[] srcMat = new Mat[NUM];
	Mat[] src_gray_Mat = new Mat[NUM];
	Mat[] src_cannyEdge_Mat = new Mat[NUM];
	int[] CornerCount = new int[NUM];
	int[] CornerCount2 = new int[NUM];
	double[] result = new double[NUM];

	double[] portion = new double[NUM];
	double currentDifference = 100;
	int maxIndex;
	public int currentNearest = 0;

	Mat dstMat = new Mat();
	Mat dst_cannyEdge_Mat = new Mat();
	Texture2D dstTexture;
	int dstCornerCount = 0;
	int dstCornerCount2 = 0;
	double dstResult = 0;

	public bool match;
	bool getNewImg = true;

	Color[] colorHSV = new Color[3];
	public static Color COLORHSV;
	float H,S,V;
	//color range
	int colorRange = 3;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < sampleNum; i++) {
			
			srcMat [i] = new Mat ();
			src_gray_Mat [i] = new Mat ();
			src_cannyEdge_Mat [i] = new Mat ();

			srcTexture [i] = Resources.Load (srcName [i]) as Texture2D;
			srcMat [i] = new Mat (srcTexture [i].height, srcTexture [i].width, CvType.CV_8UC1);
			ProcessImg (srcTexture [i], srcMat [i], src_cannyEdge_Mat [i]);
			Debug.Log (srcMat [i].size ());
//			Debug.Log (src_gray_Mat[i].size());
			Debug.Log (src_cannyEdge_Mat [i].size ());
			CornerCount [i] = cornerHarris (src_cannyEdge_Mat [i], 150);
			CornerCount2 [i] = cornerHarris (src_cannyEdge_Mat [i], 100);

			Debug.Log ("1:    " + CornerCount [i]);
			Debug.Log ("2     " + CornerCount2 [i]);
			result [i] = (CornerCount [i] * 1.0f) / CornerCount2 [i];
			Debug.Log (result [i]);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (getNewImg) {
			if (CaptureEHD.getImage) {
				dstTexture = CaptureEHD.dstTexture;
				for (int i = 0; i < colorRange; i++) {
					colorHSV[i] = dstTexture.GetPixel (dstTexture.width / 2 + (dstTexture.width/800) * i * Convert.ToInt32(Math.Pow(-1,i)), dstTexture.height / 2 + (dstTexture.height / 800) * i * Convert.ToInt32(Math.Pow(-1,i)));
					Debug.Log (colorHSV[i]);
					COLORHSV += colorHSV [i];
				}
				COLORHSV = COLORHSV / colorRange;
				Color.RGBToHSV(COLORHSV,out H,out S, out V);
				COLORHSV = Color.HSVToRGB (H,S,1);
				dstMat = new Mat (dstTexture.height, dstTexture.width, CvType.CV_8UC1);
				ProcessImg (dstTexture,dstMat,dst_cannyEdge_Mat);
				Debug.Log (dstMat.size());
				Debug.Log (dst_cannyEdge_Mat.size());
				dstCornerCount = cornerHarris (dst_cannyEdge_Mat,150);
				dstCornerCount2 = cornerHarris (dst_cannyEdge_Mat,100);
				Debug.Log (dstCornerCount);
				Debug.Log (dstCornerCount2);
				dstResult = (dstCornerCount * 1.0f) / dstCornerCount2;
				Debug.Log (dstResult);

				for(int i = 0; i < sampleNum; i++){
					portion [i] = dstResult / result [i];
					portion [i] = Math.Abs (dstResult - result[i]);
					if (portion [i] < currentDifference) {
						currentDifference = portion [i];
						currentNearest = i;
					}
					Debug.Log(portion [i]);
				}
				Debug.Log(currentNearest);
				getNewImg = false;
				match = true;
			}
		}
		
	}

	void ProcessImg(Texture2D texture,Mat mat,Mat edge){
		Utils.texture2DToMat (texture, mat); 
		Imgproc.resize(mat,mat,new Size(400,400));
		Imgproc.cvtColor (mat, mat, Imgproc.COLOR_RGB2BGRA);
		Imgproc.Canny (mat, edge, 10, 125);
		Debug.Log ("Processed!");
	}

	int cornerHarris(Mat cannyEdges,int threshold){

		Mat corners = new Mat ();
		Mat tempDst = new Mat ();

		Imgproc.cornerHarris (cannyEdges, tempDst, blockSize, apertureSize, k);
		Mat tempDstNorm = new Mat();
		Core.normalize(tempDst, tempDstNorm, 0, 255, Core.NORM_MINMAX);
		Core.convertScaleAbs(tempDstNorm, corners);

		int count = 0;
//		int[,] countAdd = new int[ tempDstNorm.cols() , tempDstNorm.rows()];

		for (int i = 0; i < tempDstNorm.cols(); i++) {
			for (int j = 0; j < tempDstNorm.rows(); j++) {
				double[] value = tempDstNorm.get(j, i);
//				Debug.Log (value[0]);
				if (value[0] > threshold) {
//					Core.circle(corners, new Point(i, j), 5, new Scalar(r.nextInt(255), 2));
					count ++;
				}
			}
		}

		return count;
	}
}
