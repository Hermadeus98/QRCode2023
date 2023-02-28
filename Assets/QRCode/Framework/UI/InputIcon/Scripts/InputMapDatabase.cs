namespace QRCode.Framework
{
    using System;
    using Debugging;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.DatabasePath.BasePath + "Input Maps Database", fileName = "BD_InputMaps")]
    public class InputMapDatabase : ScriptableObjectDatabase<InputMap>
    {
        
    }

    [Serializable]
    public class InputMap
    {
        [SerializeField] private string m_mapScheme;
        [TableList]
        public InputMapElement[] InputMapElements;

        public Sprite FindIcon(string inputName)
        {
            for (int i = 0; i < InputMapElements.Length; i++)
            {
                if (inputName == InputMapElements[i].InputName)
                {
                    return InputMapElements[i].InputIcon;
                }
            }

            QRDebug.DebugError(K.DebuggingChannels.Inputs, $"Cannot find InputMapElement with {inputName} in {m_mapScheme}.");
            return null;
        }
    }
    
    [Serializable]
    public class InputMapElement
    {
        public string InputName;
        [PreviewField(ObjectFieldAlignment.Center)]
        public Sprite InputIcon;
    }
}
