using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;

public class ImproEHD2 : MonoBehaviour {

	public const int NUM = 3;

	//standarlized
	int standardWidth = Screen.width/2-Screen.width/128;//here
	int standardHeight = Screen.width/2-Screen.width/128;
	int sampleNum = 3;

	//initialized src

	string[] srcName = new string[NUM]{"01","09","10"};
	Texture2D[] srcTexture = new Texture2D[NUM] ;
	Mat[] srcMat = new Mat[NUM];
	Mat[] srcEdgeMat = new Mat[NUM];
	int[] rowLength = new int[NUM];
	int[] colLength = new int[NUM];
	//	int[,] srcCount = new int[3,36];
	float[][] srcCount;

	//initialized dst
	Mat dstMat = new Mat();
	Mat dstEdgeMat = new Mat();
	Texture2D dstTexture;
	int dstRowLength = 0;
	int dstColLength = 0;
	float[] dstCount = new float[36];

	//match the image
	double[] result = new double[NUM];
	double currentDifference = 0;
	int maxIndex;
	public int currentNearest = 0;
	public bool match;

	//Modify
	double m_min,m_max;
	//Test dst
	//	string newImage = "CatusTest";

	//get image
	static public bool getNewImg = true;//modify
	//get image color
	Color[] colorHSV = new Color[3];
	public static Color COLORHSV;
	float H,S,V;
	//color range
	int colorRange = 3;

	//draw count
	public Transform cu;

	//Convolution
	int[,] Gx = new int[3,3]{{-1,0,1},{-2,0,2},{-1,0,1}};
	int[,] Gy = new int[3,3]{{-1,-2,-1},{0,0,0},{1,2,1}};

	//Range
	int range = 36;
	int[,] threshold = new int[2, 36] {
		{
			-170,-160,-150,-140,-130,-120,-110,-100,-90,-80,-70,-60,-50,-40,-30,-20,-10,0,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170,180
		}, 
		{
			-180,-170,-160,-150,-140,-130,-120,-110,-100,-90,-80,-70,-60,-50,-40,-30,-20,-10,0,10,20,30,40,50,60,70,80,90,100,110,120,130,140,150,160,170
		}
	};


	// Use this for initialization
	void Start () {

		m_max = 125;
		m_min = 10;

		srcCount = new float[NUM][];

		for (int i = 0; i < sampleNum; i++) {

			srcMat [i] = new Mat ();
			srcEdgeMat[i] = new Mat ();

			srcTexture[i] = Resources.Load (srcName[i]) as Texture2D;
			srcMat[i] = new Mat (srcTexture[i].height, srcTexture[i].width, CvType.CV_8UC1);
			ProcessImg (srcTexture [i], srcMat [i],srcEdgeMat[i],10,125);

			rowLength[i] = srcMat[i].rows();
			colLength[i] = srcMat[i].cols();
			srcCount[i] = Convolution(rowLength[i],colLength[i],srcMat[i],srcEdgeMat[i]);
		}

		//Test Compare dst to src
		//		dstTexture = Resources.Load (newImage) as Texture2D;
		//		dstMat = new Mat (dstTexture.height, dstTexture.width, CvType.CV_8UC1);
		//		ProcessImg (dstTexture,dstMat,dstEdgeMat);
		//
		//		dstRowLength = dstMat.rows();
		//		dstColLength = dstMat.cols();
		//		dstCount = Convolution(dstRowLength,dstColLength,dstMat,dstEdgeMat);
		//
		//		for(int i = 0; i < sampleNum; i++){
		//			result[i] = StandardizedEuclidean(srcCount[i],dstCount);
		//			if (result [i] < currentDifference) {
		//				currentDifference = result [i];
		//				currentNearest = i;
		//			}
		//		}
		//		Debug.Log(currentNearest);

		//Debug Convolution(Count) 
		//				for (int i = 0; i < range; i++) {
		////					Debug.Log (dstCount[i]);
		//					Debug.Log (srcCount[0][i]);
		//			//Vector3 cup = new Vector3 (i, 0f, 0f);
		//			Transform bar;
		//			bar = Instantiate (cu, new Vector3 (i*1.0f, 0f, 0f),Quaternion.identity);
		////			bar.localScale *= new Vector3 (srcCount[0][i],0,0);
		//			bar.localScale=new Vector3(bar.localScale.x, bar.localScale.y*srcCount [1] [i]*10f,bar.localScale.z);
		//				}

	}

