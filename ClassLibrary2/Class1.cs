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
using System.Collections.Generic;
using NotificationFilter;
using Stride.UI.Panels;
using static DistantWorlds.UI.EmpireMessageDialog;
using Stride.Rendering;
using Stride.Graphics;
using DistantWorlds2;
using static Stride.Core.Diagnostics.TimestampLocalLogger;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;



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
            //Harmony.DEBUG = true;
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
    [HarmonyPatch(typeof(DistantWorlds.UI.MessageListView))]
    [HarmonyPatch(nameof(DistantWorlds.UI.MessageListView.CheckMessagePassesFilters))] // if possible use nameof() here
    //[HarmonyPatch(new Type[] { typeof(bool), typeof(List<EmpireMessage>) })]
    public class NotificationFilterPatcher
    {
        private static DWButton _btnFilterCurrent;
        private static DWButton _btnOpenSettings;
        private static DWPanel _panelBackground;
        private static ReaderWriterLockSlim _ruleListLocker = new(LockRecursionPolicy.NoRecursion);
        private static int _alreadyRunning = _StoppedValue;
        private const int _RunningValue = 1;
        private const int _StoppedValue = 0;

        private static Form1 _fFilterSettings = new Form1();

        public static Rules _Rules = new Rules();

        static NotificationFilterPatcher()
        {
            _btnFilterCurrent = CreateButton();
            _btnOpenSettings = CreateButton();


            DWControl.SetupButton(ref _btnFilterCurrent, "FilterCurrent", new Stride.Core.Mathematics.Vector2(90, 90), new Stride.Core.Mathematics.Vector2(90, 90), "Filter current", CreateFilterRule, true);
            DWControl.SetupButton(ref _btnOpenSettings, "OpenFilterSettings", new Stride.Core.Mathematics.Vector2(90, 90), new Stride.Core.Mathematics.Vector2(90, 90), "Open filter settings", ShowFilterSettings, true);

            _btnFilterCurrent.Visible = false;
            _btnOpenSettings.Visible = false;

            _panelBackground = CreatePanel(_fFilterSettings.Size);
            _fFilterSettings.Hiden += fFilterSettings_Hiden;
            //UserInterfaceController.AddCustomControl(_dwBackPanel);
        }

        public static void Postfix(ref bool __result, EmpireMessage message)
        {
            //__result = false;
            //return;
            bool res = true;
            BaseRule activatedRule = null;
            if (__result)
            {
                _ruleListLocker.EnterReadLock();
                foreach (var item in _Rules.RuleList)
                {
                    if (item.MessageType == message.Type)
                    {
                        activatedRule = item;
                        res = false;
                        break;
                    }
                    else
                    {
                        foreach (var itemRule in item.ItemRules)
                        {
                            if (message.RelatedItem != null)
                            {
                                if (message.RelatedItem != null && (itemRule.RelatedItemType == message.RelatedItem.Type &&
                                    itemRule.Id == message.RelatedItem.Id))
                                {
                                    res = false;
                                    break;
                                }
                            }
                        }
                        if (!res)
                        {
                            activatedRule = item;
                            break;
                        }

                        foreach (var itemRule in item.ObjectRules)
                        {
                            if (message.RelatedObject != null)
                            {
                                if (message.RelatedObject != null && (itemRule.RelatedObjectType == message.RelatedObject.ItemType))
                                {
                                    res = false;
                                    break;
                                }
                            }
                        }
                        if (!res)
                        {
                            activatedRule = item;
                            break;
                        }
                    }
                }
                _ruleListLocker.ExitReadLock();
                int value;
                if (!res && Interlocked.CompareExchange(ref _alreadyRunning, _RunningValue, _StoppedValue) == 0)
                {
                    __result = false;
                    if (activatedRule.DefaultAction == DefaultMessageAction.Yes)
                    {

                        // new DWButtonData("EmpireMessageDialogYes", "Text", new EventHandler<DWEventArgs>(_ScaledRenderer.EmpireMessageYesClick), __instance);
                        DWGamePatcher._Renderer.EmpireMessageYesClick(null, new DWEventArgs(message));
                    }
                    else if (activatedRule.DefaultAction == DefaultMessageAction.No)
                    { DWGamePatcher._Renderer.EmpireMessageNoClick(null, new DWEventArgs(message)); }
                    else if (activatedRule.DefaultAction == DefaultMessageAction.Show)
                    { DWGamePatcher._Renderer.EmpireMessageShowClick(null, new DWEventArgs(message)); }
                    _alreadyRunning = _StoppedValue;
                }
            }
        }
        public static void ShowButton()
        {
            //_dwButton.Position = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.Position.X,
            //    UserInterfaceController.EmpireMessageDialog.Position.Y);
            //_dwButton.TargetPosition = new Stride.Core.Mathematics.Vector2 (UserInterfaceController.EmpireMessageDialog.Position.X, 
            //    UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.ClipRegion.Height);
            _btnFilterCurrent.Visible = true;
            _btnOpenSettings.Visible = true;
        }
        public static void HideButton()
        {
            _btnFilterCurrent.Visible = false;
            _btnOpenSettings.Visible = false;
        }

        public static void UpdatePosition(Stride.Core.Mathematics.Vector2 pos)
        {
            _btnFilterCurrent.Position = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);
            _btnFilterCurrent.TargetPosition = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);

            _btnOpenSettings.Position = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X + _btnFilterCurrent.Size.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);
            _btnOpenSettings.TargetPosition = new Stride.Core.Mathematics.Vector2(UserInterfaceController.EmpireMessageDialog.TargetPosition.X + _btnFilterCurrent.Size.X,
                UserInterfaceController.EmpireMessageDialog.TargetPosition.Y + UserInterfaceController.EmpireMessageDialog.Size.Y);
        }
        private static void CreateFilterRule(object? sender, DWEventArgs e)
        {
            if (UserInterfaceController.EmpireMessageDialog.Message != null)
            {
                _ruleListLocker.EnterWriteLock();
                var msg = UserInterfaceController.EmpireMessageDialog.Message;
                BaseRule rule = new BaseRule();
                rule.DefaultAction = DefaultMessageAction.Yes;
                rule.MessageType = msg.Type;
                rule.ObjectRules.Add(new() { RelatedObjectType = msg.RelatedObject.ItemType });
                rule.ItemRules.Add(new() { Id = msg.RelatedItem.Id, RelatedItemType = msg.RelatedItem.Type });
                _Rules.RuleList.Add(rule);
                _ruleListLocker.ExitWriteLock();
            }

        }

        private static DWPanel CreatePanel(Size size)
        {
            DWPanel res = new DWPanel();
            res.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
            res.BackgroundImageTintColor = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
            res.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
            res.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
            res.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
            res.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);

            res.Name = "TestPanel";
            res.HeaderText = "Filter settings";
            
            UserInterfaceController.HoverInfo.Visible = false;
            res.Visible = true;
            res.AddCloseButtonToHeader = true;
            res.SetFeatureDefaults();
            return res;
        }
        private static DWButton CreateButton()
        {
            DWButton res = new()
            {
                PostScene = true,
                ButtonColorDisabled = new Stride.Core.Mathematics.Color(1, 1, 1, 255),
                ButtonColorGradient = new Stride.Core.Mathematics.Color(48, 48, 48, 255),
                ButtonColorHover = new Stride.Core.Mathematics.Color(120, 120, 120, 255),
                ButtonColorToggle = new Stride.Core.Mathematics.Color(245, 180, 96, 255),
                BackColor = new Stride.Core.Mathematics.Color(96, 96, 96, 48),
                BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48),
                DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255),
                ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255),
                ForeColorHover = new Stride.Core.Mathematics.Color(255, 255, 255, 255),
                ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255),
                ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255),
                DropDownSelectorBackgroundColor = new Stride.Core.Mathematics.Color(160, 160, 160, 40),
                DropDownSelectorBackgroundColorHover = new Stride.Core.Mathematics.Color(64, 64, 64, 128)
            };
            return res;
        }

        private static void ShowFilterSettings(object? sender, DWEventArgs e)
        {
            _panelBackground.SetSizeAndPosition(new Stride.Core.Mathematics.Vector2(_fFilterSettings.Width, _fFilterSettings.Height + _panelBackground.HeaderHeight), new Stride.Core.Mathematics.Vector2());
            _panelBackground.Position = new Stride.Core.Mathematics.Vector2(UserInterfaceController.ScreenWidth / 2 - _fFilterSettings.Width / 2, UserInterfaceController.ScreenHeight / 2 - _fFilterSettings.Height / 2 - _panelBackground.HeaderHeight);
            _panelBackground.TargetPosition = new Stride.Core.Mathematics.Vector2(UserInterfaceController.ScreenWidth / 2 - _fFilterSettings.Width / 2, UserInterfaceController.ScreenHeight / 2 - _fFilterSettings.Height / 2 - _panelBackground.HeaderHeight);
            UserInterfaceController.AddCustomControl(_panelBackground);
            _fFilterSettings.Location = new Point(UserInterfaceController.ScreenWidth / 2 - _fFilterSettings.Width / 2, UserInterfaceController.ScreenHeight / 2 - _fFilterSettings.Height / 2 );
            _fFilterSettings.Show();
        }

        private static void fFilterSettings_Hiden(object? sender, EventArgs e)
        {
            UserInterfaceController.RemoveCustomControl(_panelBackground);
        }

        //[HarmonyDebug]
        //[HarmonyPatch(typeof(DistantWorlds.UI.UserInterfaceController))]
        //[HarmonyPatch(nameof(DistantWorlds.UI.UserInterfaceController.SetActionButtonsFromTasks))] // if possible use nameof() here
        //[HarmonyPatch(new Type[] { typeof(Galaxy), typeof(Empire), typeof(object), typeof(GameTask[]), typeof(GameTaskList[]), typeof(DWButton[]) })]
        //public class Class2
        //{
        //    static DWPanel _panel;
        //    static bool _added = false;

        //    static Class2()
        //    {
        //        PanelGen();
        //    }
        //    public static  void Prefix()
        //    {
        //        // ...
        //    }

        //    public static void Postfix(object selected)
        //    {
        //        if (_added)
        //        { UserInterfaceController.RemoveCustomControl(_panel); _added = false; }
        //        if (selected != null)
        //        {
        //            UserInterfaceController.AddCustomControl(_panel);
        //            _added = true;
        //        }
        //    }
        //    public static void PanelGen()
        //    {
        //        DWPanel panel = new DWPanel();

        //        panel.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
        //        panel.BackgroundImageTintColor = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
        //        panel.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
        //        panel.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        panel.ListSelectionColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        panel.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
        //        panel.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);

        //        panel.Name = "TestPanel";
        //        panel.HeaderText = "TestPanel";
        //        panel.SetSizeAndPosition(new Stride.Core.Mathematics.Vector2(500, 400), new Stride.Core.Mathematics.Vector2(500, 500));
        //        UserInterfaceController.HoverInfo.Visible = false;
        //        panel.Visible = true;
        //        panel.AddCloseButtonToHeader = true;
        //        panel.SetFeatureDefaults();
        //        DWButton button = new DWButton();
        //        button.PostScene = true;

        //        button.ButtonColorDisabled = new Stride.Core.Mathematics.Color(1, 1, 1, 255);
        //        button.ButtonColorGradient = new Stride.Core.Mathematics.Color(48, 48, 48, 255);
        //        button.ButtonColorHover = new Stride.Core.Mathematics.Color(120, 120, 120, 255);
        //        button.ButtonColorToggle = new Stride.Core.Mathematics.Color(245, 180, 96, 255);
        //        button.BackColor = new Stride.Core.Mathematics.Color(96, 96, 96, 48);
        //        button.BorderColor = new Stride.Core.Mathematics.Color(128, 128, 128, 48);
        //        button.DarkColor = new Stride.Core.Mathematics.Color(96, 96, 96, 255);
        //        button.ForeColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        button.ForeColorHover = new Stride.Core.Mathematics.Color(255, 255, 255, 255);
        //        button.ListSelectionColor = new Stride.Core.Mathematics.Color(212, 212, 212, 255);
        //        button.ListSelectionColor = new Stride.Core.Mathematics.Color(255, 0, 0, 255);
        //        button.ShadowColor = new Stride.Core.Mathematics.Color(0, 0, 0, 255);
        //        button.DropDownSelectorBackgroundColor = new Stride.Core.Mathematics.Color(160, 160, 160, 40);
        //        button.DropDownSelectorBackgroundColorHover = new Stride.Core.Mathematics.Color(64, 64, 64, 128);

        //        DWControl.SetupButton(ref button, "TestBtn", new Stride.Core.Mathematics.Vector2(90, 90), new Stride.Core.Mathematics.Vector2(90, 90), "TestBtn", null, false);
        //        panel.AddControl(button);

        //        panel.Visible = true;
        //        button.Visible = true;
        //        panel.CloseButton.ClickEvent += ExitClick;
        //        _panel = panel;
        //    }

        //    private static void ExitClick(object? sender, DWEventArgs e)
        //    {
        //        UserInterfaceController.RemoveCustomControl(_panel); 
        //        _added = false;
        //    }
        //}
    }


    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.UI.UserInterfaceController))]
    [HarmonyPatch(nameof(DistantWorlds.UI.UserInterfaceController.ShowEmpireMessageDialog))] // if possible use nameof() here
                                                                                             //[HarmonyPatch(new Type[] { typeof(EmpireMessage), typeof(bool), typeof(EmpireMessageDialog.ClickedEventHandler),
                                                                                             //typeof(DWButtonData),typeof(DWButtonData),typeof(DWButtonData),typeof(DWButtonData),typeof(RectangleF)
                                                                                             //,typeof(Side),typeof(float),typeof(bool),typeof( EmpireMessageDialog.MessageHoveredEventHandler),
                                                                                             //    typeof( EmpireMessageDialog.MessageHoveredEventHandler)})]
    public class ScaledRenderMessageHoverStartPatcher
    {

        //[HarmonyTargetMethods]
        //static IEnumerable<MethodBase> TargetMethods()
        //{
        //    var res = AccessTools.GetTypesFromAssembly(typeof(DistantWorlds.UI.UserInterfaceController).Assembly)
        //        .SelectMany(type => type.GetMethods())
        //        .Where(method => method.Name == nameof(DistantWorlds.UI.UserInterfaceController.ShowEmpireMessageDialog))
        //        .Cast<MethodBase>();
        //    return res;
        //}

        public static void Postfix()
        {
            NotificationFilterPatcher.ShowButton();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.UI.UserInterfaceController))]
    //[HarmonyPatch(nameof(DistantWorlds.UI.UserInterfaceController.HideEmpireMessageDialog))] // if possible use nameof() here
    //[HarmonyPatch(new Type[] { typeof(RectangleF), typeof(bool) })]
    public class ScaledRenderMessageHoverEndPatcher
    {
        [HarmonyTargetMethods]
        static IEnumerable<MethodBase> TargetMethods()
        {
            var res = AccessTools.GetTypesFromAssembly(typeof(DistantWorlds.UI.UserInterfaceController).Assembly)
                .SelectMany(type => type.GetMethods())
                .Where(method => method.Name == nameof(DistantWorlds.UI.UserInterfaceController.HideEmpireMessageDialog) && method.GetParameters().Length == 2)
                .Cast<MethodBase>();
            return res;
        }

        public static void Postfix(RectangleF targetRectangle)
        {
            NotificationFilterPatcher.HideButton();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.UI.EmpireMessageDialog))]
    [HarmonyPatch(nameof(DistantWorlds.UI.EmpireMessageDialog.Render))] // if possible use nameof() here
    [HarmonyPatch(new Type[] { typeof(RenderDrawContext), typeof(SpriteBatch), typeof(Stride.Core.Mathematics.Point), typeof(bool) })]
    public class EmpireMessageDialorRenderPatcher
    {
        public static void Postfix(EmpireMessageDialog __instance)
        {
            NotificationFilterPatcher.UpdatePosition(__instance.TargetPosition);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds2.DWGame))]
    [HarmonyPatch("Initialize")]
    public class DWGamePatcher
    {
        public static ScaledRenderer _Renderer;
        
        public static void Postfix(ScaledRenderer ____Renderer, DWGame __instance)
        {
            _Renderer = ____Renderer;
            __instance.Window.Deactivated += Window_Deactivated;
            __instance.Window.Activated += Window_Activated;
            __instance.Window.FullscreenChanged += Window_FullscreenChanged;
            __instance.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private static void Window_ClientSizeChanged(object? sender, EventArgs e)
        {
        }

        private static void Window_FullscreenChanged(object? sender, EventArgs e)
        {
        }

        private static void Window_Activated(object? sender, EventArgs e)
        {

        }

        private static void Window_Deactivated(object? sender, EventArgs e)
        {
            
        }
    }
}