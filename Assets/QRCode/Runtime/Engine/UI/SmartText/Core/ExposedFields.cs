namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    public static class ExposedFields
    {
        private static List<ExposedFieldInfo> m_exposedMembers;
        public static List<ExposedFieldInfo> ExposedMembers => m_exposedMembers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            m_exposedMembers = FindAllExposedFields().ToList();
        }

        public static ExposedFieldInfo[] FindAllExposedFields()
        {
            List<ExposedFieldInfo> exposedFieldInfos = new List<ExposedFieldInfo>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
                    PropertyInfo[] propertyInfos = type.GetProperties(flags);

                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        if (propertyInfo.CustomAttributes.ToArray().Length > 0)
                        {
                            ExposedFieldAttribute attribute = propertyInfo.GetCustomAttribute<ExposedFieldAttribute>();
                            if (attribute != null)
                            {
                                exposedFieldInfos.Add(new ExposedFieldInfo(propertyInfo, attribute));
                            }
                        }
                    }
                }
            }

            return exposedFieldInfos.ToArray();
        }
    }

    public struct ExposedFieldInfo
    {
        public PropertyInfo PropertyInfo { get; }
        public ExposedFieldAttribute ExposedFieldAttribute { get; }

        public ExposedFieldInfo(PropertyInfo propertyInfo, ExposedFieldAttribute exposedFieldAttribute)
        {
            PropertyInfo = propertyInfo;
            ExposedFieldAttribute = exposedFieldAttribute;
        }
    }
}
