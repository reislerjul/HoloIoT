using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateRange : MonoBehaviour {


    private string range;
    public Text rangeText;
	
	// Update is called once per frame
	void Update () {
        range = SQLConnect.start + " - " + SQLConnect.end;
        rangeText.text = "Range: " + range;
    }
}
