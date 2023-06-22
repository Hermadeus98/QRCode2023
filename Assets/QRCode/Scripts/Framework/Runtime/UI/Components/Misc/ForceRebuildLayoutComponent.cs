namespace QRCode.Framework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Scripting;
    using UnityEngine.UI;

    public class ForceRebuildLayoutComponent : MonoBehaviour
    {
        private void Start()
        {
            ForceRebuildLayout();
        }

        [Preserve][Button]
        public void ForceRebuildLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
