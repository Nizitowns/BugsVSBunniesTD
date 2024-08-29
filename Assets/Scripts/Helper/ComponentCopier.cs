using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Helper
{
    public static class ComponentCopier
    {
        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();

            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;

            var fields = GetAllFields(type);
            foreach (var field in fields)
            {
                if (field.IsStatic || IsObsolete(field)) continue;
                field.SetValue(dst, field.GetValue(original));
            }

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name" || IsObsolete(prop)) continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }

            return dst as T;
        }

        public static IEnumerable<FieldInfo> GetAllFields(System.Type t)
        {
            if (t == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Static | BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;
            return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
        }

        private static bool IsObsolete(MemberInfo member)
        {
            var obsoleteAttr = member.GetCustomAttribute<ObsoleteAttribute>();
            return obsoleteAttr != null;
        }
    }
}