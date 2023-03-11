using System.Collections.Generic;
using System.Reflection;
using QRCode.Framework;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ExposedValueSelector))]
public class ExposedFieldPropertyDrawer : PropertyDrawer
{
    private bool gotFields = false;
    private List<PropertyInfo> m_propertyInfos;
    private List<string> m_propertyNames;

    private int index;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!gotFields)
        {
            GetFields();
        }

        SerializedProperty fieldNameProperty = property.FindPropertyRelative("FieldName");

        index = GetFieldsName(fieldNameProperty.stringValue);
        index = EditorGUI.Popup(position, index, m_propertyNames.ToArray());
        fieldNameProperty.stringValue = m_propertyInfos[index].Name;
    }

    private int GetFieldsName(string value)
    {
        string fieldName = value;
        int count = 0;

        foreach (var property in m_propertyInfos)
        {
            if (property.Name == fieldName)
            {
                return count;
            }

            count++;
        }

        return 0;
    }
    
    private void GetFields()
    {
        m_propertyInfos = new List<PropertyInfo>();
        m_propertyNames = new List<string>();
        var exposedMembers = ExposedFields.FindAllExposedFields();
            
        foreach (var member in exposedMembers)
        {
            MemberInfo memberInfo = member.PropertyInfo;
            ExposedFieldAttribute attribute = member.ExposedFieldAttribute;

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                PropertyInfo property = (PropertyInfo)memberInfo;

                object obj = null;
                string value = "N/A";
                        
                obj = property.GetValue(null);
                if (obj != null)
                {
                    value = obj.ToString();
                }
                
                m_propertyInfos.Add(member.PropertyInfo);
                m_propertyNames.Add($"{memberInfo.ReflectedType}/{attribute.DisplayName}");
                //Debug.Log(attribute.DisplayName + " = " + value);
            }
        }
        
        gotFields = true;
    }
}
