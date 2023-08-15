namespace QRCode.Engine.Core.Inputs
{
    using UnityEngine;

    using System;
    
    using Sirenix.OdinInspector;
    using TMPro;

    using Toolbox;
    using Debugging;
    using Toolbox.Database;
    using Constants = Toolbox.Constants;

    [CreateAssetMenu(menuName = Constants.DatabasePath.Inputs + "Input Maps Database", fileName = "DB_InputMaps")]
    public class InputMapDatabase : ScriptableObjectDatabase<InputMap>
    {
        protected override string m_databaseInformation { get => "An input map represents all information about an game controller, like input icons."; }
    }

    [Serializable]
    public class InputMap
    {
        [SerializeField] private string m_mapScheme;
        [SerializeField] private TMP_SpriteAsset m_iconsSpriteAsset = null;
        
        [TableList]
        [SerializeField] private InputMapElement[] m_inputMapElements;

        public TMP_SpriteAsset IconsSpriteAsset => m_iconsSpriteAsset;
        
        private InputSettings m_inputSettings = null;
        private InputSettings InputSettings
        {
            get
            {
                if (m_inputSettings == null)
                {
                    m_inputSettings = InputSettings.Instance;
                }

                return m_inputSettings;
            }
        }

        public Sprite FindIcon(string inputName, int alternativeInputIconIndex = 0)
        {
            if (string.IsNullOrEmpty(inputName))
            {
                return InputSettings.NotFoundedIconSprite;
            }
            
            for (int i = 0; i < m_inputMapElements.Length; i++)
            {
                if (inputName == m_inputMapElements[i].InputName)
                {
                    if (m_inputMapElements[i].InputIcons.Length - 1 > alternativeInputIconIndex)
                    {
                        QRDebug.DebugError(Constants.DebuggingChannels.Inputs, $"There is no icon at index {alternativeInputIconIndex} in {inputName} in {m_mapScheme}");
                        return m_inputMapElements[i].InputIcons[0];
                    }
                    
                    return m_inputMapElements[i].InputIcons[alternativeInputIconIndex];
                }
            }

            QRDebug.DebugError(Constants.DebuggingChannels.Inputs, $"Cannot find InputMapElement with [{inputName}] in {m_mapScheme}.");
            return InputSettings.NotFoundedIconSprite;
        }

        public int FindTextMeshProSpriteSheetIndex(string inputName)
        {
            for (int i = 0; i < m_inputMapElements.Length; i++)
            {
                if (inputName == m_inputMapElements[i].InputName)
                {
                    return m_inputMapElements[i].TextMeshProSpriteSheetIndex;
                }
            }
            QRDebug.DebugError(Constants.DebuggingChannels.Inputs, $"Cannot find InputMapElement with {inputName} in {m_mapScheme}.");
            return -1;
        }
    }
    
    [Serializable]
    public class InputMapElement
    {
        public string InputName;
        [PreviewField(ObjectFieldAlignment.Center)]
        public Sprite[] InputIcons;
        public int TextMeshProSpriteSheetIndex;
    }
}
