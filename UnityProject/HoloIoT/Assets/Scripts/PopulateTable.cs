using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateTable : MonoBehaviour {

    public Text globalAvg;
    public Text globalMin;
    public Text globalMax;
    public Text deviceAvg;
    public Text deviceMax;
    public Text deviceMin;
    public Text deviceSTD;
    public Text globalSTD;
    public Text attr;
    public bool flag;
    public bool all;
    public int i;

    // Use this for initialization
    void Start()
    {

        globalAvg.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 3].ToString();
        globalMin.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 1].ToString();
        globalMax.text = SQLConnect.tableVals[(i * SQLConnect.tableStats)].ToString();
        deviceAvg.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 7].ToString();
        deviceMin.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 5].ToString();
        deviceMax.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 4].ToString();
        deviceSTD.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 6].ToString();
        globalSTD.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 2].ToString();

        flag = false;
        attr.text = SQLConnect.GlobalString;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("item is instantiated");
        if (!flag)
        {
            

            if (SQLConnect.tableVals[3] != 0 && SQLConnect.tableVals[7] != 0 && SQLConnect.tableVals[11] != 0 && SQLConnect.tableVals[15] != 0)
            {
                if (i != 1000)
                {
                    /*
                    Vector3 temp = Camera.main.transform.position + Camera.main.transform.forward * 3.0f;
                    temp.x += i * 0.5375f ;
                    transform.position = temp;
                    */
                    globalAvg.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 3].ToString();
                    globalMin.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 1].ToString();
                    globalMax.text = SQLConnect.tableVals[(i * SQLConnect.tableStats)].ToString();
                    deviceAvg.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 7].ToString();
                    deviceMin.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 5].ToString();
                    deviceMax.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 4].ToString();
                    deviceSTD.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 6].ToString();
                    globalSTD.text = SQLConnect.tableVals[(i * SQLConnect.tableStats) + 2].ToString();
                    attr.text = char.ToUpper(SQLConnect.attributes[i][0]) + SQLConnect.attributes[i].Substring(1);
                    flag = true;
                }
                
            }
        }


    }
}
