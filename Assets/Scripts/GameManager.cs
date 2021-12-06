using System;
using UnityEngine;
using UnityEngine.UI;
using Managers.SpeechRecognition;
using AndroidScript;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Button btnStartSpeech;

    [SerializeField]
    private Text txtStatus, txtConsole, txtRms;

    void Start()
    {
        SpeechRecognitionManager.Instance.AddEventListener(OnBeginningOfSpeech, OnEndOfSpeech, OnError, OnResults, OnRmsChanged);
        this.btnStartSpeech.onClick.AddListener(StartSpeech);
        txtConsole.text = "";
    }
    private void StartSpeech()
    {
        LogConsole("StartSpeech");
        SpeechRecognitionManager.Instance.StartSpeech();
    }

    // 即將開始語音辨識
    void OnBeginningOfSpeech() => txtStatus.text = "<color=red>開啟語音辨識</color>";
    // 接收rmsdB資料
    void OnRmsChanged(float rmsdB) => txtRms.text = $"RMS: {rmsdB.ToString("f3")}";
    // 結束語音辨識
    void OnEndOfSpeech() => txtStatus.text = "<color=yellow>結束語音辨識</color>";
    // 錯誤訊息(沒偵測到結果)
    void OnError(string errMsg) => LogConsole("onError: " + errMsg);
    // 辨識結果(偵測到結果)
    void OnResults(string[] list) => LogConsole("onResults: " + string.Join(", ", list));
    
    private void LogConsole(string msg)
    {
        Debug.Log(msg);
        txtConsole.text += $"\n{DateTime.Now.ToString("HH:mm:ss")} [LOG] {msg}";
        AndroidToast.Instance.ShowToast(msg);
    }

    private void OnDestroy()
    {
        SpeechRecognitionManager.Instance.RemoveEventListener(OnBeginningOfSpeech, OnEndOfSpeech, OnError, OnResults, OnRmsChanged);
    }

    private void Reset()
    {
        btnStartSpeech = GameObject.Find("ButtonStartSpeech").GetComponent<Button>();
        txtStatus = GameObject.Find("TextStatus").GetComponent<Text>();
        txtConsole = GameObject.Find("TextConsole").GetComponent<Text>();
        txtRms = GameObject.Find("TextRms").GetComponent<Text>();
    }
}
