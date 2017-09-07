using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateTables : MonoBehaviour {


    public GameObject allTablePrefab;
    public GameObject tablePrefab;
    private GameObject scrollview;
    private GameObject content;

    public void Start()
    {
        scrollview = transform.Find("Scroll View").gameObject;
        content = transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;
        scrollview.SetActive(false);
    }

    public void DestroyTables()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in transform)
        {
            if (child != scrollview.transform)
            {
                Destroy(child.gameObject);
            }
        }

        scrollview.SetActive(false);
    }
    


    public void InstantiateTable()
    {
    
        if (SQLConnect.GlobalString != "All")
        {
            for (int i = 0; i < SQLConnect.attributes.Length; i++)
            {
                if (SQLConnect.GlobalString.ToLower() == SQLConnect.attributes[i])
                {
                    Instance(i, false);
                    break;
                }
            }
        }
        else
        {
            scrollview.SetActive(true);
            for (int i = 0; i < SQLConnect.attributes.Length; i++)
            {
                Instance(i, true);
            }
        }
    }

    private void Instance(int j, bool t)
    {
        GameObject temp;

        // if all mode is on, put the tables in a viewport
        if (t)
        {
            temp = Instantiate(allTablePrefab, content.transform);
        }
        else
        {
            temp = Instantiate(tablePrefab, transform);
        }
        temp.GetComponent<PopulateTable>().i = j;
        temp.GetComponent<PopulateTable>().flag = false;
    }


}
