using UnityEngine;

namespace Managers.SpeechRecognition
{
    internal class SpeechRecognitionListener : AndroidJavaProxy
    {
        public delegate void OnBeginningOfSpeech();

        public delegate void OnRmsChanged(float rmsdB);

        public delegate void OnEndOfSpeech();

        public delegate void OnError(string errMsg);

        public delegate void OnResults(string[] list);

        private OnBeginningOfSpeech csonBeginningOfSpeech;
        private OnRmsChanged csonRmsChanged;
        private OnEndOfSpeech csonEndOfSpeech;
        private OnError csonError;
        private OnResults csonResults;

        public SpeechRecognitionListener(OnBeginningOfSpeech onBeginningOfSpeech, OnRmsChanged onRmsChanged, OnEndOfSpeech onEndOfSpeech, OnError onError, OnResults onResults) : base("com.chsi.hmdsdk.listener.SpeechListener")
        {
            csonBeginningOfSpeech = onBeginningOfSpeech;
            csonRmsChanged = onRmsChanged;
            csonEndOfSpeech = onEndOfSpeech;
            csonError = onError;
            csonResults = onResults;
        }

        #region {各事件供Android Jave底層CallBack呼叫}
        private void onBeginningOfSpeech() => this.csonBeginningOfSpeech?.Invoke();
        private void onRmsChanged(float rmsdB) => this.csonRmsChanged?.Invoke(rmsdB);
        private void onEndOfSpeech() => this.csonEndOfSpeech?.Invoke();
        private void onError(string errMsg) => this.csonError?.Invoke(errMsg);
        private void onResults(string[] list) => this.csonResults?.Invoke(list);
       #endregion
    }
}
