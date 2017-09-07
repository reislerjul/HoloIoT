using System;
using Unity3dAzure.AppServices;

[Serializable]

// Contains the anomaly fields that'll be pulled from th table
public class Anomaly : DataModel
{
    public string deviceid;

}