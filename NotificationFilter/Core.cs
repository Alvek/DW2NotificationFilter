using DistantWorlds.Types;
using DistantWorlds2;
using Stride.Core.Diagnostics;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationFilter
{
    static class Core
    {
        private static NotificationFilter _notificationFilter { get; set; } = new NotificationFilter();
        private static NativeWindowListener _nativeWindowListener { get; set; } = new NativeWindowListener();
        private static UiManager? _uiManager { get; set; }
        private static FilteredMessageAction? _filteredMessageAction { get; set; }

        static Core()
        {
            _nativeWindowListener.Activating += NativeWindowListener_Activating;
            _nativeWindowListener.Deactivating += NativeWindowListener_Deactivating;
        }


        #region CurrentEmpireMessage
        public static void ShowEmpireMessageFilterCurrentBtn()
        {
            _uiManager?.ShowEmpireMessageFilterCurrentBtn();
        }
        public static void HideEmpireMessageFilterCurrentBtn()
        {
            _uiManager?.HideEmpireMessageFilterCurrentBtn();
        }
        public static void UpdateEmpireMessageFilterCurrentBtnPosition(Vector2 pos)
        {
            _uiManager?.UpdatePosition(pos);
        }
        #endregion

        public static void FilterEmpireMessageList(ref bool result, EmpireMessage message)
        {
            TempFixCreateUiManager();
            SetHandle();
            _notificationFilter.Filter(ref result, message);
        }
        public static void SetRenderer(ScaledRenderer renderer)
        {
            _filteredMessageAction = new FilteredMessageAction(renderer);
        }
        public static bool DoFilterAction(BaseRule rule, EmpireMessage message)
        {
            return _filteredMessageAction?.DoFilterAction(rule, message) ?? false;
        }

        private static void SetHandle()
        {
            if (!_nativeWindowListener.HandleSet)
            {
                _nativeWindowListener.SetHandles(Process.GetCurrentProcess().MainWindowHandle, _uiManager.GetSettingFormHandle());
            }
        }

        private static void TempFixCreateUiManager()
        {
            if (_uiManager == null)
            { _uiManager = new UiManager(_notificationFilter.CreateFilterRule, Process.GetCurrentProcess().MainWindowHandle); }
        }

        #region NativeWindow

        private static void NativeWindowListener_Activating(object? sender, EventArgs e)
        {
            _uiManager?.ManageActivation();
        }
        private static void NativeWindowListener_Deactivating(object? sender, EventArgs e)
        {
            _uiManager?.ManageDeactivation();
        }
        #endregion
    }
}
