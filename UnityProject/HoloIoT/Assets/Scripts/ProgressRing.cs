using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressRing : MonoBehaviour {

    Image ring;
    public static bool flag;
    public static bool flag2;
    public static bool flag3;

	// Use this for initialization
	void Start () {
        ring = GetComponent<Image>();
        ring.fillAmount = 0.0f;
        flag = false;
        flag2 = false;
        flag3 = false;

    }
	
	// Update is called once per frame
	void Update () {


        if ((flag && flag2) || flag3)
        {

            gameObject.SetActive(true);
            if (ring.fillAmount < 1.0f)
            {
                ring.fillAmount += 0.01f;
            }
            else
            {
                ring.fillAmount = 0.0f;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }


	}

    public void ResetFill()
    {
        ring.fillAmount = 0.0f;
    }

}
