using UnityEngine;
using System.Collections;

public class CameraWork : MonoBehaviour {
	private float t; 

	private float[][] data;
    public LineChart _LineChart;
    public static string str = "";


    public static bool flag;



	// Use this for initialization
	void Start () {

        flag = true;

    }


    void Update()
    {
        if (!flag)
        {
            if (SQLConnect.graphVals[SQLConnect.attributes[0]] != null && SQLConnect.graphVals[SQLConnect.attributes[1]] != null)
            {
                if (str != "all")
                {
                    data = new float[1][];
                    data[0] = SQLConnect.graphVals[str];
                }
                else
                {
                    data = new float[SQLConnect.attributes.Length][];
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = SQLConnect.graphVals[SQLConnect.attributes[i]];
                    }
                }
                if (_LineChart != null && str != "") 
                {
                    _LineChart.UpdateData(data);
                }

                flag = true;
            }
        }

    }


}
