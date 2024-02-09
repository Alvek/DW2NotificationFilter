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
using System.Drawing;
using System.Numerics;



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
        static DWPanel _panel;
        static bool _added = false;

        static Class2()
        {
            PanelGen();
        }
        public static  void Prefix()
        {
            // ...
        }

        public static void Postfix(object selected)
        {
            if (_added)
            { UserInterfaceController.RemoveCustomControl(_panel); _added = false; }
            if (selected != null)
            {
                UserInterfaceController.AddCustomControl(_panel);
                _added = true;
            }
        }
        public static void PanelGen()
        {
            DWPanel panel = new DWPanel();

            panel.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
            panel.BackgroundImageTintColor = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
            panel.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
            panel.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
            panel.ListSelectionColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
            panel.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
            panel.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);

            panel.Name = "TestPanel";
            panel.HeaderText = "TestPanel";
            panel.SetSizeAndPosition(new Stride.Core.Mathematics.Vector2(500, 400), new Stride.Core.Mathematics.Vector2(500, 500));
            UserInterfaceController.HoverInfo.Visible = false;
            panel.Visible = true;
            panel.AddCloseButtonToHeader = true;
            panel.SetFeatureDefaults();
            DWButton button = new DWButton();
            button.PostScene = true;

            button.ButtonColorDisabled = new Stride.Core.Mathematics.Color(1, 1, 1, 255);
            button.ButtonColorGradient = new Stride.Core.Mathematics.Color(48, 48, 48, 255);
            button.ButtonColorHover = new Stride.Core.Mathematics.Color(120, 120, 120, 255);
            button.ButtonColorToggle = new Stride.Core.Mathematics.Color(245, 180, 96, 255);
            button.BackColor = new Stride.Core.Mathematics.Color(96, 96, 96, 48);
            button.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
            button.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
            button.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
            button.ForeColorHover = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
            button.ListSelectionColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
            button.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
            button.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);
            button.DropDownSelectorBackgroundColor = new Stride.Core.Mathematics.Color(160, 160, 160, 40);
            button.DropDownSelectorBackgroundColorHover = new Stride.Core.Mathematics.Color(64, 64, 64, 128);

            DWControl.SetupButton(ref button, "TestBtn", new Stride.Core.Mathematics.Vector2(90, 90), new Stride.Core.Mathematics.Vector2(90, 90), "TestBtn", null, false);
            panel.AddControl(button);

            panel.Visible = true;
            button.Visible = true;
            panel.CloseButton.ClickEvent += ExitClick;
            _panel = panel;
        }

        private static void ExitClick(object? sender, DWEventArgs e)
        {
            UserInterfaceController.RemoveCustomControl(_panel); 
            _added = false;
        }
    }
}