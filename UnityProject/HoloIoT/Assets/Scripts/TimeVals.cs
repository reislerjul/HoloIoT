using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class TimeVals : MonoBehaviour {


    public GameObject startHour;
    public GameObject startMinute;
    public GameObject startDay;
    public GameObject startMonth;
    public GameObject startYear;
    public GameObject endHour;
    public GameObject endMinute;
    public GameObject endDay;
    public GameObject endMonth;
    public GameObject endYear;
    private DateTime startVal;
    private DateTime endVal;
    public GameObject setRange;
    public GameObject ranges;
    public GameObject Canvas;
    public GameObject Canvas2;
    public GameObject controller;
    public Text txt;
    public GameObject keyboard;
    public GameObject[] arr;
    private Button[] keys;
    private int curr;
    private string tempString;
    private string prevString;
    private int counter;
    public GameObject dictationCheck;
    public GameObject dictation;
    public GameObject instructions;
    public GameObject panel;
    private Button[] panelButtons;
    private int toggle;
    private string days;
    public GameObject tagCanvas;
    public GameObject tap;
    public static bool tapmode;
    public GameObject drop;

    // Use this for initialization
    void Start() {

        curr = 1000;
        tempString = "";
        prevString = "";
        counter = 0;
        toggle = 40000;
        days = "";
        tapmode = false;

        // Get an array of the panel's buttons. Set all of them to false to start. 
        panelButtons = new Button[4];
        panelButtons = panel.GetComponentsInChildren<Button>();
        for (int j = 0; j < panelButtons.Length; j++)
        {
            panelButtons[j].gameObject.SetActive(false);
        }
        panelButtons[0].onClick.AddListener(() => PanelPlay());
        panelButtons[1].onClick.AddListener(() => PanelPause());
        panelButtons[2].onClick.AddListener(() => PanelEnter());
        panelButtons[3].onClick.AddListener(() => PanelExit());

        // The default mode is the keyboard
        dictationCheck.SetActive(false);
        panel.SetActive(false);
        tap.transform.Find("Check").gameObject.SetActive(false);

        // Set listeners for if any of the buttons is clicked. 
        arr = new GameObject[] { startHour, startMinute, startDay, startMonth, startYear, endHour, endMinute, endDay, endMonth, endYear };

        for (int i = 0; i < arr.Length; i++)
        {
            Button b = arr[i].GetComponent<Button>();
            int tempInt = i;
            b.onClick.AddListener(() => ButtonClicked(tempInt));
        }

        // Create a button handler for the dictation button
        Button c = dictation.GetComponent<Button>();
        c.onClick.AddListener(() => ModeChange());

        // Create a button handler for the tap/hold button
        Button d = tap.GetComponent<Button>();
        d.onClick.AddListener(() => ModeChangeTap());

        // Get an array of the keyboard keys
        Button[] temp1 = keyboard.transform.Find("key1").gameObject.GetComponentsInChildren<Button>();
        Button[] temp2 = keyboard.transform.Find("key2").gameObject.GetComponentsInChildren<Button>();
        Button[] temp3 = keyboard.transform.Find("key3").gameObject.GetComponentsInChildren<Button>();
        Button[] temp4 = keyboard.transform.Find("key4").gameObject.GetComponentsInChildren<Button>();
        keys = new Button[temp1.Length + temp2.Length + temp3.Length + temp4.Length];
        temp1.CopyTo(keys, 0);
        temp2.CopyTo(keys, temp1.Length);
        temp3.CopyTo(keys, temp2.Length + temp1.Length);
        temp4.CopyTo(keys, temp3.Length + temp2.Length + temp1.Length);


        // Add listener for the keys        
        for (int k = 0; k < keys.Length; k++)
        {
            if (k != 9 && k < 11)
            {
                int tempInt = k;
                keys[k].onClick.AddListener(() => keyClicked(tempInt));
            }
        }

        keys[9].onClick.AddListener(() => XClicked(9));

        keys[11].onClick.AddListener(() => EnterClicked(11));

        // Set the keyboard to inactive when a button hasnt been pressed
        keyboard.SetActive(false);

        // Set this view inactive at first. 
        Canvas2.SetActive(false);

        startVal = Convert.ToDateTime(SQLConnect.start);
        endVal = Convert.ToDateTime(SQLConnect.end);

        // The button to enter the ranges screen is clicked. 
        Button but = ranges.GetComponent<Button>();
        but.onClick.AddListener(OpenRanges);

        // The button to leave the ranges screen is clicked. 
        Button tempButton = setRange.GetComponent<Button>();
        tempButton.onClick.AddListener(SetRange);

        // initialize the text for hours.
        startHour.GetComponent<Text>().text = startVal.Hour.ToString();
        endHour.GetComponent<Text>().text = endVal.Hour.ToString();

        // initialize the Text options for minutes
        startMinute.GetComponent<Text>().text = (startVal.Minute > 9) ? startVal.Minute.ToString(): "0" + startVal.Minute.ToString();
        endMinute.GetComponent<Text>().text = (endVal.Minute > 9) ? endVal.Minute.ToString(): "0" + endVal.Minute.ToString();

        // initialize the Text options for days
        startDay.GetComponent<Text>().text = startVal.Day.ToString();
        endDay.GetComponent<Text>().text = endVal.Day.ToString();

        // initialize the Text options for months
        startMonth.GetComponent<Text>().text = startVal.Month.ToString();
        endMonth.GetComponent<Text>().text = endVal.Month.ToString();

        // initialize the Text options for years
        startYear.GetComponent<Text>().text = startVal.Year.ToString();
        endYear.GetComponent<Text>().text = endVal.Year.ToString();


    }

    // If someone says yesterday, update the dates
    public void OnYesterday()
    {
        DateTime yesterday = DateTime.Today.AddDays(-1);
        Dates(yesterday);
    }

    // If someone says today, update the dates
    public void OnToday()
    {
        DateTime today = DateTime.Today;
        Dates(today);
    }

    public void Dates(DateTime date)
    {
        arr[2].GetComponent<Text>().text = date.Day.ToString();
        arr[3].GetComponent<Text>().text = date.Month.ToString();
        arr[4].GetComponent<Text>().text = date.Year.ToString();
        arr[7].GetComponent<Text>().text = date.Day.ToString();
        arr[8].GetComponent<Text>().text = date.Month.ToString();
        arr[9].GetComponent<Text>().text = date.Year.ToString();
    }

    public void ModeChangeTap()
    {
        EnableButtons();
        counter = 1;
        tap.transform.Find("Check").gameObject.SetActive(!tap.transform.Find("Check").gameObject.activeSelf);
        tapmode = !tapmode;
    }


    // Changes the mode between dictation mode and keyboard mode
    public void ModeChange()
    {
        EnableButtons();
        counter = 1;
        dictationCheck.SetActive(!dictationCheck.activeSelf);
        panel.SetActive(!panel.activeSelf);
        if (!panel.activeSelf)
        {
            panelButtons[0].gameObject.SetActive(false);
            panelButtons[1].gameObject.SetActive(false);
        }

        instructions.GetComponent<Text>().text = "Say \"Start Time\", \"Start Date\", \"End Time\", \"End Date\", or \"Up to Date\"";

        // If dictation mode is active, the time buttons should be disabled. 
        if (dictationCheck.activeSelf)
        {
            if (keyboard.activeSelf)
            {
                arr[curr].GetComponent<Text>().text = prevString;
                keyboard.SetActive(false);
            }
            foreach (GameObject obj in arr)
            {
                obj.GetComponent<Button>().interactable = true;
                obj.GetComponent<Button>().enabled = false;
            }

        }
        else
        {
            foreach (GameObject obj in arr)
            {
                obj.GetComponent<Button>().enabled = true;
            }
        }

    }

    // The following 5 functions are used to respond to possible voice input from the user.
    // The user will pick which option to change.
    public void StartTimeChange()
    {
        toggle = 0;
        TimeInstructions();

    }

    public void StartDateChange()
    {
        toggle = 1;
        DateInstructions();
    }

    public void EndDateChange()
    {
        toggle = 2;
        DateInstructions();
    }

    public void EndTimeChange()
    {
        toggle = 3;
        TimeInstructions();
    }

    public void OnUpToDate()
    {
        toggle = 4;
        if (dictationCheck.activeSelf)
        {
            instructions.GetComponent<Text>().text = "Press Play Button. Use format \"Last X Minutes\" or \"Last Hour\". Press Pause Button.";
        }
        PausePlay();
    }

    // The following two functions change the instructions based
    // on whether the user is choosing to edit a date or time. 
    void TimeInstructions()
    {
        if (dictationCheck.activeSelf)
        {
            instructions.GetComponent<Text>().text = "Press Play Button. Say time in format \"4:16 PM\" or \"1 o'clock AM\". Press Pause Button.";
        }
        PausePlay();
        
    }

    void DateInstructions()
    {
        if (dictationCheck.activeSelf)
        {
            instructions.GetComponent<Text>().text = "Press Play Button. Say date in format \"September 1st 1993\". Press Pause Button.";
        }
        PausePlay();
    }

    // Helper function for changing the state of panel buttons
    void ButtonActivator(int i, int j, bool t1, bool t2)
    {
        panelButtons[i].gameObject.SetActive(t1);
        panelButtons[j].gameObject.SetActive(t2);
    }

    // Enables the pause and play buttons on the panel
    void PausePlay()
    {
        ButtonActivator(0, 1, true, true);
    }

    // Enables the enter and exit buttons on the panel
    void SaveExit()
    {
        ButtonActivator(2, 3, true, true);
    }

    void PanelPlay()
    {
        panel.GetComponent<MicrophoneManager>().StartRecording();
        EnableButtons();
        counter = 1;

    }
    
    void PanelPause()
    {
        panel.GetComponent<MicrophoneManager>().StopRecording();
        SaveExit();
        ButtonActivator(0, 1, false, false);
        EnableButtons();
        counter = 1;
    }

    void PanelEnter()
    {
        EnableButtons();
        counter = 1;
        if (days != "")
        {
            string[] temp = days.Split(' ');
            if (toggle == 2)
            {
                endDay.GetComponent<Text>().text = temp[1];
                endMonth.GetComponent<Text>().text = temp[0];
                endYear.GetComponent<Text>().text = temp[2];
            }
            else if (toggle == 1)
            {
                startDay.GetComponent<Text>().text = temp[1];
                startMonth.GetComponent<Text>().text = temp[0];
                startYear.GetComponent<Text>().text = temp[2];
            }
            else if (toggle == 0)
            {
                startHour.GetComponent<Text>().text = temp[0];
                startMinute.GetComponent<Text>().text = temp[1];
            }
            else if (toggle == 3)
            {
                endHour.GetComponent<Text>().text = temp[0];
                endMinute.GetComponent<Text>().text = temp[1];
            }
            else if (toggle == 4)
            {
                DateTime endTime = DateTime.UtcNow;
                TimeSpan interval = temp[1].ToLower() == "hour" ? new TimeSpan(Convert.ToInt32(temp[0]), 0, 0) : new TimeSpan(0, Convert.ToInt32(temp[0]), 0);
                DateTime startTime = endTime.Subtract(interval);
                startHour.GetComponent<Text>().text = startTime.Hour.ToString();
                startMinute.GetComponent<Text>().text = startTime.Minute < 10 ? "0" + startTime.Minute.ToString() : startTime.Minute.ToString();
                startDay.GetComponent<Text>().text = startTime.Day.ToString();
                startMonth.GetComponent<Text>().text = startTime.Month.ToString();
                startYear.GetComponent<Text>().text = startTime.Year.ToString();
                endHour.GetComponent<Text>().text = endTime.Hour.ToString();
                endMinute.GetComponent<Text>().text = endTime.Minute < 10 ? "0" + endTime.Minute.ToString() : endTime.Minute.ToString();
                endDay.GetComponent<Text>().text = endTime.Day.ToString();
                endMonth.GetComponent<Text>().text = endTime.Month.ToString();
                endYear.GetComponent<Text>().text = endTime.Year.ToString();
            }
            days = "";
            ButtonActivator(0, 1, false, false);
            ButtonActivator(2, 3, false, false);
            instructions.GetComponent<Text>().text = "Say \"Start Time\", \"Start Date\", \"End Time\", \"End Date\", or \"Up to Date\"";
        }

    }

    void PanelExit()
    {
        EnableButtons();
        counter = 1;
        ButtonActivator(0, 1, false, false);
        ButtonActivator(2, 3, false, false);
        instructions.GetComponent<Text>().text = "Say \"Start Time\", \"Start Date\", \"End Time\", \"End Date\", or \"Up to Date\"";
    }

    // Disables the keys in the keyboard for a few frames. This is used so that the system
    // doesnt cause the button to be pressed more several times. 
    void EnableButtons()
    {
        foreach (Button b in keys)
        {
            b.enabled = !b.enabled;
        }
        dictation.GetComponent<Button>().enabled = !dictation.GetComponent<Button>().enabled;
        tap.GetComponent<Button>().enabled = !tap.GetComponent<Button>().enabled;
        foreach (Button c in panelButtons)
        {
            c.enabled = !c.enabled;
        }
    }



    // Removes the ordinals and the period from the date. 
    string RemoveOrdinals(string s)
    {

        DateTime tempDate;


        // Remove the period at the end of the phrase 
        if (s.Contains("."))
        {
            s = s.Substring(0, s.IndexOf("."));
        }

        // Check that the string can be split into 3 divided by ' ' 
        string[] date = s.Split(' ');
        if (date.Length != 3)
        {
            return "incorrect format";
        }
        

        // Remove the ordinal 
        if (date[1].Contains("th") || date[1].Contains("st") || date[1].Contains("nd") || date[1].Contains("rd"))
        {
            date[1] = date[1].Substring(0, date[1].Length - 2);
        }


        if (!DateTime.TryParseExact(date[0], "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None, out tempDate))
        {
            return "incorrect format";
        }

        return tempDate.Month + " " + date[1] + " " + date[2]; 
    }

    // Parse the datetimes from the user input
    string ParseTime(string s)
    {
        DateTime temptime;
        string temp;
        if (s.Contains("."))
        {
            s = s.Substring(0, s.IndexOf("."));
        }
        
        if (!DateTime.TryParse(s, out temptime))
        {
            return "incorrect format";
        }
        temp = temptime.Minute < 10 ? "0" + temptime.Minute.ToString() : temptime.Minute.ToString();

        return temptime.Hour.ToString() + " " + temp;
    }

    // Parses the "last X minutes"/"last hour" style phrases
    string ParseUpToDate(string s)
    {
        
        if (s.Contains("."))
        {
            s = s.Substring(0, s.IndexOf("."));
        }
        
        string[] date = s.Split(' ');

        if (date.Length != 3 && date.Length != 2)
        {
            
            return "incorrect format";
        }
        else if (date.Length == 3)
        {
            
            int temp;
            if (date[0].ToLower() != "last" || (date[2].ToLower() != "hours" && date[2].ToLower() != "minutes") || !int.TryParse(date[1], out temp)) {
                Debug.Log("caught in first if statement");
                Debug.Log(date[0].ToLower() != "last");
                Debug.Log((date[2].ToLower() != "hours" || date[2].ToLower() != "minutes"));
                Debug.Log(!int.TryParse(date[1], out temp));
                return "incorrect format";
            }
            return date[1] + " " + date[2].Substring(0, date[2].Length - 1).ToLower();

        }
        else if (date.Length == 2)
        {
            if (date[0].ToLower() != "last" || (date[1].ToLower() != "hour" && date[1].ToLower() != "minute"))
            {
                return "incorrect format";
            }
            return "1 " + date[1].ToLower(); 
        }
        return "incorrect format";

    }

    void Update()
    {

        // If Microphone Manager's sentence is not "", someone verbally entered a time or date.
        if (MicrophoneManager.sentence != "")
        {
            // Parse the string representing the time
            if (toggle == 0 || toggle == 3)
            {
                days = ParseTime(MicrophoneManager.sentence);
            }
            // User picked the up to date option. Parse the phrase
            else if (toggle == 4)
            {
                days = ParseUpToDate(MicrophoneManager.sentence);
            }
            // Parse the string representing the date
            else
            {

                days = RemoveOrdinals(MicrophoneManager.sentence);

            }
            if (days == "incorrect format")
            {
                instructions.GetComponent<Text>().text = "Incorrect date or time format.";
                days = "";
            }
            MicrophoneManager.sentence = "";
        }


        // Update the number that is shown visually as the value. 
        if (curr < arr.Length && !dictationCheck.activeSelf && tempString != "")
        {
            arr[curr].GetComponent<Text>().text = tempString;
        }

        if (counter > 0)
        {
            counter++;
            if (counter == 20)
            {
                EnableButtons();
                counter = 0;
            }
        }

    }

    void EnterClicked(int i)
    {
        if (tempString == "")
        {
            arr[curr].GetComponent<Text>().text = prevString;
        }
        else if (tempString.Length == 1)
        {
            arr[curr].GetComponent<Text>().text = "0" + arr[curr].GetComponent<Text>().text;
        }
        keyboard.SetActive(false);
        curr = 2000;
        EnableButtons();
        counter = 1;
        foreach (GameObject game in arr)
        {
            game.GetComponent<Button>().interactable = true;
        }
    }


    // If the X is clicked, set the value back to its previous value. 
    void XClicked(int i)
    {
        arr[curr].GetComponent<Text>().text = prevString;
        curr = 2000;
        keyboard.SetActive(false);
        EnableButtons();
        counter = 1;
        foreach (GameObject game in arr)
        {
            game.GetComponent<Button>().interactable = true;
        }
    }

    // One of the time values is clicked. Activate the keyboard and reset the variables.
    void ButtonClicked(int i)
    {
        // Only do this if the app is in keyboard mode. Otherwise, don't do anything. 
        if (!tapmode)
        {
            keyboard.SetActive(true);
            curr = i;
            tempString = "";
            prevString = arr[curr].GetComponent<Text>().text;
            EnableButtons();
            counter = 1;

            // While the keyboard is open, the buttons shouldnt be interactible. 
            foreach (GameObject game in arr)
            {
                game.GetComponent<Button>().interactable = false;
            }
        }
    }

    // A key is clicked (not the X or enter buttons). The tempString is updated with the number clicked. 
    void keyClicked(int k)
    {
        tempString += keys[k].gameObject.GetComponentInChildren<Text>().text;
        EnableButtons();
        counter = 1;
    }


    // The button to set the ranges is clicked. First, check that the range is valid. If it is, enable the other canvas
    // and update the query.
    public void SetRange()
    {
        // Check that the specified dates are actual dates. 
        try
        {
            // Convert the new values to datetimes
            DateTime newStart = new DateTime(Convert.ToInt32(startYear.GetComponent<Text>().text),
                Convert.ToInt32(startMonth.GetComponent<Text>().text), Convert.ToInt32(startDay.GetComponent<Text>().text),
                Convert.ToInt32(startHour.GetComponent<Text>().text), Convert.ToInt32(startMinute.GetComponent<Text>().text),
                0);

            DateTime newEnd = new DateTime(Convert.ToInt32(endYear.GetComponent<Text>().text),
                Convert.ToInt32(endMonth.GetComponent<Text>().text), Convert.ToInt32(endDay.GetComponent<Text>().text),
                Convert.ToInt32(endHour.GetComponent<Text>().text), Convert.ToInt32(endMinute.GetComponent<Text>().text),
                0);

            // Check for an invalid range. If valid, update SQLConnect start and end and run the query. 
            if ((newEnd.CompareTo(newStart) < 0) || (newEnd.CompareTo(DateTime.UtcNow) > 0))
            {
                txt.text = "Click to Set Range: Invalid Range";
            }
            else
            {
                Canvas2.SetActive(false);
                SQLConnect.start = newStart.ToString();
                SQLConnect.end = newEnd.ToString();
                Canvas.SetActive(true);
                tagCanvas.SetActive(true);
                drop.GetComponent<Dropdown>().options.Clear();
                drop.SetActive(false);
            }
        }
        catch
        {
            txt.text = "Click to Set Range: Invalid Date";
        }
    }
 
    // Open the ranges screen. Set the previous canvas inactive.
    public void OpenRanges()
    {
        Canvas.SetActive(false);
        tagCanvas.SetActive(false);
        Canvas2.SetActive(true);
    }


}
