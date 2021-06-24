using System;
using UnityEngine;

namespace Game.Map2D
{
    public class ShowIfAttribute : PropertyAttribute
    {
        private ActionOnConditionFail _action;
        private ConditionOperator _operator;
        private string[] _conditions;

        public ActionOnConditionFail Action => _action;
        public ConditionOperator Operator => _operator;
        public string[] Conditions => _conditions;

        public ShowIfAttribute(ActionOnConditionFail @action, ConditionOperator @operator, params string[] @conditions)
        {
            _action = @action;
            _operator = @operator;
            _conditions = @conditions;
        }
    }
    public enum ConditionOperator { And, Or }
    public enum ActionOnConditionFail { DoNotDraw, Disable }
}