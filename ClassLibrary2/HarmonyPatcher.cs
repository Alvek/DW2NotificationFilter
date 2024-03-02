using System.IO;
using System;
using HarmonyLib;
using System.Linq;
using DistantWorlds.UI;
using System.Reflection;
using DistantWorlds.Types;
using System.Drawing;
using System.Collections.Generic;
using Stride.Graphics;
using DistantWorlds2;
using Stride.Rendering;


namespace NotificationFilter
{

    public class Preloader
    {
        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            HarmonyPatcher.Init();
        }
        private static System.Reflection.Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            AssemblyName name = new AssemblyName(args.Name);
            if (name.Name == "0Harmony")
            {
                var asmLoc = typeof(HarmonyPatcher).Assembly.Location;
                var asmDir = Path.GetDirectoryName(asmLoc);
                var harmonyDll = Path.Join(asmDir, "0Harmony.dll");
                return Assembly.LoadFrom(harmonyDll);
            }
            return null;
        }
    }
    public class HarmonyPatcher
    {
        public static void Init()
        {
            //Logger.ChannelFilter = Logger.LogChannel.All;
            //Harmony.DEBUG = true;
            //FileLog.Reset();
            var harmony = new Harmony("DW2.NotificationFilter");
            harmony.PatchAll();
            
            //FileLog.Log($"{DateTime.Now} patch done");
            //FileLog.FlushBuffer();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.UI.MessageListView))]
    [HarmonyPatch(nameof(DistantWorlds.UI.MessageListView.CheckMessagePassesFilters))] // if possible use nameof() here
    //[HarmonyPatch(new Type[] { typeof(bool), typeof(List<EmpireMessage>) })]
    public class NotificationFilterPatcher
    {
        public static void Postfix(ref bool __result, EmpireMessage message)
        {
            Core.FilterEmpireMessageList(ref __result, message);            
        }
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
            Core.ShowEmpireMessageFilterCurrentBtn();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.UI.UserInterfaceController))]
    //[HarmonyPatch(nameof(DistantWorlds.UI.UserInterfaceController.HideEmpireMessageDialog))] // if possible use nameof() here
    //[HarmonyPatch(new Type[] { typeof(RectangleF), typeof(bool) })]
    public class ScaledRenderMessageHoverEndPatcher
    {
        /// <summary>
        /// Looks for methods we need to patch. Could be removed if right HarmonyPatch attribute values are added.
        /// </summary>
        /// <returns></returns>
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
            Core.HideEmpireMessageFilterCurrentBtn();
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
            Core.UpdateEmpireMessageFilterCurrentBtnPosition(__instance.TargetPosition);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds2.DWGame))]
    [HarmonyPatch("Initialize")]
    public class DWGamePatcher
    {        
        public static void Postfix(ScaledRenderer ____Renderer, DWGame __instance)
        {
            Core.Init(____Renderer);
        }
    }
}