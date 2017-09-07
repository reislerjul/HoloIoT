using UnityEngine;
using System.Collections;

public class BarChart : Chart {

	public float mBarWidth = 0.2f; 
	public Color[] mBaseColor; 
	public float MaxValue = 1.0f, MinValue = 0.0f; 
	public bool AutoRange = true; 
	public float alpha = 1.0f; 
	public bool Stacked = true; 
	public float mDepthPitch = 0.001f; 

	
	private float mWidth = 1.0f, mHeight = 1.0f; 
	private Mesh mesh;
	private MeshFilter meshFilter;
	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uvs;
	private Color[] mColor; 


	
	BarChart() {
		int length = 8; 
		mData = new float[2][]; 
		mData[0] = new float[length]; 
		for(int j=0;j<mData[0].Length;j++) {
			mData[0][j] = mData[0].Length / (float)(j + 2.0f); 
		}
		mData[1] = new float[length]; 
		for(int j=0;j<mData[1].Length;j++) {
			mData[1][j] = (float)j / (float)mData[1].Length; 
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
		
		vertices = new Vector3[mData.Length * mData[0].Length * 4];
		mColor = new Color[mData.Length * mData[0].Length * 4];
		uvs = new Vector2[mData.Length * mData[0].Length * 4];
		triangles = new int[3* 2 * mData.Length * (mData[0].Length)];

		if(AutoRange) {
			MaxValue=0;
			MinValue=0; 
			if(mData.Length > 0) {
				if(mData[0].Length > 0) {
					MaxValue = MinValue = mData[0][0]; 
				}
			}
			for(int i=0;i<mData.Length;i++) {
				for(int j=0;j<mData[i].Length;j++) {
					if(MaxValue < mData[i][j]) {
						MaxValue = mData[i][j]; 
					}
					if(MinValue > mData[i][j]) {
						MinValue = mData[i][j]; 
					}
				}
				
			}
			float dur = MaxValue - MinValue; 
		}
		
		for(int j=0;j<mData[0].Length;j++) {
			float sum = MinValue; 
			for(int i=0;i<mData.Length;i++) {
				float bw2 = (float)mWidth / (float)mData[i].Length * mBarWidth * 0.5f; 
				float t = (float)mWidth * (float)(j + 1) / (float)mData[i].Length; 
				float v;
				float vb; 
				float xoffset = 0.0f; 
				if(Stacked) {
					v = (mData[i][j] + sum - MinValue) / (MaxValue - MinValue) * mHeight; 
					vb = (sum - MinValue) / (MaxValue - MinValue) * mHeight; 
				} else {
					v = (mData[i][j] - MinValue) / (MaxValue - MinValue) * mHeight; 
					vb = 0.0f; 
					xoffset = i * bw2*2.0f; 
				}

				vertices[(i*mData[i].Length+j)*4+0] = new Vector3( xoffset + t - bw2, vb, mDepthPitch * (-i-1) );
				vertices[(i*mData[i].Length+j)*4+1] = new Vector3( xoffset + t + bw2, vb, mDepthPitch * (-i-1) );
				vertices[(i*mData[i].Length+j)*4+2] = new Vector3( xoffset + t - bw2, v, mDepthPitch * (-i-1) );
				vertices[(i*mData[i].Length+j)*4+3] = new Vector3( xoffset + t + bw2, v, mDepthPitch * (-i-1) );
				for(int k=0;k<4;k++) {
					mColor[(i*mData[i].Length+j)*4+k] = mBaseColor[i]; 
					mColor[(i*mData[i].Length+j)*4+k].a = alpha; 
				}
				uvs[(i*mData[i].Length+j)*4+0] = new Vector2(0.0f, 0.0f); 
				uvs[(i*mData[i].Length+j)*4+1] = new Vector2(1.0f, 0.0f); 
				uvs[(i*mData[i].Length+j)*4+2] = new Vector2(0.0f, 1.0f); 
				uvs[(i*mData[i].Length+j)*4+3] = new Vector2(1.0f, 1.0f); 


				if(v > vb) {
					triangles[(i*(mData[i].Length)+j)*6+0] = 0 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+1] = 2 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+2] = 1 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+3] = 1 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+4] = 2 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+5] = 3 + (i*mData[i].Length+j)*4;
				} else {
					triangles[(i*(mData[i].Length)+j)*6+0] = 0 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+1] = 1 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+2] = 2 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+3] = 1 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+4] = 3 + (i*mData[i].Length+j)*4;
					triangles[(i*(mData[i].Length)+j)*6+5] = 2 + (i*mData[i].Length+j)*4;
				}

				sum += mData[i][j]; 
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
		meshFilter.sharedMesh.name = "BarChartMesh";

		vertices = null; 
		mColor = null; 
		uvs = null; 
		triangles = null; 
		mesh = null;
		meshFilter = null; 
	}
	
}
