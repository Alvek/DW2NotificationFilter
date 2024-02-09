using DistantWorlds.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationFilter
{
    public class Rules
    {
        public string RuleSetName { get; set; }
        public List<Rule> RuleList { get; set; }
    }
    public class Rule
    {
        public EmpireMessageType MessageType { get; set; }
        public ShipRole TargetObjRole { get; set; }
    }
}
