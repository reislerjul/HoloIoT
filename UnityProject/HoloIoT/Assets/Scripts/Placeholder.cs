using System;
using UnityEngine;
using HoloToolkit.Unity;

public class Placeholder : MonoBehaviour
{



    private void Start()
    {

    }
    public void OnScan()
    {
        
        SQLConnect.ID = "Scanning";
        TextToSpeechControl.Speech("Scanning QR code");


#if !UNITY_EDITOR
    MediaFrameQrProcessing.Wrappers.ZXingQrCodeScanner.ScanFirstCameraForQrCode(
        result =>
        {
          UnityEngine.WSA.Application.InvokeOnAppThread(() =>
          {
            Debug.Log("scanned");
            SQLConnect.ID = result?.Text ?? "not found";
            if (SQLConnect.ID == "not found")
            {
                TextToSpeechControl.Speech("Scan failed");

            }
            else
            {  
                TextToSpeechControl.Speech("Scan successful");
                MoveToView.flag = true;
            }
          }, 
          false);
        },
        TimeSpan.FromSeconds(30));
#endif

    }


    public void OnTextScan()
    {
        TextToSpeechControl.Speech("Scanning text");
        SQLConnect.ID = "Scanning";


#if !UNITY_EDITOR
    MediaFrameQrProcessing.Wrappers.IPAddressScanner.ScanFirstCameraForIPAddress(
        result =>
        {
          UnityEngine.WSA.Application.InvokeOnAppThread(() =>
          {
            Debug.Log("scanned");
            SQLConnect.ID = result?.ToString() ?? "not found";
            if (SQLConnect.ID == "not found")
            {
                TextToSpeechControl.Speech("Scan failed");
            }
            else
            {  
                TextToSpeechControl.Speech("Scan successful");
                MoveToView.flag = true;
            }
          }, 
          false);
        },
        TimeSpan.FromSeconds(30));
#endif


    }

}