	// Update is called once per frame
	void Update () {
		if(getNewImg){
			if (CaptureEHD2.getImage) {
				dstTexture = CaptureEHD2.dstTexture;
				for (int i = 0; i < colorRange; i++) {
					colorHSV[i] = dstTexture.GetPixel (dstTexture.width / 2 + (dstTexture.width/100) * i * Convert.ToInt32(Math.Pow(-1,i)), dstTexture.height / 2 + (dstTexture.height / 100) * i * Convert.ToInt32(Math.Pow(-1,i)));
					Debug.Log (colorHSV[i]);
					COLORHSV += colorHSV [i];
				}
				COLORHSV = COLORHSV / colorRange;
				Color.RGBToHSV(COLORHSV,out H,out S, out V);
				COLORHSV = Color.HSVToRGB (H,S,1);
				Debug.Log ("color:  "+COLORHSV);
				dstMat = new Mat (dstTexture.height, dstTexture.width, CvType.CV_8UC1);
				ProcessImg (dstTexture,dstMat,dstEdgeMat,m_min,m_max);

				Debug.Log (dstMat.size());
				for (int i = 0; i < 50; i++) {
					//					Debug.Log (dstMat.get(standardWidth/2+i,standardWidth/2+i)[0]);
					//					Debug.Log (dstEdgeMat.get(standardWidth/2+i,standardWidth/2+i)[0]);
				}

				dstRowLength = dstMat.rows();
				dstColLength = dstMat.cols();
				dstCount = Convolution(dstRowLength,dstColLength,dstMat,dstEdgeMat);

				//				for (int i = 0; i < range; i++) {
				//					Debug.Log (dstCount[i]);
				//					Transform bar;
				//					bar = Instantiate (cu, new Vector3 (i*1.0f, 0f, 0f),Quaternion.identity);
				//					//			bar.localScale *= new Vector3 (srcCount[0][i],0,0);
				//					bar.localScale=new Vector3(bar.localScale.x, bar.localScale.y*dstCount[i]*10f,bar.localScale.z);
				//				}

				currentDifference = StandardizedEuclidean(srcCount[0],dstCount);
				currentNearest = 0;

				for(int i = 0; i < sampleNum; i++){
					result[i] = StandardizedEuclidean(srcCount[i],dstCount);
					if (result [i] < currentDifference) {
						currentDifference = result [i];
						currentNearest = i;
					}
					Debug.Log(result[i]);
				}
				Debug.Log(currentNearest);
				getNewImg = false;
				match = true;
			}
		}
	}

	void ProcessImg(Texture2D texture,Mat mat,Mat edge,double min,double max){
		Utils.texture2DToMat (texture, mat); 
		//		Imgproc.resize(mat,mat,new Size(standardWidth,standardHeight));
		Imgproc.cvtColor (mat, mat, Imgproc.COLOR_RGB2BGRA);
		Imgproc.GaussianBlur (mat, mat, new Size (5, 5), 1.4, 1.4);
		Imgproc.Canny (mat, edge, min, max);
		Debug.Log ("Processed!");
		//		Debug.Log (dstMat.size());
		//		for (int i = 0; i < 50; i++) {
		//			Debug.Log (dstMat.get(512+i,512+i)[0]);
		//			Debug.Log (dstEdgeMat.get(512+i,512+i)[0]);
		//		}
	}


	float[] Convolution(int row,int col,Mat src,Mat edge){
		int k = 0;
		double X = 0;
		double Y = 0;
		double[] tan = new double[2000000];
		float[] count = new float[36];

		for (int i = 1; i < row - 1; i++) {
			for (int j = 1; j < col - 1; j++) {
				if(edge.get(i,j)[0] != 0){
					X = src.get (i - 1, j - 1)[0] * Gx [0, 0] + src.get (i - 1, j + 1)[0] * Gx [0, 2] +
						src.get (i, j-1)[0] * Gx [1, 1] + src.get (i, j + 1)[0] * Gx [1, 2] +
						src.get (i + 1, j - 1)[0] * Gx [2, 0] + src.get (i + 1, j + 1)[0] * Gx [2, 2];
					Y = src.get (i - 1, j - 1)[0] * Gy [0, 0] + src.get (i - 1, j)[0] * Gy [0, 1] + src.get (i - 1, j + 1)[0] * Gy [0, 2] +
						src.get (i + 1, j - 1)[0] * Gy [2, 0] + src.get (i + 1, j)[0] * Gy [2, 1] + src.get (i + 1, j + 1)[0] * Gy [2, 2];
					if (X == 0) {
						X = 1e-6;
					}
					tan [k] = Math.Atan (Y/X) * (180.0/3.14159);
					k++;
				}
			}
		}

		for (int i = 0; i < k; i++) {
			for (int j = 0; j < range; j++) {
				if (tan [i] <= threshold [0, j] && tan [i] > threshold [1, j]) {
					count[j]++;
					break;
				}
			}
		}

		for (int i = 0; i < 36; i++) {
			count [i] = count [i] / k;
		}
		return count;
	}

	double Euclidean(float[] srcCount,float[] dstCount){
		double E = 0;
		for(int i =0; i<range;i++){
			E += (srcCount [i] - dstCount [i]) * (srcCount [i] - dstCount [i]);
		}
		return E;
	}


	double StandardizedEuclidean(float[] srcCount,float[] dstCount){
		float srcSum = 0;
		float dstSum = 0;
		double result;

		for (int i = 0; i < range; i++) {
			srcSum += srcCount [i];
			dstSum += dstCount [i];
		}

		for (int i = 0; i < range; i++)
		{
			srcCount[i] /= srcSum;
			dstCount[i] /= dstSum;
		}
		result = Euclidean(srcCount, dstCount);
		/*
  srcM = srcSum/range;
  dstM = dstSum/range;
  for (int i = 0; i < range; i++) {
   srcSS += (srcCount [i] - srcM) * (srcCount [i] - srcM);
   dstSS += (dstCount [i] - dstM) *  (dstCount [i] - dstM);
   N += ((srcCount [i] - srcM) - (dstCount [i] - dstM)) * ((srcCount [i] - srcM) - (dstCount [i] - dstM));
  }

  result = N/(srcSS+dstSS);
  */
		return result;
	}

	public void Restart(){
		getNewImg = true;
	}

}
