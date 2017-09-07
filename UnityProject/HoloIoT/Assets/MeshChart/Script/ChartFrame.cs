using UnityEngine;
using System.Collections;

public class ChartFrame : MonoBehaviour {

	public float mThickness = 0.01f; 
	public int row_max = 4; 
	public int col_max = 4; 
	public bool UpdateForEditor = false; 
	public Color LineColor = Color.white; 
	public float MarginLeft = 0.1f; 
	public float MarginRight = 0.1f; 
	public float MarginTop = 0.1f; 
	public float MarginBottom = 0.1f; 
	public float Alpha = 1.0f; 

	private Mesh meshFrame;
	private MeshFilter meshFilterFrame;
	private Vector3[] verticesFrame;
	private int[] trianglesFrame;
	private Vector2[] uvsFrame;
	private float mWidth = 1.0f, mHeight = 1.0f; 
	private Color[] mColorFrame; 
	private bool mAlreadyUpdated = false; 

	
	// Use this for initialization
	void Start () {
		initParameter(); 
		CreateChartData(); 
		
	}
	

	void initParameter()
	{
	}

	void UpdateData(int row, int col) 
	{
		row_max = row; 
		col_max = col; 
		mAlreadyUpdated = true; 
	}




	
	void CreateChartData() {
		meshFilterFrame = (MeshFilter)GetComponent("MeshFilter");
		meshFrame = meshFilterFrame.sharedMesh;//new Mesh();
		if(meshFrame == null) {
			meshFrame = new Mesh();
		}

		meshFrame.Clear();
		
		verticesFrame = new Vector3[4 * (col_max+1) + 4 * (row_max+1)];
		mColorFrame = new Color[4 * (col_max+1) + 4 * (row_max+1)];
		uvsFrame = new Vector2[4 * (col_max+1) + 4 * (row_max+1)];
		trianglesFrame = new int[6 * (col_max+1) + 6 * (row_max+1)];
		for(int i=0;i<col_max+1;i++) {
			int offset = i*4; 
			int offset2 = i*6; 
			verticesFrame[offset+0] = new Vector3(-MarginLeft, i * mWidth / (float)col_max - mThickness, 0);  
			verticesFrame[offset+1] = new Vector3(-MarginLeft, i * mWidth / (float)col_max + mThickness, 0);  
			verticesFrame[offset+2] = new Vector3( 1.0f+MarginRight, i * mWidth / (float)col_max - mThickness, 0);  
			verticesFrame[offset+3] = new Vector3( 1.0f+MarginRight, i * mWidth / (float)col_max + mThickness, 0);  
			uvsFrame[offset+0] = new Vector2(0, 0); 
			uvsFrame[offset+1] = new Vector2(0, 1); 
			uvsFrame[offset+2] = new Vector2(1, 0); 
			uvsFrame[offset+3] = new Vector2(1, 1); 
			trianglesFrame[offset2+0] = offset + 0; 
			trianglesFrame[offset2+1] = offset + 1; 
			trianglesFrame[offset2+2] = offset + 2; 
			trianglesFrame[offset2+3] = offset + 1; 
			trianglesFrame[offset2+4] = offset + 3; 
			trianglesFrame[offset2+5] = offset + 2; 
			Color lcl = LineColor; 
			if(i != 0) {
				lcl.a = 0.5f; 
			}
			lcl.a *= Alpha; 
			for(int j=0;j<4;j++) {
				mColorFrame[offset + j] = lcl; 
			}
		}
		for(int i=0;i<row_max+1;i++) {
			int offset = 4*(col_max+1) + i * 4; 
			int offset2 = 6*(col_max+1) + i*6; 
			verticesFrame[offset+0] = new Vector3(i * mHeight / (float)row_max - mThickness, -MarginBottom, 0);  
			verticesFrame[offset+1] = new Vector3(i * mHeight / (float)row_max + mThickness, -MarginBottom, 0);  
			verticesFrame[offset+2] = new Vector3(i * mHeight / (float)row_max - mThickness, 1.0f+MarginTop, 0);  
			verticesFrame[offset+3] = new Vector3(i * mHeight / (float)row_max + mThickness, 1.0f+MarginTop, 0);  
			uvsFrame[offset+0] = new Vector2(0, 0); 
			uvsFrame[offset+1] = new Vector2(0, 1); 
			uvsFrame[offset+2] = new Vector2(1, 0); 
			uvsFrame[offset+3] = new Vector2(1, 1); 
			trianglesFrame[offset2+0] = offset + 0; 
			trianglesFrame[offset2+1] = offset + 2; 
			trianglesFrame[offset2+2] = offset + 1; 
			trianglesFrame[offset2+3] = offset + 1; 
			trianglesFrame[offset2+4] = offset + 2; 
			trianglesFrame[offset2+5] = offset + 3; 
			Color lcl = LineColor; 
			if(i != 0) {
				lcl.a = 0.5f; 
			}
			lcl.a *= Alpha; 
			for(int j=0;j<4;j++) {
				mColorFrame[offset + j] = lcl; 
			}
		}
		
		if(meshFrame.vertices.Length != verticesFrame.Length) {
			meshFrame.triangles = null;
		}

		meshFrame.vertices = verticesFrame;
		meshFrame.triangles = trianglesFrame;
		meshFrame.uv = uvsFrame;
		meshFrame.colors = mColorFrame; 
		
		meshFrame.RecalculateNormals();
		meshFrame.RecalculateBounds();
		//mesh.Optimize();
		
		meshFilterFrame.sharedMesh = meshFrame;
		meshFilterFrame.sharedMesh.name = "ChartFrameMesh";	
		
	}

	// Update is called once per frame
	void Update () {
		if(mAlreadyUpdated) {
			mAlreadyUpdated = false; 
		} else if(UpdateForEditor) {
			CreateChartData(); 
		}
		
	}
}
