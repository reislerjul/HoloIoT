using UnityEngine;
using System;
using System.Collections.Generic;
using Unity3dAzure.AppServices;
using System.Linq;
using UnityEngine.UI;
using HoloToolkit.Unity;



public class SQLConnect : MonoBehaviour
{

    [Header("Azure App Service")]
    // Azure Mobile App connection strings
    [SerializeField]
    private string _appUrl = "PASTE_YOUR_APP_URL";

    // App Service Rest Client
    private MobileServiceClient _client;

    // App Service Table defined using a DataModel
    private MobileServiceTable<Telemetry> _table;
    private MobileServiceTable<Anomaly> _anomalies;

    // Global string keeps track of which attribute is being displayed
    public static string GlobalString = "";

    // The device ID
    public static string ID;

    // The default start and end times for the data query 
    public static string end;
    public static string start;

    public GameObject ring;
    public GameObject drop;

    // For a single attribute, there are 8 statistics to calculate. Minimum, maximum, average, 
    // standard deviation for both the device and globally. 
    public static int tableStats = 8;

    // attributes contains the names of the attributes. Developers should change this for their 
    // specific IoT scenario. tableVals and graphVals contains the data to populate the tables/charts.
    // telGlobal and graphVals contain the telemetry for each attribute, which can then be accessed to calculate
    // values to go into the table
    public static string[] attributes = new string[2] { "temperature", "humidity" };
    public static float[] tableVals;
    public static Dictionary<string, float[]> graphVals;
    private static Dictionary<string, float[]> telGlobal;

    // True if there are results from the query, false otherwise 
    public static bool flag;

    // Contains the telemetry list
    private List<Telemetry> global;
    private uint globalSkip;
    private List<Telemetry> device;
    private uint deviceSkip;
    private List<Anomaly> anoms;
    private uint anomSkip;
    public static List<string> anomDevices;

    // Start and end time for the graph label
    public static string[] times;

    // Parameters for the query
    private string filter;
    private string filter2;
    private string select;
    string filt;
    string sel;

    // Use this for initialization
    void Start () {
        drop.SetActive(false);
        anoms = new List<Anomaly>();
        anomDevices = new List<string>();

        // Create App Service client
        _client = new MobileServiceClient(_appUrl);

        // Get App Service 'Telemetry' table
        _table = _client.GetTable<Telemetry>("Telemetry");
        _anomalies = _client.GetTable<Anomaly>("Telemetry");

        anomSkip = 0;
        ID = "";
        end = DateTime.UtcNow.ToString();
        start = DateTime.UtcNow.Subtract(new TimeSpan(0, 2, 0)).ToString();
        times = new string[2] {" ", " "};
        tableVals = new float[attributes.Length * tableStats];
        graphVals = new Dictionary<string, float[]>();
        telGlobal = new Dictionary<string, float[]>();
        foreach (string attribute in attributes)
        {
            graphVals.Add(attribute, null);
            telGlobal.Add(attribute, null);
        }
        ResetData(); 
    }

    // Resets the data that's being queried 
    public void ResetData()
    {
        for (int i = 0; i < tableVals.Length; i++)
        {
            tableVals[i] = 0f;
        }
        foreach (string attribute in attributes)
        {
            graphVals[attribute] = null;
            telGlobal[attribute] = null;
        }
        flag = true;
        times = new string[2] {" ", " "};
        global = new List<Telemetry>();
        globalSkip = 0;
        device = new List<Telemetry>();
        deviceSkip = 0;
        filter = "";
        filter2 = "";
        select = "";
    }


    // Use this to run the queries for anomaly detection
    public void AnomalyDetection()
    {
        ring.GetComponent<ProgressRing>().ResetFill();
        ring.SetActive(true);
        ProgressRing.flag3 = true;
        drop.SetActive(true);
        drop.GetComponent<Dropdown>().options.Clear();
        anoms.Clear();
        anomDevices.Clear();
        anomSkip = 0;
        filt = string.Format("eventprocessedutctime ge '{0}' and eventprocessedutctime le '{1}' and prediction eq 1",
            start, end);
        sel = "deviceid";
        CustomQuery anomalyQuery = new CustomQuery(filt, "", 50, anomSkip, sel);
        StartCoroutine(_anomalies.Query<Anomaly>(anomalyQuery, AnomalyQuery));
    }

