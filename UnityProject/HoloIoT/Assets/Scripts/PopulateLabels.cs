using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateLabels : MonoBehaviour {

    
    public Text header;
    public Text MinX;
    public Text MaxX;
    public Text MaxY;
    public Text MinY;
    private string header2;

	// Use this for initialization
	void Start () {
        MinX.text = "0";
        MaxX.text = "0";
        MaxY.text = "0";
        MinY.text = "0";

        // Header2 is used in the quick view case.
	}

	// Update is called once per frame
	void Update () {

        header.text = SQLConnect.GlobalString;

        for (int i = 0; i < SQLConnect.attributes.Length; i++)
        {
            if (header.text.ToLower() == SQLConnect.attributes[i])
            {
                MinY.text = SQLConnect.tableVals[(i * 8) + 5].ToString();
                MaxY.text = SQLConnect.tableVals[(i * 8) + 4].ToString();
                break;
            }
        }
        if (header.text == "All")
        {
            MinY.text = Mathf.Min(SQLConnect.tableVals[5], SQLConnect.tableVals[13]).ToString();
            MaxY.text = Mathf.Max(SQLConnect.tableVals[4], SQLConnect.tableVals[12]).ToString();
        }

        MinX.text = SQLConnect.times[0];
        MaxX.text = SQLConnect.times[1];
    }

}
