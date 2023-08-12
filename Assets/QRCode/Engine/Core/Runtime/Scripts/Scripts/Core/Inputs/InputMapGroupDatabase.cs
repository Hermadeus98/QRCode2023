namespace QRCode.Engine.Core.Inputs
{
    using System;
    using System.Collections.Generic;
    using Toolbox;
    using Toolbox.Database;
    using UnityEngine;

    [CreateAssetMenu(menuName = Constants.DatabasePath.Inputs + "Input Maps Groups Database", fileName = "DB_InputMapGroup")]
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
