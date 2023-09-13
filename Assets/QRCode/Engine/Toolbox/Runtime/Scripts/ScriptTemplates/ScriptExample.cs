namespace QRCode.Engine.Toolbox
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    
    public class ScriptExample : MonoBehaviour
    {
        [SuffixLabel("s")]
        [SerializeField] private float m_duration = 0.0f;
        
        [SuffixLabel("$")]
        [SerializeField] private int m_currency = 0;
    }
}
