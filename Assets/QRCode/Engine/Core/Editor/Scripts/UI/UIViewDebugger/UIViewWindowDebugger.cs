namespace QRCode.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using Engine.Core.UI;
    using UnityEditor;
    using UnityEngine;

    public class UIViewWindowDebugger : EditorWindow
    {
        private struct UIViewEditorInfo
        {
            public bool IsExtended;
            public UIView UIView;
        }

        private bool isInit = false;
        private Dictionary<string, UIViewEditorInfo> m_uiViewEditorInfos;

        [MenuItem("QRCode/UI/IUViewDebugger")]
        private static void Init()
        {
            var window = (UIViewWindowDebugger)EditorWindow.GetWindow(typeof(UIViewWindowDebugger));
            window.titleContent = new GUIContent("UI View Debugger");
            window.Show();
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("You must be in play mode to use debugger.");
                isInit = false;
                return;
            }

            if (!isInit)
            {
                m_uiViewEditorInfos = new Dictionary<string, UIViewEditorInfo>();
                isInit = true;
            }

            if (UI.UIViewDatabase == null)
            {
                return;
            }
            
            var allUIView = UI.UIViewDatabase.GetDatabase.Values;
            
            var viewCount = allUIView.Count;
            for (var i = 0; i < viewCount; i++)
            {
                GUILayout.BeginVertical("Box");

                var view = allUIView.ElementAt(i);
                if (!m_uiViewEditorInfos.ContainsKey(view.ViewName))
                {
                    m_uiViewEditorInfos.Add(view.ViewName, new UIViewEditorInfo()
                    {
                        IsExtended = false,
                        UIView = view,
                    });
                }

                var viewEditorInfo = m_uiViewEditorInfos[view.ViewName];
                
                var iconContent = new GUIContent();
                iconContent = view.isActiveAndEnabled
                    ? EditorGUIUtility.IconContent("d_winbtn_mac_close")
                    : EditorGUIUtility.IconContent("d_winbtn_mac_min");

                var foldoutRect = new Rect(3, 3 * i + 2, position.width - 6, 15);
                viewEditorInfo.IsExtended = EditorGUI.Foldout(foldoutRect, viewEditorInfo.IsExtended, view.ViewName);
                
                var iconRect = foldoutRect;
                iconRect.width = iconContent.image.width + 2f;
                foldoutRect.width -= iconRect.width;
                EditorGUI.PrefixLabel(iconRect, iconContent);


                if (viewEditorInfo.IsExtended)
                {
                    GUILayout.Label($"Is Active : {view.isActiveAndEnabled}");
                }

                GUILayout.EndVertical();
            }
        }
    }
}
