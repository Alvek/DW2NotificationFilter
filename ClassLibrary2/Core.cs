using DistantWorlds.Types;
using DistantWorlds2;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
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
        private static UiManager _uiManager { get; set; }
        private static FilteredMessageAction? _filteredMessageAction { get; set; }

        #region CurrentEmpireMessage
        public static void ShowEmpireMessageFilterCurrentBtn()
        {
            SetHandle();
            _uiManager.ShowEmpireMessageFilterCurrentBtn();
        }
        public static void HideEmpireMessageFilterCurrentBtn()
        {
            _uiManager.HideEmpireMessageFilterCurrentBtn();
        }
        public static void UpdateEmpireMessageFilterCurrentBtnPosition(Vector2 pos)
        {
            _uiManager.UpdatePosition(pos); 
        }
        #endregion

        public static void FilterEmpireMessageList(ref bool result, EmpireMessage message)
        {
            _notificationFilter.Filter(ref result, message);
        }
        public static void Init(ScaledRenderer renderer)
        {
            _uiManager = new UiManager(_notificationFilter.CreateFilterRule);
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
                _nativeWindowListener.SetHandle(Process.GetCurrentProcess().MainWindowHandle);
            }
        }
    }
}
