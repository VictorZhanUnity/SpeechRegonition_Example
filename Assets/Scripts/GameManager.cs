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

    // �Y�N�}�l�y������
    void OnBeginningOfSpeech() => txtStatus.text = "<color=red>�}�һy������</color>";
    // ����rmsdB���
    void OnRmsChanged(float rmsdB) => txtRms.text = $"RMS: {rmsdB.ToString("f3")}";
    // �����y������
    void OnEndOfSpeech() => txtStatus.text = "<color=yellow>�����y������</color>";
    // ���~�T��(�S�����쵲�G)
    void OnError(string errMsg) => LogConsole("onError: " + errMsg);
    // ���ѵ��G(�����쵲�G)
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
