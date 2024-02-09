using Valve.VR;
using System.Windows.Forms;
using System.IO;
using System;
using HarmonyLib;

namespace ClassLibrary2
{
    public class Class1
    {
        public static void Test()
        {
            var harmony = new Harmony("com.example.patch");

            var mOriginal = AccessTools.Method(typeof(DistantWorlds.UI.UserInterfaceController), nameof(DistantWorlds.UI.UserInterfaceController.SetActionButtonsFromTasks)); // if possible use nameof() here
            var mPrefix = SymbolExtensions.GetMethodInfo(() => MyPrefix());
            var mPostfix = SymbolExtensions.GetMethodInfo(() => MyPostfix());
            // in general, add null checks here (new HarmonyMethod() does it for you too)

            //harmony.Patch(mOriginal, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));

            FileLog.Log("patch done");
        }
        public static void Test(Type t)
        {
            var harmony = new Harmony("com.example.patch");

            var mOriginal = AccessTools.Method(t, "DoSomething"); // if possible use nameof() here
            var mPrefix = SymbolExtensions.GetMethodInfo(() => MyPrefix());
            var mPostfix = SymbolExtensions.GetMethodInfo(() => MyPostfix());
            // in general, add null checks here (new HarmonyMethod() does it for you too)

            //harmony.Patch(mOriginal, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));

            FileLog.Log("patch done");
        }

        public static void MyPrefix()
        {
            // ...
        }

        public static void MyPostfix()
        {
            FileLog.Log("postifx");
            MessageBox.Show("OhShit, it's alive! muahhaha!!!!");
        }
    }
}