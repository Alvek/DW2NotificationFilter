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
        public List<RelatedItemRules> ItemRules { get; set; } = [];
        public List<RelatedObjectRules> ObjectRules { get; set; } = [];
        public DefaultMessageAction DefaultAction { get; set; }
    }
    public class RelatedItemRules
    {
        public int Id { get; set; }
        public ItemType RelatedItemType { get; set; }

    }
    public class RelatedObjectRules
    {
        public ItemType RelatedObjectType { get; set; }
        //public ShipRole RelatedObjType { get; set; }

    }
}
