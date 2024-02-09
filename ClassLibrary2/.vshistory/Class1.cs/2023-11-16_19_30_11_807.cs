using Valve.VR;
using System.Windows.Forms;
using System.IO;
using System;
using HarmonyLib;
using System.Linq;
using DistantWorlds.UI;

namespace ClassLibrary2
{
    
    public class Class1
    {
        public static void Test()
        {
            Harmony.DEBUG = true;
            //FileLog.Reset();
            var harmony = new Harmony("com.example.patch");

            //Type t = AppDomain.CurrentDomain.GetAssemblies()
            //.Select(assembly => assembly.GetType("DistantWorlds.UI.UserInterfaceController"))
            //.FirstOrDefault(t => t != null); // if possible use nameof() here
            //var mOriginal = AccessTools.Method(t, "SetActionButtonsFromTasks"); // if possible use nameof() here

            var mOriginal = AccessTools.Method(typeof(DistantWorlds.UI.UserInterfaceController), nameof(DistantWorlds.UI.UserInterfaceController.SetActionButtonsFromTasks));

            //var mPrefix = SymbolExtensions.GetMethodInfo(() => Prefix());
            //var mPostfix = SymbolExtensions.GetMethodInfo(() => Postfix());
            // in general, add null checks here (new HarmonyMethod() does it for you too)

            //harmony.Patch(mOriginal, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            //harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));

            harmony.PatchAll();

            FileLog.Log($"{DateTime.Now} patch done");
            FileLog.FlushBuffer();
        }
        //public static void Test(Type t)
        //{
        //    var harmony = new Harmony("com.example.patch");

        //    var mOriginal = AccessTools.Method(t, "DoSomething"); // if possible use nameof() here
        //    //var mPrefix = SymbolExtensions.GetMethodInfo(() => MyPrefix());
        //    //var mPostfix = SymbolExtensions.GetMethodInfo(() => MyPostfix());
        //    // in general, add null checks here (new HarmonyMethod() does it for you too)

        //    //harmony.Patch(mOriginal, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
        //    harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));

        //    FileLog.Log("patch done");
        //}

    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.UI.UserInterfaceController))]
    [HarmonyPatch(nameof(DistantWorlds.UI.UserInterfaceController.SetActionButtonsFromTasks))] // if possible use nameof() here
    public class Class2
    {
        public static void Prefix()
        {
            // ...
        }

        public static void Postfix()
        {
            FileLog.Log("postifx");
            MessageBox.Show("OhShit, it's alive! muahhaha!!!!");
        }
    }
}