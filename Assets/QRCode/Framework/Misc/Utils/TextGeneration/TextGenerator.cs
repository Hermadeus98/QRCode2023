namespace QRCode.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public static class TextGenerator
    {
        public const char OPEN_BRACKET = '{';
        public const char CLOSE_BRACKET = '}';
        public const string TAB = "    ";
        public const string DOUBLE_TAB = TAB + TAB;
        
        public static TextAsset GenerateCSEnum(string enumPath, string enumName, string enumNamespace, List<string> fields)
        {
            var selectionIdGUIDs = Selection.assetGUIDs;

            if (selectionIdGUIDs.Length == 0)
                return null;

            var sanitizePath = enumPath.Substring(6);
            var path = Application.dataPath + sanitizePath;
            Debug.Log($"{enumName} was generated at path : {path}.");

            var body = DOUBLE_TAB + "Undefined = 0" + $",\n";
            for (int i = 0; i < fields.Count; i++)
            {
                body += DOUBLE_TAB + fields[i] + " = " + (i + 1).ToString() + ',' + $"\n";
            }

            var content = GenerateCSEnumBody(enumNamespace, enumName, body);
            
            var fullPath = path + $"/{enumName}.cs";
            File.WriteAllText(fullPath, content);
            EditorUtility.RequestScriptReload();

            return AssetDatabase.LoadAssetAtPath<TextAsset>(fullPath);
        }
        
        private static string GenerateCSEnumBody(string namespaceName, string fileName, string body)
        {
            return $"" +
                  $"namespace {namespaceName}\n" +
                  $"{OPEN_BRACKET}\n" +
                  $"\n" +
                  $"// MACHINE-GENERATED CODE - DO NOT MODIFY BY HAND!\n" +
                  $"{TAB}public enum {fileName}\n" +
                  $"{TAB}{OPEN_BRACKET}\n" +
                  $"{body}" +
                  $"{TAB}{CLOSE_BRACKET}\n" +
                  $"{CLOSE_BRACKET}\n";
        }
    }
}
