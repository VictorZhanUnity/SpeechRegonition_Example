using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Android;

namespace Managers.SpeechRecognition
{
    /// <summary>
    /// From Google Speech-to-Text API
    /// �ϥ�AddEventListener�s�WCallBack�ƥ�B�z
    /// �ϥ�StartSpeech�}�l�y������
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
        /// �O�_�w�Ұʿ���
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
        /// �ШDApp�����v��
        /// </summary>
        private void RequireMicPermission()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        }

        /// <summary>
        /// �}�l���ѻy�� (�L�@�q�ɶ��|�۰�����)
        /// </summary>
        public void StartSpeech() => this.ajObject?.Call("start");
        /// <summary>
        /// ������ѻy��
        /// </summary>
        public void StopSpeech() => this.ajObject?.Call("stop");
        /// <summary>
        /// �^��
        /// </summary>
        public void Destroy() => this.ajObject?.Call("destroy");

        /// <summary>
        /// �s�W�ƥ�CallBack
        /// </summary>
        /// <param name="onBeginningOfSpeech">�}�l�y������</param>
        /// <param name="onRmsChanged">����rmsdB���(���q�j�p)</param>
        /// <param name="onEndOfSpeech">�����y������</param>
        /// <param name="onError">���~�T��(�S�����쵲�G)</param>
        /// <param name="onResults">���ѵ��G (�������쵲�G)</param>
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

        #region{Listner CallBack�ƥ�I�s}
        /// <summary>
        /// �}�l�y������
        /// </summary>
        private void onBeginningOfSpeech()
        {
            m_IsActivated = true;
            OnBeginningOfSpeech?.Invoke();
        }
        /// <summary>
        /// ����rmsdB���(���q�j�p)
        /// </summary>
        private void onRmsChanged(float rmsdB) => OnRmsChanged?.Invoke(rmsdB);
        /// <summary>
        /// �����y������
        /// </summary>
        private void onEndOfSpeech()
        {
            m_IsActivated = false;
            OnEndOfSpeech?.Invoke();
        }
        /// <summary>
        /// ���~�T��(�S�����쵲�G)
        /// </summary>
        private void onError(string errMsg) => OnError?.Invoke(errMsg);
        /// <summary>
        /// ���ѵ��G�A�^�Ǭۦ��r���M�� (�������쵲�G)
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

