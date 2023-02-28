namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Localization.Components;
    using UnityEngine.Scripting;

    /// <summary>
    /// FROM https://unitycopilot.com/videos/view/vLKeqS1PeTU
    /// </summary>
    public class SmartText : SerializedMonoBehaviour
    {
        [SerializeField] private ExposedValueSelector[] m_exposedValues;
        [SerializeField] private TextMeshProUGUI m_text = null;
        
        private List<string> m_instancedValues;
        private static bool m_isInit = false;

        private static Dictionary<string, PropertyInfo> m_fieldNameDictionary;

        private void Start()
        {
            UpdateText();
        }

        [Preserve]
        public void UpdateText()
        {
            CreateDictionary();
            m_text.SetText(GetFormattedString());
        }

        private string GetFormattedString()
        {
            m_instancedValues = new List<string>();
            for (int i = 0; i < m_exposedValues.Length; i++)
            {
                PropertyInfo propertyInfo = m_fieldNameDictionary[m_exposedValues[i].FieldName];
                string value = GetValue(propertyInfo);
                m_instancedValues.Add(value);
            }

            return string.Format(m_text.text, m_instancedValues.ToArray());
        }

        private string GetValue(PropertyInfo propertyInfo)
        {
            object obj = null;
            string value = "MISSINGS!";
            
            obj = propertyInfo.GetValue(null);
            if (obj != null)
            {
                value = obj.ToString();
            }

            return value;
        }

        private void CreateDictionary()
        {
            if (m_isInit)
            {
                return;
            }
            
            m_fieldNameDictionary = new Dictionary<string, PropertyInfo>();

            var fieldsName = ExposedFields.FindAllExposedFields();

            foreach (var field in fieldsName)
            {
                m_fieldNameDictionary.Add(field.PropertyInfo.Name, (PropertyInfo)field.PropertyInfo);
            }

            m_isInit = true;
        }
    }
}
