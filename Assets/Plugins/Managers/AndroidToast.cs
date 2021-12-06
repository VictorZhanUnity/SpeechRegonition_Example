using UnityEngine;

namespace AndroidScript
{
    /// <summary>
    /// 顯示浮動文字
    /// </summary>
    public class AndroidToast
    {
        private static AndroidToast _instance;
        public static AndroidToast Instance
        {
            get
            {
                if (_instance == null) _instance = new AndroidToast();
                return _instance;
            }
        }

        private AndroidJavaClass unityPlayer, toastClass;
        private AndroidJavaObject unityActivity;

        public void ShowToast(string value)
        {
            if (this.unityPlayer == null)
            {
                this.unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            }
            if (this.toastClass == null)
            {
                this.unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                this.toastClass = new AndroidJavaClass("android.widget.Toast");
            }
            if (this.unityActivity != null && this.toastClass != null)
            {
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, value, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
