using UnityEngine;

namespace CustAnim.Editor
{
    public class ShowAttribute : PropertyAttribute
    {
        private readonly ActionOnConditionFail _action;
        private readonly InverseCondition _inverse;
        private readonly string _condition;

        public ActionOnConditionFail Action => _action;
        public InverseCondition Inserse => _inverse;
        public string Condition => _condition;

        public ShowAttribute(ActionOnConditionFail action, InverseCondition inverse, string condition)
        {
            _action = action;
            _inverse = inverse;
            _condition = condition;
        }
    }
    public enum InverseCondition { No, Yes }
    public enum ActionOnConditionFail { DoNotDraw, Disable }
}