using UnityEngine;

namespace Kogane
{
    public static class AndroidRestarter
    {
        // https://blog.nekonium.com/unity-restart-for-android/
        public static void Restart()
        {
            using var unityPlayer                      = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
            const int kIntent_FLAG_ACTIVITY_CLEAR_TASK = 0x00008000;
            const int kIntent_FLAG_ACTIVITY_NEW_TASK   = 0x10000000;

            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>( "currentActivity" );
            var pm              = currentActivity.Call<AndroidJavaObject>( "getPackageManager" );
            var intent          = pm.Call<AndroidJavaObject>( "getLaunchIntentForPackage", Application.identifier );

            intent.Call<AndroidJavaObject>( "setFlags", kIntent_FLAG_ACTIVITY_NEW_TASK | kIntent_FLAG_ACTIVITY_CLEAR_TASK );
            currentActivity.Call( "startActivity", intent );
            currentActivity.Call( "finish" );
            var process = new AndroidJavaClass( "android.os.Process" );
            var pid     = process.CallStatic<int>( "myPid" );
            process.CallStatic( "killProcess", pid );
        }
    }
}