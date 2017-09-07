using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SendQuery : MonoBehaviour {

    public GameObject dataText;
    public GameObject NewQV;
    public GameObject placeholder;
    public GameObject graphView;
    private GameObject[] attr;
    private string start;
    private string end;
    private string ID;
    private bool flag;
    private int counter;
    public GameObject legend;
    private float timer;
    public GameObject rangesButton;
    public GameObject checklist;
    Button[] checkButtons;
    private int numIdentification = 2;
    private int numAttributes;
    private int numDisplays = 2;

    // Use this for initialization
    void Start () {
        checkButtons = checklist.transform.GetComponentsInChildren<Button>(); 
        timer = 2f;
        start = SQLConnect.start;
        end = SQLConnect.end;
        ID = SQLConnect.ID;
        attr = new GameObject[checkButtons.Length - numIdentification]; 
        flag = false;
        counter = 0;
        dataText.SetActive(false);
        legend.SetActive(false);
        
        // Adding 1 to account for all mode
        numAttributes = SQLConnect.attributes.Length + 1;

        // Handle the buttons for everything in the checklist except for scanner and TextScan. 
        for (int i = 0; i < checkButtons.Length - numIdentification; i++)
        {
            GameObject parent = checkButtons[i].gameObject; 
            attr[i] = parent;
            parent.transform.Find("Check").gameObject.SetActive(false);
            int tempint = i;
            checkButtons[i].onClick.AddListener(() => ButtonClicked(tempint));
          
        }

        // Set all displays to false
        graphView.SetActive(false);
        NewQV.GetComponent<InstantiateTables>().DestroyTables();

    }
	
	// Update is called once per frame
	void Update () {

        // Determine if live mode is checked. If so, SQLConnect's start and end time should be updated 
        // appropriately every, say, 7 seconds. 
        if (checklist.transform.Find("Historic").Find("Check").gameObject.activeSelf)
        {
            rangesButton.GetComponent<Button>().enabled = false;
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                TimeSpan interval = Convert.ToDateTime(SQLConnect.end) - Convert.ToDateTime(SQLConnect.start);
                SQLConnect.end = DateTime.UtcNow.ToString();
                SQLConnect.start = Convert.ToDateTime(SQLConnect.end).Subtract(interval).ToString();
                timer = 7f;
            }

        }
        else
        {
            rangesButton.GetComponent<Button>().enabled = true;
        }

        // If there aren't results from the query, set all the views inactive
        if (!SQLConnect.flag)
        {
            dataText.SetActive(true);
            graphView.SetActive(false);
            NewQV.GetComponent<InstantiateTables>().DestroyTables();

        }
        if (SQLConnect.flag)
        {
            dataText.SetActive(false);
        }
		
        // Only run the queries if the ranges or device ID change
        if (start != SQLConnect.start || end != SQLConnect.end || ID != SQLConnect.ID)
        {

            start = SQLConnect.start;
            end = SQLConnect.end;
            ID = SQLConnect.ID;

            // Only update the query if the ID is set to a valid ID
            if (SQLConnect.ID != "" && SQLConnect.ID != "not found")
            {
                placeholder.GetComponent<SQLConnect>().ResetData();

                // Runs the query based on whether live mode is on
                placeholder.GetComponent<SQLConnect>().RunQueries(checklist.transform.Find("Historic").Find("Check").gameObject.activeSelf);

            }
            flag = true;
        }

        // Goes through the different conditions for the view windows. 
        if (flag)
        {
            // Inactivate all the views. 
            graphView.SetActive(false);
            NewQV.GetComponent<InstantiateTables>().DestroyTables();
            legend.SetActive(false);
          
            bool attrCheck = false;
            bool displayCheck = false;

            // Check that one of the attributes is checked and one of the displays is checked
            for (int j = 0; j < numAttributes + numDisplays; j++)
            {
                if (j < numAttributes)
                {
                    if (attr[j].transform.Find("Check").gameObject.activeSelf)
                    {
                        attrCheck = true;
                    }
                }
                else
                {
                    if (attr[j].transform.Find("Check").gameObject.activeSelf)
                    {
                        displayCheck = true;
                    }
                }
            }

           
            // One of the Attributes is checked and one of the Displays is checked
            if (attrCheck && displayCheck)
            {
                // Find the attribute that is checked and update the Global String
                for (int i = 0; i < numAttributes; i++)
                {
                    if (attr[i].transform.Find("Check").gameObject.activeSelf)
                    {
                        SQLConnect.GlobalString = attr[i].transform.Find("Text").gameObject.GetComponent<Text>().text;
                        break;
                    }
                }

                // Find which display option is checked. Set its corresponding display to active if there is a deviceid
                if (SQLConnect.ID != "" && SQLConnect.ID != "not found")
                {
                    if (checklist.transform.Find("Table").Find("Check").gameObject.activeSelf)
                    {
                        NewQV.GetComponent<InstantiateTables>().DestroyTables();
                        NewQV.GetComponent<InstantiateTables>().InstantiateTable();
                    }
                    else if (checklist.transform.Find("Graph").Find("Check").gameObject.activeSelf && checklist.transform.Find("All").Find("Check").gameObject.activeSelf)
                    {
                        CameraWork.str = SQLConnect.GlobalString.ToLower();
                        CameraWork.flag = false;
                        graphView.SetActive(true);
                        legend.SetActive(true);
                    }
                    else if (checklist.transform.Find("Graph").Find("Check").gameObject.activeSelf)
                    {
                        CameraWork.str = SQLConnect.GlobalString.ToLower();
                        graphView.SetActive(true);
                        CameraWork.flag = false;
                    }
                }

            }

            flag = false;
        }
        
        // After a button is clicked, disable it for about 30 frames to prevent it from being reclicked. 
        if (counter != 0)
        {
            counter++;
            if (counter == 30)
            {
                EnableButtons();
                counter = 0;
            }
            
        }

	}

    // If any of the buttons is clicked, change the flag to true and run through the set of conditions
    void ButtonClicked(int i)
    { 
        // Set the check to its opposite state 
        attr[i].transform.Find("Check").gameObject.SetActive(!attr[i].transform.Find("Check").gameObject.activeSelf);

        // If the check is now true and its not the "live" check, set all the other attributes in this GameObject's group to false
        if (attr[i].transform.Find("Check").gameObject.activeSelf && i < numAttributes + numDisplays)
        {
            Uncheck(i);
        }

        if (i < numAttributes + numDisplays)
        {
            flag = true;
        }
        counter = 1;
        EnableButtons();

    }
    
    // Sets all checks in the group to false except for the ith
    void Uncheck(int i)
    {
        int start;
        int end;
        if (i < numAttributes)
        {
            start = 0;
            end = numAttributes;
        }
        else
        {
            start = numAttributes;
            end = numAttributes + numDisplays;
        }
        for (int k = start; k < end; k++)
        {
            if (k != i)
            {
                attr[k].transform.Find("Check").gameObject.SetActive(false);
            }
        }
    }

    // Change the enabled state of the buttons on the checklist 
    void EnableButtons()
    {

        for (int i = 0; i < attr.Length; i++)
        {

            attr[i].GetComponent<Button>().enabled = !attr[i].GetComponent<Button>().enabled;
        }

    }


    // The following functions are used for voice commands. Each one calls ButtonClicked using the 
    // integer index of the button in attr that corresponds to the voice command.

    public void TempVoice()
    {
        ButtonClicked(0);
    }

    public void HumidVoice()
    {
        ButtonClicked(1);
    }

    public void AllVoice()
    {
        ButtonClicked(2);
    }

    public void GraphVoice()
    {
        ButtonClicked(3);
    }

    public void TableVoice()
    {
        ButtonClicked(4);
    }

    public void LiveVoice()
    {
        ButtonClicked(5);
    }



}
