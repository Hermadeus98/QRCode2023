namespace QRCode.Editor
{
    using Framework;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class InputIconsDatabaseWindowEditor : OdinEditorWindow
    {
        [SerializeField][InlineEditor(InlineEditorModes.FullEditor)] private InputMapDatabase m_inputMapDatabase = null;

        [MenuItem("QRCode/Input Icons Database")]
        private static void OpenWindow()
        {
            GetWindow<InputIconsDatabaseWindowEditor>().Show();
        }

        protected override void Initialize()
        {
            base.Initialize();
            DB.Instance.TryGetDatabase(DBEnum.DB_InputMaps, out m_inputMapDatabase);
        }
    }
}
