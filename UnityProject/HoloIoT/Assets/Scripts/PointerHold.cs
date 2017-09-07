using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PointerHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool flag;
    private int counter;
    private string str;
    private int max;
    private int min;

    void Start()
    {
        flag = false;
        counter = 0;
        max = (this.gameObject.name) == "Hour" ? 23
            : (this.gameObject.name) == "Minute" ? 59
            : (this.gameObject.name) == "Day" ? 31
            : (this.gameObject.name) == "Month" ? 12
            : (this.gameObject.name) == "Year" ? (DateTime.Now.Year + 2)
            : 2000;
        min = (this.gameObject.name) == "Year" ? (DateTime.Now.Year - 2)
            : (this.gameObject.name) == "Day" || (this.gameObject.name) == "Month" ? 1
            : 0;
    }


    // Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        flag = true;
        str = GetComponent<Text>().text;
    }

    // Do this when the mouse is unclicked over the selectable object this script is attached to.
    public void OnPointerUp(PointerEventData eventData)
    {
        flag = false;
        counter = 0;
    }

    void Update()
    {
        // Only do these if the app is in tapmode
        if (TimeVals.tapmode)
        {
            if (flag)
            {
                Debug.Log("held");
                counter++;
                if (counter == 10)
                {
                    Debug.Log("Holding");
                    counter = 0;
                    int temp = Convert.ToInt32(GetComponent<Text>().text) + 1;
                    if (this.gameObject.name == "Minute")
                    {
                        temp = temp > max ? min : temp;
                        GetComponent<Text>().text = temp < 10 ? "0" + temp.ToString() : temp.ToString();
                    }
                    else
                    {
                        GetComponent<Text>().text = temp > max ? min.ToString() : temp.ToString();
                    }
                }
            }

        }

    }


}
