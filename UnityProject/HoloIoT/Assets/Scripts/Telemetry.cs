using System;
using Unity3dAzure.AppServices;

[Serializable]

// Telemetry contains the fields that'll be pulled from the table. Developers should modify this 
// depending on their IoT scenario. 
public class Telemetry : DataModel
{
    public string deviceid;
    public float temperature;
    public float humidity;
    public string eventprocessedutctime;
    public float prediction;


}