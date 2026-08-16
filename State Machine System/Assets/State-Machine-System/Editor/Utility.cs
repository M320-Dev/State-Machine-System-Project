using System;
using System.Reflection;
using UnityEditor;

namespace M320.StateMachineSystem.Editor
{
    public static class Utility 
    {
        public static T GetLeafTargetObject<T>(SerializedProperty property) 
        {
            return (T)GetLeafTargetObject(property);
        }
        public static object GetLeafTargetObject(SerializedProperty property)
        {
            object targetObject = property.serializedObject.targetObject;
            string propertyPath = property.propertyPath.Replace(".Array.data", "");
            string[] path = propertyPath.Split('.');

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (string name in path)
            {
                if (TryGetArrayIndex(name, out string arrayName, out int index))
                {
                    FieldInfo fieldInfo = FindFieldInfoWithinAncestry(targetObject.GetType(), arrayName, bindingFlags);
                    Array targetArray = (Array)fieldInfo.GetValue(targetObject);
                    targetObject = targetArray.GetValue(index);
                }
                else
                {
                    FieldInfo fieldInfo = FindFieldInfoWithinAncestry(targetObject.GetType(), name, bindingFlags);
                    targetObject = fieldInfo.GetValue(targetObject);
                }
            }

            return targetObject;
        }

        private static FieldInfo FindFieldInfoWithinAncestry(Type type, string name, BindingFlags bindingFlags) 
        {
            bindingFlags |= BindingFlags.DeclaredOnly;
            while (type != null)
            {
                FieldInfo fieldInfo = type.GetField(name, bindingFlags);

                if (fieldInfo != null) return fieldInfo;

                type = type.BaseType;
            }
            return null;
        }

        private static bool TryGetArrayIndex(string name, out string arrayName, out int index)
        {
            arrayName = name;
            index = -1;

            int startIndex = name.IndexOf('[');

            if (startIndex == -1) return false;

            arrayName = name[..startIndex];

            string substring = name[(startIndex + 1)..^1];
            index = int.Parse(substring);

            return true;
        }
    }
}
