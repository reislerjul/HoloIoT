using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTag : MonoBehaviour {

    public GameObject Dtag;

	
	// Update is called once per frame
	void Update () {

        Dtag.GetComponent<Text>().text = "Device ID: " + SQLConnect.ID;

    }
}