    private void AnomalyQuery(IRestResponse<NestedResults<Anomaly>> response)
    {
        if (!response.IsError)
        {
            Anomaly[] items = response.Data.results;
            if (anomSkip != 0)
            {
                anoms.AddRange(items.ToList());
            }
            else
            {
                anoms = items.ToList(); // set for first page of results
            }

            anomSkip += 50;
            if (anomSkip < response.Data.count)
            {
                CustomQuery anomalyQuery = new CustomQuery(filt, "", 50, anomSkip, sel);
                StartCoroutine(_anomalies.Query<Anomaly>(anomalyQuery, AnomalyQuery));
            }
            else
            {
                ProgressRing.flag3 = false;
                if (anoms.Count() != 0)
                {
                    anomDevices = (from element in anoms select element.deviceid).Distinct().ToList<string>();
                }
                else
                {
                    anomDevices.Add("0 Anomalies");
                }
            }
        }
        else
        {
            
            Debug.LogWarning("Read Nested Results Error Status:" + response.StatusCode.ToString() + " Url: " + response.Url);
        }
    }

    // Runs the queries. Accepts a boolean which represents whether live mode is on.  
    public void RunQueries(bool live)
    {
        ResetData();
        ring.GetComponent<ProgressRing>().ResetFill();
        ring.SetActive(true);
        ProgressRing.flag = true;
        ProgressRing.flag2 = true;

        // Check if up-to-date data is needed. Otherwise, include an end time for query results. 
        if (live)
        {
            filter = string.Format("eventprocessedutctime ge '{0}' and deviceid eq '{1}'",
                start, ID);
            filter2 = string.Format("eventprocessedutctime ge '{0}'",
                start);
        }
        else
        {
            filter = string.Format("eventprocessedutctime ge '{0}' and eventprocessedutctime le '{1}' and deviceid eq '{2}'",
                start, end, ID);
            filter2 = string.Format("eventprocessedutctime ge '{0}' and eventprocessedutctime le '{1}'",
                start, end);
        }

        
        foreach (string element in attributes)
        {
            select += element + ", ";
        }

        // This will change based on what the variable for the time element of the telemetry is called
        select += "eventprocessedutctime, prediction";

        // Send queries for both the device and globally. Order the results by the timestamp and the deviceid. 
        CustomQuery deviceQuery = new CustomQuery(filter, "eventprocessedutctime, deviceid", 50, deviceSkip, select);
        StartCoroutine(_table.Query<Telemetry>(deviceQuery, DeviceQuery));
        CustomQuery globalQuery = new CustomQuery(filter2, "eventprocessedutctime, deviceid", 50, globalSkip, select);
        StartCoroutine(_table.Query<Telemetry>(globalQuery, GlobalQuery));
    }


 
    private void GlobalQuery(IRestResponse<NestedResults<Telemetry>> response)
    {
        if (!response.IsError)
        {
            Telemetry[] items = response.Data.results;
            if (globalSkip != 0)
            {
                global.AddRange(items.ToList());
            }
            else
            {
                global = items.ToList(); // set for first page of results
            }

            globalSkip += 50;
            if (globalSkip < response.Data.count)
            {
                CustomQuery globalQuery = new CustomQuery(filter2, "eventprocessedutctime, deviceid", 50, globalSkip, select);
                StartCoroutine(_table.Query<Telemetry>(globalQuery, GlobalQuery));
            }
            else
            {
                ProgressRing.flag = false;
                if (global.Count() != 0)
                {
                    foreach (string element in attributes)
                    {
                        telGlobal[element] = dataPoints(global, element);
                    }
                    GetStats(0, telGlobal);
                }
            }
        }
        else
        {
            Debug.LogWarning("Read Nested Results Error Status:" + response.StatusCode.ToString() + " Url: " + response.Url);
        }
    }
    


