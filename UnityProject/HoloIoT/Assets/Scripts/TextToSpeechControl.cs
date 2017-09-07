using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


public class TextToSpeechControl : MonoBehaviour {

    public GameObject sound;
    private static TextToSpeechManager textToSpeech;

    void Start()
    {
        textToSpeech = sound.GetComponent<TextToSpeechManager>();
        textToSpeech.Voice = TextToSpeechVoice.Zira;
    }


    public static void Speech(string phrase)
    {
        textToSpeech.SpeakText(phrase);
    }


}
