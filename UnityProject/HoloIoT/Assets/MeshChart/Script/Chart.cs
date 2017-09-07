using UnityEngine;
using System.Collections;

public abstract class Chart : MonoBehaviour {

	public bool UpdateForEditor = false; 

	protected float[][] mData; 
	private bool mAlreadyUpdated = false; 

	public void UpdateData(float[] data) {
		if(data.Length == 0) {
			Debug.LogError ("data is not set. "); 
			return; 
		}

		mData = new float[1][]; 
		mData[0] = new float[data.Length]; 
		data.CopyTo(mData[0], 0); 
		CreateChartData(); 
		mAlreadyUpdated = true; 
	}
	
	public void UpdateData(float[][] data) {
		if(data.Length == 0) {
			Debug.LogError ("data is not set. "); 
			return; 
		}
		if(data[0].Length == 0) {
			Debug.LogError ("data is not set. "); 
			return; 
		}
		int length = data[0].Length; 
		for(int i=1;i<data.Length;i++) {
			if(length != data[i].Length) {
				Debug.LogError ("data[X].Length is not same for all X. "); 
				return; 
			}
		}

		mData = new float[data.Length][];
		for(int i=0;i<mData.Length;i++) {
			mData[i] = new float[data[i].Length]; 
			data[i].CopyTo(mData[i], 0); 
		}
		CreateChartData(); 
		mAlreadyUpdated = true; 
	}

	// Use this for initialization
	void Start () {
		CreateChartData(); 
	}

	// Update is called once per frame
	void Update () {
		if(mData.Length == 0) {
			Debug.LogError ("data is not set. "); 
			return; 
		}
		if(mData[0].Length == 0) {
			Debug.LogError ("data is not set. "); 
			return; 
		}
		if(mAlreadyUpdated) {
			mAlreadyUpdated = false; 
		} else if(UpdateForEditor) {
			CreateChartData(); 
		}
	}

	abstract protected void CreateChartData(); 
}
