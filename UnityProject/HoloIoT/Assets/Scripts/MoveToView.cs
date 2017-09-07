using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToView : MonoBehaviour {


    public static bool flag;

	// Use this for initialization
	void Start () {
        flag = false;
	}
	
	// Update is called once per frame
	void Update () {

        // Flag is updated when a scan is successful. If it is, update the position of the canvas containing
        // the graphs/charts so that it is right in front of the user. 
        if (flag)
        {
            Vector3 temp = Camera.main.transform.position + Camera.main.transform.forward * 3.0f;
            temp.y += 1f;
            transform.position = temp;
            flag = false;
        }
	}
}
