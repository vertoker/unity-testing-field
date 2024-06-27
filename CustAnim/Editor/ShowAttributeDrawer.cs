using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CustAnim.Editor
{
    [CustomPropertyDrawer(typeof(ShowAttribute), true)]
    public class ShowAttributeDrawer : PropertyDrawer
    {
        #region Reflection helpers
        private static MethodInfo GetMethod(object target, string methodName)
        {
            return GetAllMethods(target, m => m.Name.Equals(methodName,
                      StringComparison.InvariantCulture)).FirstOrDefault();
        }
        private static FieldInfo GetField(object target, string fieldName)
        {
            return GetAllFields(target, f => f.Name.Equals(fieldName,
                  StringComparison.InvariantCulture)).FirstOrDefault();
        }
        private static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
        {
            List<Type> types = new List<Type>()
            {
                target.GetType()
            };

            while (types.Last().BaseType != null)
            {
                types.Add(types.Last().BaseType);
            }

            for (int i = types.Count - 1; i >= 0; i--)
            {
                IEnumerable<FieldInfo> fieldInfos = types[i]
                    .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(predicate);

                foreach (var fieldInfo in fieldInfos)
                {
                    yield return fieldInfo;
                }
            }
        }
        private static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
        {
            IEnumerable<MethodInfo> methodInfos = target.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(predicate);
            return methodInfos;
        }
        #endregion

        private bool MeetsConditions(SerializedProperty property)
        {
            var showAttribute = attribute as ShowAttribute;
            var target = property.serializedObject.targetObject;
            FieldInfo conditionField = GetField(target, showAttribute.Condition);
            
            if (conditionField != null && conditionField.FieldType == typeof(bool))
            {
                var met = (bool)conditionField.GetValue(target);
                return showAttribute.Inserse == InverseCondition.Yes ? met : !met;
            }
            
            Debug.LogError("Invalid boolean condition");
            return true;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool meetsCondition = MeetsConditions(property);
            var showIfAttribute = attribute as ShowAttribute;

            if (!meetsCondition && showIfAttribute.Action == ActionOnConditionFail.DoNotDraw)
                return 0;
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (MeetsConditions(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var showIfAttribute = attribute as ShowAttribute;
            if (showIfAttribute.Action == ActionOnConditionFail.Disable)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(position, property, label, true);
                EditorGUI.EndDisabledGroup();
            }

        }
    }
}