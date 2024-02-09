using System.Windows.Forms;
using System.IO;
using System;
using HarmonyLib;
using System.Linq;
using DistantWorlds.UI;
//using HarmonyLib.Tools;
using System.Diagnostics;
using System.Reflection;



namespace ClassLibrary2
{

    public class Class0
    {
        public static void Test()
        {
            Debugger.Break();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Class1.Test();
        }
        private static System.Reflection.Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            Debugger.Break();
            AssemblyName name = new AssemblyName(args.Name);
            if (name.Name == "0Harmony")
            {
                var asmLoc = typeof(Class1).Assembly.Location;
                var asmDir = Path.GetDirectoryName(asmLoc);
                var harmonyDll = Path.Join(asmDir, "0Harmony.dll");
                return Assembly.LoadFrom(harmonyDll);
            }
            return null;
        }
    }
    public class Class1
    {
        public static void Test()
        {
            Debugger.Break();
            //Logger.ChannelFilter = Logger.LogChannel.All;
            Harmony.DEBUG = true;
            //FileLog.Reset();
            var harmony = new Harmony("com.test.test");

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
            Debugger.Break();
            // ...
        }

        public static void Postfix()
        {
            Debugger.Break();
            FileLog.Log("postifx");
            MessageBox.Show("OhShit, it's alive! muahhaha!!!!");
        }
    }
}