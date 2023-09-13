namespace QRCode.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using QRCode.Engine.Core.GameInstance;
    using QRCode.Engine.Core.Inputs;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.InputSystem;


    public class ActionMapDebuggerEditor : EditorWindow
    {
        private InputManager m_inputManagementService = null;
        private int indent;
        
        private class ActionMapDebuggerInfo
        {
            public bool IsExtended;
            public bool IsActivate;
        }

        private bool isInit = false;
        
        private Dictionary<InputActionMap, ActionMapDebuggerInfo> m_allInfos;

        [MenuItem("QRCode/Inputs/Action Map Debugger")]
        private static void Open()
        {
            var window = GetWindow<ActionMapDebuggerEditor>();
            window.name = "Action Map Debugger";
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        
        protected void OnGUI()
        {
            if (Application.isPlaying == false)
            {
                GUILayout.Label("The debugger is available only in editor.");
                isInit = false;
                return;
            }

            if (GameInstance.Instance.IsReady == false)
            {
                return;
            }

            indent = EditorGUI.indentLevel;

            if (isInit == false)
            {
                m_allInfos = new Dictionary<InputActionMap, ActionMapDebuggerInfo>();
                m_inputManagementService = InputManager.Instance;
                isInit = true;
                
                var actionsActionMaps = m_inputManagementService.GetPlayerInput().actions.actionMaps;

                for (int i = 0; i < actionsActionMaps.Count; i++)
                {
                    m_allInfos.Add(actionsActionMaps[i], new ActionMapDebuggerInfo());
                }
            }

            GUILayout.BeginVertical("box");
            for (int i = 0; i < m_allInfos.Count; i++)
            {
                DrawInputActionMapInfos(m_allInfos.ElementAt(i).Key);
            }
            GUILayout.EndVertical();
        }

        private void DrawInputActionMapInfos(InputActionMap actionsActionMap)
        {
            GUILayout.BeginVertical("box");
            var headStyle = new GUIStyle
            {
                fontSize = 15,
                richText = true,
                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                },
                fontStyle = FontStyle.Bold
            };
            
            GUILayout.BeginHorizontal("box");
            GUILayout.Label( actionsActionMap.name, headStyle);

            var buttonStyle = new GUIStyle(EditorStyles.miniButton);
            m_allInfos[actionsActionMap].IsActivate = actionsActionMap.enabled;
            var color = actionsActionMap.enabled ? new Color(0f, 0.31f, 0f) : new Color(0.31f, 0f, 0f);
            buttonStyle.normal.background = MakeBackgroundTexture(10, 10, color);
            var hoverColor = actionsActionMap.enabled ? new Color(0f, 0.58f, 0f) : new Color(0.58f, 0f, 0f);
            buttonStyle.hover.background = MakeBackgroundTexture(10, 10, hoverColor);
                
            if (GUILayout.Button("Activate", buttonStyle))
            {
                if (m_allInfos[actionsActionMap].IsActivate)
                {
                    m_inputManagementService.SetActionMapDisable(actionsActionMap.name);
                    m_allInfos[actionsActionMap].IsActivate = false;
                }
                else
                {
                    m_inputManagementService.SetActionMapEnable(actionsActionMap.name);
                    m_allInfos[actionsActionMap].IsActivate = true;
                }
            }
            
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel = indent + 45;
            for (int i = 0; i < actionsActionMap.actions.Count; i++)
            {
                DrawActionInfos(actionsActionMap.actions[i]);
            }
            EditorGUI.indentLevel = indent;
            GUILayout.EndVertical();
        }

        private void DrawActionInfos(InputAction inputAction)
        {
            GUILayout.Label(inputAction.name);
        }
        
        private Texture2D MakeBackgroundTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
 
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
 
            Texture2D backgroundTexture = new Texture2D(width, height);
 
            backgroundTexture.SetPixels(pixels);
            backgroundTexture.Apply();
 
            return backgroundTexture;
        }
    }
}
