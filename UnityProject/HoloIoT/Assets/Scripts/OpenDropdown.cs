using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;



public class OpenDropdown : MonoBehaviour {

    private Dropdown drop;

    // Use this for initialization
    void Start () {
        drop = GetComponent<Dropdown>();
        drop.options.Clear();
    }


	// Update is called once per frame
	void Update () {

        // Do this until the dropdown menu is updated with anomalies
        if (drop.options.Count == 0)
        {
            drop.transform.Find("Label").gameObject.GetComponent<Text>().text = "";

            // Add the anomalies to the dropdown list
            foreach (string device in SQLConnect.anomDevices)
            {
                drop.options.Add(new Dropdown.OptionData(device));
            }

            // This will onoly happen once
            if (drop.options.Count != 0)
            {

                // Update the dropdown caption to be the number of anomalies detected
                if (drop.options[0].text != "0 Anomalies")
                {
                    drop.transform.Find("Label").gameObject.GetComponent<Text>().text = drop.options.Count.ToString() + " Anomalies";
                }
                else
                {
                    drop.transform.Find("Label").gameObject.GetComponent<Text>().text = drop.options[0].text;
                }
                // When anomaly detection is done, have the system alert the user to the number of anomalies detected
                TextToSpeechControl.Speech(drop.transform.Find("Label").gameObject.GetComponent<Text>().text.Split()[0] + " devices with anomalies");
            }
        }
	}
}