    private void DeviceQuery(IRestResponse<NestedResults<Telemetry>> response)
    {
        if (!response.IsError)
        {
            Telemetry[] items = response.Data.results;
            if (deviceSkip != 0)
            {
                device.AddRange(items.ToList());
            }
            else
            {
                device = items.ToList(); // set for first page of results
            }

            deviceSkip += 50;
            if (deviceSkip < response.Data.count)
            {
                CustomQuery deviceQuery = new CustomQuery(filter, "eventprocessedutctime, deviceid", 50, deviceSkip, select);
                StartCoroutine(_table.Query<Telemetry>(deviceQuery, DeviceQuery));
            }
            else
            {
                ProgressRing.flag2 = false;
                if (device.Count() == 0)
                {
                    flag = false;
                }
                else
                {
                    
                    foreach (string element in attributes)
                    {
                        graphVals[element] = dataPoints(device, element);
                    }
                    GetStats(4, graphVals);

                    TextToSpeechControl.Speech(getNumAnomalies(device).ToString() + " anomalous data points detected");
                    /*textToSpeech.SpeakText(getNumAnomalies(device).ToString() + " anomalous data points detected");*/
                }
            }
        }
        else
        {
            Debug.LogWarning("Read Nested Results Error Status:" + response.StatusCode.ToString() + " Url: " + response.Url);
        }
    }
    
    // a helper function used to get the stats to fill in the table
    private void GetStats(int start, Dictionary<string, float[]> telList)
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            tableVals[tableStats * i + start] = getMax(telList[attributes[i]].ToList());
            tableVals[tableStats * i + start + 1] = getMin(telList[attributes[i]].ToList());
            tableVals[tableStats * i + start + 2] = getSTD(telList[attributes[i]].ToList());
            tableVals[tableStats * i + start + 3] = getAvg(telList[attributes[i]].ToList());
        }
    }


    // The time ranges specified in the query may equal the beginning and ending time. Thus, we find this in the data. Since the list is sorted by time, we can take the first 
    // and last elements.
    private string[] TimeRange(List<Telemetry> g)
    {
        return new string[] { Convert.ToDateTime(g.First().eventprocessedutctime).ToUniversalTime().ToString(),
            Convert.ToDateTime(g[g.Count() - 1].eventprocessedutctime).ToUniversalTime().ToString()};
    }


   // Returns an array of measurements for a specific attribute. This will change based on the attribute values
   private float[] dataPoints(List<Telemetry> g, string t)
    {
        
        IEnumerable<float> query1 = null;
        if (t == "temperature")
        {
            query1 = (from element in g select element.temperature);
        }
        else if (t == "humidity")
        {
            query1 = (from element in g select element.humidity);
        }

        float[] array = query1.Cast<float>().ToArray();


        return array;
    }


    private int getNumAnomalies(List<Telemetry> g)
    {
        int query1 = 0;
        if (g.Count > 0)
        {
            query1 = (from element in g where element.prediction == 1 select element.prediction).Count();
        }
        return query1;
    }

    // Returns the average of the elements. This will change based on the attribute values
    private float getAvg(List<float> g)
    {
        var query1 = 0f;
        query1 = g.Average();
        return query1;

    }


    // Returns the minimum of the elements. This will change based on the attribute values
    private float getMin(List<float> g)
    {
        var query1 = 0f;
        query1 = g.Min();
        return query1;
    }

    // Returns the maximum of the elements. This will change based on the attribute values
    private float getMax(List<float> g)
    {
        var query1 = 0f;
        query1 = g.Max();
        return query1;
    }


    // Returns the standard deviation of the elements. This will change based on the attribute values
    private float getSTD(List<float> g)
    {
        float variance;
        if (g != null)
        {
            variance = VarianceOfPopulation(g);
            return (float)Math.Sqrt(variance);
        }
        return 0f;
    }

    // Since were using the whole population to do this, we want the population variance. This will change based on the attribute values
    private static float VarianceOfPopulation(List<float> source)
    {
        var count = source.Count();

        if (count == 0) return 0;

        var mean = source.Average();
        var squaredDiffs = source.Select(d => (d - mean) * (d - mean));
        return squaredDiffs.Average();
    }


}
