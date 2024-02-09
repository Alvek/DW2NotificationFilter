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
        public List<BaseRule> RuleList { get; set; } = [];
    }
    public class BaseRule
    {
        public EmpireMessageType MessageType { get; set; }
        public ShipRole RelatedObjType { get; set; }
        public ItemType RelatedItemType { get; set; }
    }
    public RelatedItemRules
}
