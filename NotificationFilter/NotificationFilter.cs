using DistantWorlds.Types;
using DistantWorlds.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationFilter
{
    internal class NotificationFilter
    {
        public Rules Rules {get; set;} = new Rules();

        private ReaderWriterLockSlim _ruleListLocker = new(LockRecursionPolicy.NoRecursion);


        public void Filter(ref bool needToFilterMEssage, EmpireMessage message)
        {
            //__result = false;
            //return;
            bool foundRule = false;
            BaseRule activatedRule = null;
            if (needToFilterMEssage)
            {
                _ruleListLocker.EnterReadLock();
                foreach (var item in Rules.RuleList)
                {
                    if (item.MessageType == message.Type)
                    {
                        activatedRule = item;
                        foundRule = true;
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
                                    foundRule = true;
                                    break;
                                }
                            }
                        }
                        if (foundRule)
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
                                    foundRule = true;
                                    break;
                                }
                            }
                        }
                        if (foundRule)
                        {
                            activatedRule = item;
                            break;
                        }
                    }
                }
                _ruleListLocker.ExitReadLock();
                //todo call filter action
                if (foundRule && Core.DoFilterAction(activatedRule, message))
                {                    
                    needToFilterMEssage = false;
                }
            }
        }

        public void CreateFilterRule(object? sender, DWEventArgs e)
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
                Rules.RuleList.Add(rule);
                _ruleListLocker.ExitWriteLock();
            }
        }
    }
}
