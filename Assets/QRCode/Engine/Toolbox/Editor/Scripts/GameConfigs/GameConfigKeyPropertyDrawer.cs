namespace QRCode.Engine.Toolbox.Editor.GameConfigs
{
    using System;
    using System.Collections.Generic;
    using Toolbox.GameConfigs;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(GameConfigKeyAttribute))]
    public class GameConfigKeyPropertyDrawer : PropertyDrawer
    {
        private int index = 0;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var allKey = new List<string>();
            if (attribute is GameConfigKeyAttribute gameConfigKeyAttribute)
            {
                GameConfigBase config = GameConfigs.Instance.GetCatalogOfType(gameConfigKeyAttribute.Type);
                var allGameConfigs = config.GetGameConfigData();

                for (int i = 0; i < allGameConfigs.Count; i++)
                {
                    allKey.Add(allGameConfigs[i].Name);
                }
            }
            
            if (string.IsNullOrEmpty(property.stringValue) == false)
            {
                index = allKey.FindIndex(w => w == property.stringValue);
            }
            else
            {
                index = 0;
            }
            
            EditorGUILayout.BeginVertical("box");
            {
                index = EditorGUILayout.Popup(property.name, index, allKey.ToArray());
            }
            EditorGUILayout.EndVertical();

            property.stringValue = allKey[index];
        }

        private dynamic ConvertObject(object input, Type t) {
            return Convert.ChangeType(input, t);
        }
    }
}
