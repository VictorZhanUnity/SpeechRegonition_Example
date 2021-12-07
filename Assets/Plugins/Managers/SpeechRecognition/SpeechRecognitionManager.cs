using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Android;

namespace Managers.SpeechRecognition
{
    /// <summary>
    /// From Google Speech-to-Text API
    /// 使用AddEventListener新增CallBack事件處理
    /// 使用StartSpeech開始語音辨識
    /// </summary>
    public class SpeechRecognitionManager
    {
        #region {Singleton:Instance}
        private static SpeechRecognitionManager _instance;
        public static SpeechRecognitionManager Instance
        {
            get
            {
                if (_instance == null) _instance = new SpeechRecognitionManager();
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 是否已啟動錄音
        /// </summary>
        public bool IsActivated
        {
            get { return m_IsActivated; }
        }
        private bool m_IsActivated = false;

        internal AndroidJavaObject ajObject;
        internal SpeechRecognitionListener eventListner;

        public UnityAction OnBeginningOfSpeech;
        public UnityAction<float> OnRmsChanged;
        public UnityAction OnEndOfSpeech;
        public UnityAction<string> OnError;
        public UnityAction<string[]> OnResults;

        public SpeechRecognitionManager()
        {
            RequireMicPermission();
            eventListner = new SpeechRecognitionListener(onBeginningOfSpeech, onRmsChanged, onEndOfSpeech, onError, onResults);
            ajObject = new AndroidJavaObject("com.chsi.hmdsdk.manager.SpeechManager", GetCurrentActivity());
            ajObject.Call("addSpeechListener", eventListner);
        }

        /// <summary>
        /// 請求App錄音權限
        /// </summary>
        private void RequireMicPermission()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }

        /// <summary>
        /// 開始辨識語音 (過一段時間會自動關閉)
        /// </summary>
        public void StartSpeech() => this.ajObject?.Call("start");
        /// <summary>
        /// 停止辨識語音
        /// </summary>
        public void StopSpeech() => this.ajObject?.Call("stop");
        /// <summary>
        /// 回收
        /// </summary>
        public void Destroy() => this.ajObject?.Call("destroy");

        /// <summary>
        /// 新增事件CallBack
        /// </summary>
        /// <param name="onBeginningOfSpeech">開始語音辨識</param>
        /// <param name="onRmsChanged">接收rmsdB資料(音量大小)</param>
        /// <param name="onEndOfSpeech">結束語音辨識</param>
        /// <param name="onError">錯誤訊息(沒偵測到結果)</param>
        /// <param name="onResults">辦識結果 (有偵測到結果)</param>
        public void AddEventListener(UnityAction onBeginningOfSpeech, UnityAction onEndOfSpeech, UnityAction<string> onError, UnityAction<string[]> onResults, UnityAction<float> onRmsChanged)
        {
            OnBeginningOfSpeech += onBeginningOfSpeech;
            OnEndOfSpeech += onEndOfSpeech;
            OnError += onError;
            OnResults += onResults;
            OnRmsChanged += onRmsChanged;
        }
        public void RemoveEventListener(UnityAction onBeginningOfSpeech, UnityAction onEndOfSpeech, UnityAction<string> onError, UnityAction<string[]> onResults, UnityAction<float> onRmsChanged)
        {
            OnBeginningOfSpeech -= onBeginningOfSpeech;
            OnEndOfSpeech -= onEndOfSpeech;
            OnError -= onError;
            OnResults -= onResults;
            OnRmsChanged -= onRmsChanged;
        }

        #region{Listner CallBack事件呼叫}
        /// <summary>
        /// 開始語音辨識
        /// </summary>
        private void onBeginningOfSpeech()
        {
            m_IsActivated = true;
            OnBeginningOfSpeech?.Invoke();
        }
        /// <summary>
        /// 接收rmsdB資料(音量大小)
        /// </summary>
        private void onRmsChanged(float rmsdB) => OnRmsChanged?.Invoke(rmsdB);
        /// <summary>
        /// 結束語音辨識
        /// </summary>
        private void onEndOfSpeech()
        {
            m_IsActivated = false;
            OnEndOfSpeech?.Invoke();
        }
        /// <summary>
        /// 錯誤訊息(沒偵測到結果)
        /// </summary>
        private void onError(string errMsg) => OnError?.Invoke(errMsg);
        /// <summary>
        /// 辦識結果，回傳相似字的清單 (有偵測到結果)
        /// </summary>
        private void onResults(string[] dataList) => OnResults?.Invoke(dataList);
        #endregion

        private AndroidJavaObject GetCurrentActivity()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            return activity;
        }
    }
}

