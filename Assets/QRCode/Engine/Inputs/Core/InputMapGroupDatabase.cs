namespace QRCode.Framework
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = K.DatabasePath.Inputs + "Input Maps Groups Database", fileName = "DB_InputMapGroup")]
    public class InputMapGroupDatabase : ScriptableObjectDatabase<InputMapGroupData>
    {
        
    }

    [Serializable]
    public class InputMapGroupData
    {
        [SerializeField] private List<string> m_actionMaps = new List<string>();
        
        public List<string> ActionMaps => m_actionMaps;
    }
}
