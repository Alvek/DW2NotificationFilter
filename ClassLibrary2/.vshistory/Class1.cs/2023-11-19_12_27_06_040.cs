using System.Windows.Forms;
using System.IO;
using System;
using HarmonyLib;
using System.Linq;
using DistantWorlds.UI;
//using HarmonyLib.Tools;
using System.Diagnostics;
using System.Reflection;
using DistantWorlds.Types;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using Stride.Core.Serialization.Contents;



namespace ClassLibrary2
{

    public class Class0
    {
        public static void Test()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Class1.Test();
        }
        private static System.Reflection.Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
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
            //Logger.ChannelFilter = Logger.LogChannel.All;
            Harmony.DEBUG = true;
            //FileLog.Reset();
            var harmony = new Harmony("com.test.test");

            //Type t = AppDomain.CurrentDomain.GetAssemblies()
            //.Select(assembly => assembly.GetType("DistantWorlds.UI.UserInterfaceController"))
            //.FirstOrDefault(t => t != null); // if possible use nameof() here
            //var mOriginal = AccessTools.Method(t, "SetActionButtonsFromTasks"); // if possible use nameof() here

            //var mOriginal = AccessTools.Method(typeof(DistantWorlds.UI.UserInterfaceController), nameof(DistantWorlds.UI.UserInterfaceController.SetActionButtonsFromTasks));

            //var mPrefix = SymbolExtensions.GetMethodInfo(() => Class2.Prefix());
            //var mPostfix = SymbolExtensions.GetMethodInfo(() => Class2.Postfix());
            // in general, add null checks here (new HarmonyMethod() does it for you too)

            //harmony.Patch(mOriginal, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            //harmony.Patch(mOriginal, null, new HarmonyMethod(mPostfix));
            //try
            //{
            //    //mOriginal.Invoke(new  , new Galaxy(), typenewof(Empire), typeof(object), typeof(GameTask[]), typeof(GameTaskList[]), typeof(DWButton[] )});
            //    mOriginal.Invoke(null, new object[] { (Galaxy)null, (Empire)null, (object)null, (GameTask[])null, (GameTaskList[])null, null });
            //}
            //catch
            //{ }
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
    [HarmonyPatch(new Type[] { typeof(Galaxy), typeof(Empire), typeof(object), typeof(GameTask[]), typeof(GameTaskList[]), typeof(DWButton[]) })]
    public class Class2
    {
        public static void Prefix()
        {
            // ...
        }

        public static void Postfix(object selected)
        {
            if (selected != null)
            {
                DWPanel panel = new DWPanel();
                panel.Name = "TestPanel";
                panel.Position = new Stride.Core.Mathematics.Vector2(500, 500);

                UserInterfaceController.RemoveCustomControl(panel);
                
                UserInterfaceController.AddCustomControl(panel);
            }
        }
    }
}