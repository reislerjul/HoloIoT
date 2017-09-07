using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateLegend : MonoBehaviour {

    public GameObject LegendItem;

	// Use this for initialization. Populate the legend for all mode.
	void Start () {
		for (int i = 0; i < SQLConnect.attributes.Length; i++)
        {
            GameObject temp = Instantiate(LegendItem, transform);
            temp.transform.Find("Image").gameObject.GetComponent<Image>().color = LineChart.mBaseColor[i];
            temp.transform.Find("Text").gameObject.GetComponent<Text>().text = SQLConnect.attributes[i].Substring(0, 1).ToUpper() + SQLConnect.attributes[i].Substring(1, 2);
        }
	}
	

}
