using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]

public class LineChart : Chart {

	public float mThickness = 0.015f; 
	public float mDepthPitch = 0.001f; 
	public static Color[] mBaseColor = new Color[]{Color.blue, Color.red, Color.green,
            Color.yellow, Color.magenta, Color.cyan, Color.gray}; 
	public float mMax, mMin; 
	public float alpha = 1.0f; 
	public bool Fill = false; 
	public bool Stacked = false; 
	public bool AutoRange = false; 

	private Mesh mesh;
	private MeshFilter meshFilter;
	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uvs;
	private Color[] mColor; 
	private float mWidth = 1.0f, mHeight = 1.0f; 


	LineChart() {
		int length = 32; 
		mData = new float[1][]; 
		mData[0] = new float[length]; 
		for(int i=0;i<mData[0].Length;i++) {
			mData[0][i] = 1.0f / (float)(i + 2.0f); 
		}
		mBaseColor = new Color[]{Color.blue, Color.red, Color.green, 
			Color.yellow, Color.magenta, Color.cyan, Color.gray}; 
	}



	protected override void CreateChartData() {
		meshFilter = (MeshFilter)GetComponent("MeshFilter");
		mesh = meshFilter.sharedMesh;//new Mesh();
		if(mesh == null) {
			mesh = new Mesh();
		}

		mesh.Clear();
		
		vertices = new Vector3[mData.Length * mData[0].Length * 2];
		mColor = new Color[mData.Length * mData[0].Length * 2];
		uvs = new Vector2[mData.Length * mData[0].Length * 2];
		if(AutoRange) {
			mMax = mMin = mData[0][0]; 
			for(int i=0;i<mData.Length;i++) {
				for(int j=0;j<mData[i].Length;j++) {
					if(mMax < mData[i][j]) {
						mMax = mData[i][j]; 
					}
					if(mMin > mData[i][j]) {
						mMin = mData[i][j]; 
					}
				}

			}
		}

		int imax = 0; 
		for(int i=0;i<mData.Length;i++) {
			if(imax < mData[i].Length) {
				imax = mData[i].Length; 
			}
		}

		float dur = mMax - mMin; 
		float[] sum = new float[imax]; 

		for(int j=0;j<imax;j++) {
			sum[j] = 0.0f; 
		}

		for(int i=0;i<mData.Length;i++) {
			for(int j=0;j<mData[i].Length;j++) {
				float t = (float)mWidth * (float)j / (float)(mData[i].Length-1); 
				float v = (mData[i][j] - mMin) / (mMax - mMin) * mHeight; 
				float v2 = v; 
				if(Stacked) {
					v += sum[j]; 
				}
				Vector3 tck; 
				if(j < mData[i].Length - 1) {
					tck = new Vector3((float)mWidth/(float)mData[i].Length, mData[i][j+1] - mData[i][j], 0); 
					tck = Vector3.Normalize(tck) * mThickness; 
				} else {
					tck = new Vector3((float)mWidth/(float)mData[i].Length, mData[i][j] - mData[i][j-1], 0); 
					tck = Vector3.Normalize(tck) * mThickness; 
				}
				if(Fill) {
					vertices[(i*mData[i].Length+j)*2+0] = new Vector3( t, sum[j], (mData.Length-1-i) * mDepthPitch );
					vertices[(i*mData[i].Length+j)*2+1] = new Vector3( t, v, (mData.Length-1-i) * mDepthPitch );
				} else {
//					vertices[(i*mData[i].Length+j)*2+0] = new Vector3( t + tck.y, v - tck.x, 0 );
//					vertices[(i*mData[i].Length+j)*2+1] = new Vector3( t - tck.y, v + tck.x, 0 );
					vertices[(i*mData[i].Length+j)*2+0] = new Vector3( t , v - mThickness*0.5f, (mData.Length-1-i) * mDepthPitch );
					vertices[(i*mData[i].Length+j)*2+1] = new Vector3( t , v + mThickness*0.5f, (mData.Length-1-i) * mDepthPitch );
				}
				mColor[(i*mData[i].Length+j)*2+0] = mBaseColor[i % mBaseColor.Length]; 
				mColor[(i*mData[i].Length+j)*2+1] = mBaseColor[i % mBaseColor.Length]; 
				mColor[(i*mData[i].Length+j)*2+0].a = alpha; 
				mColor[(i*mData[i].Length+j)*2+1].a = alpha; 
				uvs[(i*mData[i].Length+j)*2+0] = new Vector2((float)j / (float)mData[i].Length, 0.0f); 
				uvs[(i*mData[i].Length+j)*2+1] = new Vector2((float)j / (float)mData[i].Length, 1.0f); 

				if(Stacked) {
					sum[j] += v2; 
				}
			}
		}

		triangles = new int[3* 2 * mData.Length * (mData[0].Length-1)];
		for(int i=0;i<mData.Length;i++) {
			for(int j=0;j<mData[i].Length-1;j++) {
				triangles[(i*(mData[i].Length-1)+j)*6+0] = 0 + (i*mData[i].Length+j)*2;
				triangles[(i*(mData[i].Length-1)+j)*6+1] = 1 + (i*mData[i].Length+j)*2;
				triangles[(i*(mData[i].Length-1)+j)*6+2] = 2 + (i*mData[i].Length+j)*2;
				triangles[(i*(mData[i].Length-1)+j)*6+3] = 1 + (i*mData[i].Length+j)*2;
				triangles[(i*(mData[i].Length-1)+j)*6+4] = 3 + (i*mData[i].Length+j)*2;
				triangles[(i*(mData[i].Length-1)+j)*6+5] = 2 + (i*mData[i].Length+j)*2;
			}
		}

		if(mesh.vertices.Length != vertices.Length) {
			mesh.triangles = null;
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.colors = mColor; 
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		//mesh.Optimize();
		
		meshFilter.sharedMesh = mesh;
		meshFilter.sharedMesh.name = "LineChartMesh";	

	}
	
}
