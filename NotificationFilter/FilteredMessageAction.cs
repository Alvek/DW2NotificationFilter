using DistantWorlds.Types;
using DistantWorlds2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationFilter
{
    internal class FilteredMessageAction
    {
        private readonly ScaledRenderer _renderer;

        private int _alreadyRunning = _StoppedValue;

        private const int _RunningValue = 1;
        private const int _StoppedValue = 0;

        public FilteredMessageAction(ScaledRenderer renderer)
        {
            this._renderer = renderer;
        }

        public bool DoFilterAction(BaseRule rule, EmpireMessage message)
        {
            bool res = false;
            if (Interlocked.CompareExchange(ref _alreadyRunning, _RunningValue, _StoppedValue) == 0)
            {
                res = true;
                if (rule.DefaultAction == DefaultMessageAction.Yes)
                {
                    // new DWButtonData("EmpireMessageDialogYes", "Text", new EventHandler<DWEventArgs>(_ScaledRenderer.EmpireMessageYesClick), __instance);
                    _renderer.EmpireMessageYesClick(this, new DWEventArgs(message));
                }
                else if (rule.DefaultAction == DefaultMessageAction.No)
                { _renderer.EmpireMessageNoClick(this, new DWEventArgs(message)); }
                else if (rule.DefaultAction == DefaultMessageAction.Show)
                { _renderer.EmpireMessageShowClick(this, new DWEventArgs(message)); }
                _alreadyRunning = _StoppedValue;
            }
            return res;
        }
    }
}
